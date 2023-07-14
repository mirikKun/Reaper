using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Base class for the commands
//This class should always look like this to make it more general, so no constructors, parameters, etc!!!
public interface ICommand
{
    public void Execute();

    public void Undo();
    public bool IsFinished{get;}
}