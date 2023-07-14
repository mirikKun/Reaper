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

    //  [SerializeField] private Transform aura;
    //The keys we have that are also connected to commands

    private ICommand _moveTo;
    public event Action OnExecutionStart;
    public event Action OnExecutionEnd;
    private Queue<ICommand> _todoCommands = new Queue<ICommand>();

    private Stack<ICommand> _undoCommands = new Stack<ICommand>();

    private ICommand _currentCommand;
    public Vector3 LastPos { get; private set; }
    private bool _isExecuting = false;

    private void Start()
    {
        //  aura.localScale=Vector3.one*playerController.MaxDistance/5f;
        LastPos = playerController.GetPos();
        LastPos = new Vector3(LastPos.x, 0.2f, LastPos.z);
    }

    public void AddMoveToCommand(Vector3 pos)
    {
        pos = CalculatePossiblePosition(pos);
        destinationPool.GetDestination().PlaceDestination(LastPos, pos);
        // aura.transform.position = pos;
        LastPos = pos;
        _todoCommands.Enqueue(new MoveToCommand(playerController, pos));
    }

    public Vector3 CalculatePossiblePosition(Vector3 newPosition)
    {
        Vector3 vector = LastPos - newPosition;
        if (vector.magnitude > playerController.MaxDistance)
        {
            var factor = playerController.MaxDistance / vector.magnitude;
            newPosition = new Vector3(LastPos.x - vector.x * factor, newPosition.y,
                LastPos.z - vector.z * factor) ;
        }

        return newPosition;
    }

    public void StartExecuting()
    {
        _isExecuting = true;
        //  aura.gameObject.SetActive(false);
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
        // aura.gameObject.SetActive(true);
        OnExecutionEnd?.Invoke();
    }


    private void ExecuteNewCommand(ICommand commandButton)
    {
        commandButton.Execute();

        //Add the new command to the last position in the list
        _undoCommands.Push(commandButton);
    }
}