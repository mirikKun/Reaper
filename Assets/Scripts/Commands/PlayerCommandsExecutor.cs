using System;
using System.Collections.Generic;
using Cinemachine;
using Players;
using Pools;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Commands
{
    public class PlayerCommandsExecutor : MonoBehaviour
    {
        [SerializeField] private PlayerMover _playerMover;
        [SerializeField] private CircleAttack _circleAttack;
        [SerializeField] private ReflectAttack _reflectAttack;

        [SerializeField] private DestinationsPool _destinationPool;
        [SerializeField] private AttackMarkersPool _circleAttackPool;
        [SerializeField] private AttackMarkersPool _reflectAttackPool;
        [SerializeField] private AttackMarker _revertAttackMarker;
        [SerializeField] private int _maxCommandCount = 5;

        [SerializeField] private float _commandReductionSpeed = 0.3f;
        private float _reductionProgress;
        private const int ObstacleLayer = 1 << 9;

        public event Action OnExecutionStart;
        public event Action OnExecutionEnd;
        public event Action OnCommandsReady;
        public event Action<int> OnCommandAdding;
        public event Action<Transform> OnMoveCommandAdding;
        public event Action<float> OnCommandProgressReduction;
        private Queue<ICommand> _todoCommands = new Queue<ICommand>();

        private Stack<ICommand> _undoCommands = new Stack<ICommand>();

        private ICommand _currentCommand;
        private IGameInput _gameInput;
        public Vector3 _lastPos;
        public Vector3 _startPos;
        private bool _isExecuting = false;
        private int _currentCommandCount;

        [Inject]
        public void Construct(PlayerMarkers playerMarkers, IGameInput gameInput)
        {
            ConstructMarkers(playerMarkers);
            ConstructInputs(gameInput);
        }

        private void Start()
        {
            SetStartValues();
        }

        private void Update()
        {
            if (_isExecuting)
            {
                ProcessCommands();
            }
            else if (_currentCommandCount == 0 && _reductionProgress < _maxCommandCount)
            {
                ChangeReductionProgress(Time.deltaTime * _commandReductionSpeed);
                OnCommandProgressReduction?.Invoke(_reductionProgress / _maxCommandCount);
                if (_reductionProgress >= _maxCommandCount)
                {
                    OnCommandsReady?.Invoke();
                }
            }
        }

        private void OnDestroy()
        {
            _gameInput.OnExecutionClick -= StartExecuting;
            _gameInput.OnAreaClick -= AddMoveToCommand;
            _gameInput.OnCircleAttackClick -= AddCircleAttackCommand;
            _gameInput.OnReflectAttackClick -= AddReflectAttackCommand;
            _gameInput.OnReverseAttackClick -= AddReverseAttackCommand;
        }

        public void SetStartValues()
        {
            _playerMover.FastMoving = false;
            _isExecuting = false;

            _currentCommandCount = 0;
            _todoCommands.Clear();
            _undoCommands.Clear();

            _destinationPool.RevertAllToPool();
            _circleAttackPool.RevertAllToPool();
            _reflectAttackPool.RevertAllToPool();

            _lastPos = _playerMover.GetPos();
            _lastPos = new Vector3(_lastPos.x, 0.2f, _lastPos.z);

            _destinationPool.SetupPool(_maxCommandCount);
            _circleAttackPool.SetupPool(_maxCommandCount);
            _reflectAttackPool.SetupPool(_maxCommandCount);

            _reductionProgress = _maxCommandCount;
        }

        private void ConstructInputs(IGameInput gameInput)
        {
            _gameInput = gameInput;
            _gameInput.OnExecutionClick += StartExecuting;
            _gameInput.OnAreaClick += AddMoveToCommand;
            _gameInput.OnCircleAttackClick += AddCircleAttackCommand;
            _gameInput.OnReflectAttackClick += AddReflectAttackCommand;
            _gameInput.OnReverseAttackClick += AddReverseAttackCommand;
        }

        private void ConstructMarkers(PlayerMarkers playerMarkers)
        {
            _destinationPool = playerMarkers.DestinationsPool;
            _circleAttackPool = playerMarkers.CircleAttackMarkersPool;
            _reflectAttackPool = playerMarkers.ReflectMarkersPool;
            _revertAttackMarker = playerMarkers.ReverseAttackMarkersPool;
        }

        private bool CanAddNewCommand()
        {
            return (_currentCommandCount < _maxCommandCount && !(_reductionProgress < _maxCommandCount));
        }

        public void AddMoveToCommand(Vector3 pos)
        {
            if (!CanAddNewCommand())
            {
                return;
            }

            pos = CalculatePossiblePosition(new Ray(_lastPos, pos - _lastPos), pos);
            var destination = _destinationPool.GetElement();
            destination.PlaceDestination(_lastPos, pos);
            OnMoveCommandAdding?.Invoke(destination.transform);

            AddCommand(new MoveToCommand(_playerMover, _lastPos, pos));
            _lastPos = pos;
        }

        public void AddCircleAttackCommand()
        {
            if (!CanAddNewCommand())
            {
                return;
            }

            _circleAttackPool.GetElement().SetPosition(_lastPos);
            AddCommand(new CircleAttackCommand(_circleAttack));
        }

        public void AddReflectAttackCommand()
        {
            if (!CanAddNewCommand())
            {
                return;
            }

            _reflectAttackPool.GetElement().SetPosition(_lastPos);
            AddCommand(new ReflectAttackCommand(_reflectAttack));
        }

        public void AddReverseAttackCommand()
        {
            if (!CanAddNewCommand() || _currentCommandCount < 1)
            {
                return;
            }

            _revertAttackMarker.gameObject.SetActive(true);
            _revertAttackMarker.SetPosition(_lastPos);
            ICommand[] commands = _todoCommands.ToArray();
            Array.Reverse(commands);
            _todoCommands.Enqueue(new ChangeSpeedCommand(_playerMover));

            AddCommandsQueue(commands);
            _todoCommands.Enqueue(new ChangeSpeedCommand(_playerMover));
            _lastPos = _startPos;
            StartExecuting();
        }

        private void AddCommandsQueue(ICommand[] commands)
        {
            _currentCommandCount++;
            foreach (var command in commands)
            {
                if (command is MoveToCommand moveToCommand)
                {
                    _todoCommands.Enqueue(moveToCommand.GetReversedCommand());
                }
            }

            OnCommandAdding?.Invoke(_maxCommandCount - _currentCommandCount);
        }

        private void AddCommand(ICommand command)
        {
            _todoCommands.Enqueue(command);
            _currentCommandCount++;
            OnCommandAdding?.Invoke(_maxCommandCount - _currentCommandCount);
        }

        private void ChangeReductionProgress(float change)
        {
            _reductionProgress += change;
            _reductionProgress = Mathf.Clamp(_reductionProgress, 0, _maxCommandCount);
        }

        public Vector3 CalculatePossiblePosition(Ray ray, Vector3 newPos)
        {
            RaycastHit hit;
            Vector3 pos = newPos;
            if (Physics.Raycast(ray, out hit, _playerMover.MaxDistance, ObstacleLayer))
            {
                pos = hit.point;
            }
            else if ((_lastPos - newPos).magnitude > _playerMover.MaxDistance)
            {
                Vector3 vector = _lastPos - newPos;
                var factor = _playerMover.MaxDistance / vector.magnitude;
                pos = new Vector3(_lastPos.x - vector.x * factor, newPos.y,
                    _lastPos.z - vector.z * factor);
            }

            return pos;
        }

        public void StartExecuting()
        {
            ChangeReductionProgress(-_currentCommandCount);

            _isExecuting = true;
            OnExecutionStart?.Invoke();
        }


        private void ProcessCommands()
        {
            if (_currentCommand != null && _currentCommand.IsFinished == false)
                return;

            if (_todoCommands.Count < 1)
            {
                EndExecution();

                return;
            }

            _currentCommand = _todoCommands.Dequeue();
            ExecuteNewCommand(_currentCommand);
        }

        private void EndExecution()
        {
            _startPos = _lastPos;
            _currentCommandCount = 0;
            _playerMover.FastMoving = false;

            _isExecuting = false;

            _destinationPool.RevertAllToPool();
            _circleAttackPool.RevertAllToPool();
            _reflectAttackPool.RevertAllToPool();
            _revertAttackMarker.gameObject.SetActive(false);

            OnExecutionEnd?.Invoke();
        }


        private void ExecuteNewCommand(ICommand commandButton)
        {
            commandButton.Execute();
            _undoCommands.Push(commandButton);
        }
    }
}