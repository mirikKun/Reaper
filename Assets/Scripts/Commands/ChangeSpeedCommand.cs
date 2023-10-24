using Players;

namespace Commands
{
    public class ChangeSpeedCommand : ICommand

    {
        protected PlayerMover playerMover;



        public ChangeSpeedCommand(PlayerMover playerMover)
        {
            this.playerMover = playerMover;
        }


        public void Execute()
        {
            playerMover.FastMoving=!playerMover.FastMoving;
        }


        public void Undo()
        {
            playerMover.FastMoving=!playerMover.FastMoving;
        }
    

        public virtual bool IsFinished => true;
    }
}