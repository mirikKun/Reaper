using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Command pattern rebind keys example from the book "Game Programming Patterns"
//Is also including undo, redo, and replay system
public class CommandsExecutor : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    [SerializeField] private DestinationObjectPoolSimple destinationPool;

    private const int OBSTACLE_LAYER_MASK = 1 << 9;

    private ICommand _moveTo;
    public event Action OnExecutionStart;
    public event Action OnExecutionEnd;
    private Queue<ICommand> _todoCommands = new Queue<ICommand>();

    private Stack<ICommand> _undoCommands = new Stack<ICommand>();

    private ICommand _currentCommand;
    public Vector3 _lastPos;
    private bool _isExecuting = false;

    private void Start()
    {
        _lastPos = playerController.GetPos();
        _lastPos = new Vector3(_lastPos.x, 0.2f, _lastPos.z);
    }

   
    public void AddMoveToCommand(Vector3 pos)
    {
        pos = CalculatePossiblePosition(new Ray(_lastPos, pos - _lastPos), pos);
        destinationPool.GetDestination().PlaceDestination(_lastPos, pos);
        _lastPos = pos;
        _todoCommands.Enqueue(new MoveToCommand(playerController, pos));
    }
    public Vector3 CalculatePossiblePosition(Ray ray,Vector3 newPos)
    {
        RaycastHit hit;
        Vector3 pos=newPos;
        if (Physics.Raycast(ray, out hit,playerController.MaxDistance,OBSTACLE_LAYER_MASK))
        {
            pos = hit.point;
        }
        else
        {
            Vector3 vector = _lastPos - newPos;
            var factor = playerController.MaxDistance / vector.magnitude;
            newPos = new Vector3(_lastPos.x - vector.x * factor, newPos.y,
                _lastPos.z - vector.z * factor) ;
        }

        return pos;
    }
    public void StartExecuting()
    {
        _isExecuting = true;
        OnExecutionStart?.Invoke();
    }

    private void Update()
    {
        if (_isExecuting)
        {
            ProcessCommands();
        }
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
        destinationPool.RevertAllToPool();
        OnExecutionEnd?.Invoke();
    }


    private void ExecuteNewCommand(ICommand commandButton)
    {
        commandButton.Execute();
        _undoCommands.Push(commandButton);
    }
}