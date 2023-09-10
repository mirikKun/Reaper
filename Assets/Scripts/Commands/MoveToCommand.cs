using UnityEngine;

public class MoveToCommand : ICommand
{
    private PlayerMover _playerMover;
    private Vector3 to;
    private Vector3 from;


    public MoveToCommand(PlayerMover playerMover,Vector3 newPosition)
    {
        this._playerMover = playerMover;
        to = newPosition;
        from = _playerMover.GetPos();
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

    public bool IsFinished => _playerMover.State==PlayerState.WaitingCommand;
}