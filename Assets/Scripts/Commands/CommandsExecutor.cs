using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Command pattern rebind keys example from the book "Game Programming Patterns"
//Is also including undo, redo, and replay system
public class CommandsExecutor : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    //The keys we have that are also connected to commands
    private ICommand _moveTo;
    
    private Queue<ICommand> _todoCommands = new Queue<ICommand>();
    private Stack<ICommand> _undoCommands = new Stack<ICommand>();

    private ICommand _currentCommand;

    private bool _isReplaying = false;



    private const float MovesDelay = 1f;



    public void AddMoveToCommand(Vector3 pos)
    {
        Debug.Log("Adding commang");
        _todoCommands.Enqueue(new MoveToCommand(playerController,pos));
        Debug.Log("Total commanda"+_todoCommands.Count);
        if (_currentCommand == null)
        {
            _currentCommand = _todoCommands.Dequeue();
        }
    }

    private void Update()
    {
        ProcessCommands();
    }

    private void ProcessCommands()
    {
        if (_currentCommand != null && _currentCommand.IsFinished == false)
            return;

        if (_todoCommands.Count<1)
        {
            return;
        }
        _currentCommand = _todoCommands.Dequeue();
        _currentCommand.Execute();
        
    }




    private void ExecuteNewCommand(ICommand commandButton)
    {
        commandButton.Execute();
        
        //Add the new command to the last position in the list
        _undoCommands.Push(commandButton);

    }



}