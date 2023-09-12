public class CircleAttackCommand: ICommand
{
    private CircleAttack _circleAttack;

    public CircleAttackCommand(CircleAttack circleAttack)
    {
        _circleAttack = circleAttack;
    }
    public void Execute()
    {
        _circleAttack.BeginAttack();
    }

    public void Undo()
    {
        _circleAttack.BeginAttack();
    }

    public bool IsFinished => _circleAttack.AttackEnded;
}