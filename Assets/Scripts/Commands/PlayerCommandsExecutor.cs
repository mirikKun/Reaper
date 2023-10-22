using System;
using System.Collections.Generic;
using Cinemachine;
using Players;
using Pools;
using UnityEngine;
using Zenject;

namespace Commands
{
    public class PlayerCommandsExecutor : MonoBehaviour
    {
        [SerializeField] private PlayerMover playerMover;
        [SerializeField] private CircleAttack circleAttack;
        [SerializeField] private ReflectAttack reflectAttack;

        [SerializeField] private DestinationsPool destinationPool;
        [SerializeField] private AttackMarkersPool circleAttackPool;
        [SerializeField] private AttackMarkersPool reflectAttackPool;
        [SerializeField] private AttackMarker revertAttackMarker;
        [SerializeField] private int maxCommandCount = 5;

        [SerializeField] private float commandReductionSpeed = 0.3f;
        private float reductionProgress;
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
        public Vector3 _lastPos;
        public Vector3 _startPos;
        private bool _isExecuting = false;
        private int _currentCommandCount;

        [Inject]
        public void Construct(PlayerMarkers playerMarkers)
        {
            destinationPool = playerMarkers.DestinationsPool;
            circleAttackPool = playerMarkers.CircleAttackMarkersPool;
            reflectAttackPool = playerMarkers.ReflectMarkersPool;
            revertAttackMarker = playerMarkers.ReverseAttackMarkersPool;
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
            else if (_currentCommandCount == 0 && reductionProgress < maxCommandCount)
            {
                ChangeReductionProgress(Time.deltaTime * commandReductionSpeed);
                OnCommandProgressReduction?.Invoke(reductionProgress / maxCommandCount);
                if (reductionProgress >= maxCommandCount)
                {
                    OnCommandsReady?.Invoke();
                }
            }
        }

        public void SetStartValues()
        {
            playerMover.FastMoving = false;

            _currentCommandCount = 0;

            _todoCommands.Clear();
            _undoCommands.Clear();
            destinationPool.RevertAllToPool();
            circleAttackPool.RevertAllToPool();
            reflectAttackPool.RevertAllToPool();
            _isExecuting = false;
            _lastPos = playerMover.GetPos();
            _lastPos = new Vector3(_lastPos.x, 0.2f, _lastPos.z);
            destinationPool.SetupPool(maxCommandCount);
            circleAttackPool.SetupPool(maxCommandCount);
            reflectAttackPool.SetupPool(maxCommandCount);
            reductionProgress = maxCommandCount;
        }

        private bool CanAddNewCommand()
        {
            return (_currentCommandCount < maxCommandCount && !(reductionProgress < maxCommandCount));
        }

        public void AddMoveToCommand(Vector3 pos)
        {
            if (!CanAddNewCommand())
            {
                return;
            }

            pos = CalculatePossiblePosition(new Ray(_lastPos, pos - _lastPos), pos);
            var destination = destinationPool.GetElement();
            destination.PlaceDestination(_lastPos, pos);
            OnMoveCommandAdding?.Invoke(destination.transform);

            AddCommand(new MoveToCommand(playerMover, _lastPos, pos));
            _lastPos = pos;
        }

        public void AddCircleAttackCommand()
        {
            if (!CanAddNewCommand())
            {
                return;
            }

            circleAttackPool.GetElement().SetPosition(_lastPos);
            AddCommand(new CircleAttackCommand(circleAttack));
        }

        public void AddReflectAttackCommand()
        {
            if (!CanAddNewCommand())
            {
                return;
            }

            reflectAttackPool.GetElement().SetPosition(_lastPos);
            AddCommand(new ReflectAttackCommand(reflectAttack));
        }

        public void AddReverseAttackCommand()
        {
            if (!CanAddNewCommand() || _currentCommandCount < 1)
            {
                return;
            }

            revertAttackMarker.gameObject.SetActive(true);
            revertAttackMarker.SetPosition(_lastPos);
            ICommand[] commands = _todoCommands.ToArray();
            Array.Reverse(commands);
            _todoCommands.Enqueue(new ChangeSpeedCommand(playerMover));

            AddCommandsQueue(commands);
            _todoCommands.Enqueue(new ChangeSpeedCommand(playerMover));
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

            OnCommandAdding?.Invoke(maxCommandCount - _currentCommandCount);
        }

        private void AddCommand(ICommand command)
        {
            _todoCommands.Enqueue(command);
            _currentCommandCount++;
            OnCommandAdding?.Invoke(maxCommandCount - _currentCommandCount);
        }

        private void ChangeReductionProgress(float change)
        {
            reductionProgress += change;
            reductionProgress = Mathf.Clamp(reductionProgress, 0, maxCommandCount);
        }

        public Vector3 CalculatePossiblePosition(Ray ray, Vector3 newPos)
        {
            RaycastHit hit;
            Vector3 pos = newPos;
            if (Physics.Raycast(ray, out hit, playerMover.MaxDistance, ObstacleLayer))
            {
                pos = hit.point;
            }
            else if ((_lastPos - newPos).magnitude > playerMover.MaxDistance)
            {
                Vector3 vector = _lastPos - newPos;
                var factor = playerMover.MaxDistance / vector.magnitude;
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
            playerMover.FastMoving = false;

            _isExecuting = false;

            destinationPool.RevertAllToPool();
            circleAttackPool.RevertAllToPool();
            reflectAttackPool.RevertAllToPool();
            revertAttackMarker.gameObject.SetActive(false);

            OnExecutionEnd?.Invoke();
        }


        private void ExecuteNewCommand(ICommand commandButton)
        {
            commandButton.Execute();
            _undoCommands.Push(commandButton);
        }
    }
}