using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToCommand : ICommand
{
    private PlayerController _playerController;
    private Vector3 to;
    private Vector3 from;


    public MoveToCommand(PlayerController playerController,Vector3 newPosition)
    {
        this._playerController = playerController;
        to = newPosition;
        from = _playerController.GetPos();
    }


    public  void Execute()
    {
        _playerController.MoveTo(to);
    }


    //Undo is just the opposite
    public  void Undo()
    {
        _playerController.MoveTo(from);
    }

    public bool IsFinished => _playerController.State==PlayerState.WaitingCommand;
}