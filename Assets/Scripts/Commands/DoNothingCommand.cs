using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//The point of this command is to do nothing
//Is used instead of setting a command to null, so it's called Null Object, which is another programming pattern 
public class DoNothingCommand : ICommand
{

    public  void Execute()
    {
    }

    public  void Undo()
    {
    }

    public bool IsFinished => true;

}