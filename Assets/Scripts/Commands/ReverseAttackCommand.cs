using System.Collections.Generic;
using UnityEngine;

public class ReverseAttackCommand : ICommand
{
    private Queue<Vector3> _moveToCommands;

    private ICommand _lastCommand;
    public ReverseAttackCommand(Stack<ICommand> doneCommands)
    {
        _moveToCommands = new Queue<Vector3>();
        ICommand command = doneCommands.Pop();
        if(command.GetType()==typeof(MoveToCommand))
        {
           // _moveToCommands.Enqueue(((MoveToCommand)command).);
        }
        
    }
    public void Execute()
    {
        throw new System.NotImplementedException();
    }

    public void Undo()
    {
        throw new System.NotImplementedException();
    }

    public bool IsFinished=> _lastCommand==null||_lastCommand.IsFinished;
}