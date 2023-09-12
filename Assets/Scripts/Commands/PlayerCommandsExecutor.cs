using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCommandsExecutor : MonoBehaviour
{
    [SerializeField] private PlayerMover playerMover;
    [SerializeField] private CircleAttack circleAttack;

    [SerializeField] private DestinationsPool simplePool;
    [SerializeField] private AttackMarkersPool circleAttackPool;
    [SerializeField] private int maxCommandCount = 5;

    [SerializeField] private float commandReductionSpeed = 0.3f;
    private float reductionProgress;
    private const int ObstacleLayerMask = 1 << 9;

    private ICommand _moveTo;
    public event Action OnExecutionStart;
    public event Action OnExecutionEnd;
    public event Action OnCommandsReady;
    public event Action<int> OnCommandAdding;
    public event Action<float> OnCommandProgressReduction;
    private Queue<ICommand> _todoCommands = new Queue<ICommand>();

    private Stack<ICommand> _undoCommands = new Stack<ICommand>();

    private ICommand _currentCommand;
    public Vector3 _lastPos;
    private bool _isExecuting = false;

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
        else if (_todoCommands.Count == 0 && reductionProgress < maxCommandCount)
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
        _todoCommands.Clear();
        _undoCommands.Clear();
        simplePool.Pool.RevertAllToPool();
        circleAttackPool.Pool.RevertAllToPool();
        _isExecuting = false;
        _lastPos = playerMover.GetPos();
        _lastPos = new Vector3(_lastPos.x, 0.2f, _lastPos.z);
        simplePool.SetupPool(maxCommandCount);
        circleAttackPool.SetupPool(maxCommandCount);
        reductionProgress = maxCommandCount;
    }

    private bool CanAddNewCommand()
    {
        return (_todoCommands.Count < maxCommandCount && !(reductionProgress < maxCommandCount));
    }
    
    public void AddMoveToCommand(Vector3 pos)
    {
        if (!CanAddNewCommand())
        {
            return;
        }

        pos = CalculatePossiblePosition(new Ray(_lastPos, pos - _lastPos), pos);
        simplePool.Pool.GetElements().PlaceDestination(_lastPos, pos);
        _lastPos = pos;
        _todoCommands.Enqueue(new MoveToCommand(playerMover, pos));

        OnCommandAdding?.Invoke(maxCommandCount - _todoCommands.Count);
    }
        
    public void AddCircleAttackCommand()
    {
        if (!CanAddNewCommand())
        {
            return;
        }
        circleAttackPool.Pool.GetElements().SetPosition(_lastPos);
        _todoCommands.Enqueue(new CircleAttackCommand(circleAttack));

        OnCommandAdding?.Invoke(maxCommandCount - _todoCommands.Count);
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
        if (Physics.Raycast(ray, out hit, playerMover.MaxDistance, ObstacleLayerMask))
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
        ChangeReductionProgress(-_todoCommands.Count);

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
        _isExecuting = false;
        simplePool.Pool.RevertAllToPool();
        circleAttackPool.Pool.RevertAllToPool();
        OnExecutionEnd?.Invoke();
    }


    private void ExecuteNewCommand(ICommand commandButton)
    {
        commandButton.Execute();
        _undoCommands.Push(commandButton);
    }
}