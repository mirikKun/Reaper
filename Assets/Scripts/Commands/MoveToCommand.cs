using Players;
using UnityEngine;

namespace Commands
{
    public class MoveToCommand : ICommand
    {
        protected readonly PlayerMover playerMover;
        protected Vector3 to;
        protected Vector3 from;


        public MoveToCommand(PlayerMover playerMover,Vector3 lastPosition,Vector3 newPosition)
        {
            this.playerMover = playerMover;
            to = newPosition;
            from = lastPosition;
        }


        public  void Execute()
        {
            playerMover.MoveTo(to);
        }


        //Undo is just the opposite
        public  void Undo()
        {
            playerMover.MoveTo(from);
        }

        public MoveToCommand GetReversedCommand()
        {
            return new MoveToCommand(playerMover,to, from);
        }

        public virtual bool IsFinished => playerMover.State==PlayerState.WaitingCommand;
    }
}