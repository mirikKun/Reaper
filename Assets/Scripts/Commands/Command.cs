public interface ICommand
{
    public void Execute();

    public void Undo();
    public bool IsFinished{get;}
}