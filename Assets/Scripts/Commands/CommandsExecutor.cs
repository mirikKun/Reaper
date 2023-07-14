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
    private Vector3 _lastPos;
    private bool _isExecuting = false;

    private void Start()
    {
      //  aura.localScale=Vector3.one*playerController.MaxDistance/5f;
        _lastPos = playerController.GetPos();
    }

    public void AddMoveToCommand(Vector3 pos)
    {
        pos = playerController.CalculatePossiblePosition(pos);
        destinationPool.GetDestination().PlaceDestination(_lastPos,pos);
       // aura.transform.position = pos;
        _lastPos = pos;
        _todoCommands.Enqueue(new MoveToCommand(playerController,pos));
        
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