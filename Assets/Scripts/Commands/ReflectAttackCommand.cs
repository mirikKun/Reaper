using Attack;

namespace Commands
{
    public class ReflectAttackCommand: ICommand
    {
        private ReflectAttack _reflectAttack;

        public ReflectAttackCommand(ReflectAttack reflectAttack)
        {
            _reflectAttack = reflectAttack;
        }
        public void Execute()
        {
            _reflectAttack.BeginAttack();
        }

        public void Undo()
        {
            _reflectAttack.BeginAttack();
        }

        public bool IsFinished => _reflectAttack.AttackEnded;
    }
}
