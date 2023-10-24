namespace Commands
{
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
}