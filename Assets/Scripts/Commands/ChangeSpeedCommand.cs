using Players;

namespace Commands
{
    public class ChangeSpeedCommand : ICommand

    {
        protected PlayerMover _playerMover;



        public ChangeSpeedCommand(PlayerMover playerMover)
        {
            _playerMover = playerMover;
        }


        public void Execute()
        {
            _playerMover.FastMoving=!_playerMover.FastMoving;
        }


        public void Undo()
        {
            _playerMover.FastMoving=!_playerMover.FastMoving;
        }
    

        public virtual bool IsFinished => true;
    }
}