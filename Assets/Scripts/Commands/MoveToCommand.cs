using UnityEngine;

public class MoveToCommand : ICommand
{
    protected PlayerMover _playerMover;
    protected Vector3 to;
    protected Vector3 from;


    public MoveToCommand(PlayerMover playerMover,Vector3 lastPosition,Vector3 newPosition)
    {
        this._playerMover = playerMover;
        to = newPosition;
        from = lastPosition;
    }


    public  void Execute()
    {
        _playerMover.MoveTo(to);
    }


    //Undo is just the opposite
    public  void Undo()
    {
        _playerMover.MoveTo(from);
    }

    public MoveToCommand GetReversedCommand()
    {
        return new MoveToCommand(_playerMover,to, from);
    }

    public virtual bool IsFinished => _playerMover.State==PlayerState.WaitingCommand;
}