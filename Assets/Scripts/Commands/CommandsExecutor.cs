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
    private Vector3 _yPos = new Vector3(0, 1f, 0);

    private ICommand _moveTo;
    public event Action OnExecutionStart;
    public event Action OnExecutionEnd;
    private Queue<ICommand> _todoCommands = new Queue<ICommand>();
    
    private Stack<ICommand> _undoCommands = new Stack<ICommand>();

    private ICommand _currentCommand;
    private Vector3 _lastPos;
    private bool _isExecuting = false;

    private void Start()
    {
      //  aura.localScale=Vector3.one*playerController.MaxDistance/5f;
        _lastPos = playerController.GetPos();
    }

    public void AddMoveToCommand(Vector3 pos)
    {
        pos = CalculatePossiblePosition(pos);
        destinationPool.GetDestination().PlaceDestination(_lastPos,pos);
       // aura.transform.position = pos;
        _lastPos = pos;
        _todoCommands.Enqueue(new MoveToCommand(playerController,pos));
        
    }
    public Vector3 CalculatePossiblePosition(Vector3 newPosition)
    {
        newPosition+=_yPos;
        Vector3 vector = _lastPos-newPosition;
        if (vector.magnitude > playerController.MaxDistance)
        {
            var factor = playerController.MaxDistance / vector.magnitude;
            newPosition = new Vector3(_lastPos.x- vector.x * factor, 0,
                _lastPos.z - vector.z * factor)+_yPos;
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

        if (_todoCommands.Count<1)
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