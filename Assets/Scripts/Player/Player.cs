
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField] private PlayerCommandsExecutor playerCommandsExecutor;
    [SerializeField] private Health health;

    public PlayerCommandsExecutor PlayerCommandsExecutor => playerCommandsExecutor;
    public Health Health => health;

    [SerializeField] private UIPlayerStateDisplay uiPlayerStateDisplay;

    private void OnEnable()
    {        
        playerCommandsExecutor.OnCommandAdding += uiPlayerStateDisplay.FillActionPoint;
        playerCommandsExecutor.OnCommandProgressReduction += uiPlayerStateDisplay.FillActionPoints;

        health.OnDamageTaken += TakeDamage;
        playerCommandsExecutor.OnExecutionEnd += EnableDamageable;
        playerCommandsExecutor.OnExecutionStart += DisableDamageable;
    }
    private void OnDisable()
    {
        playerCommandsExecutor.OnCommandAdding -= uiPlayerStateDisplay.FillActionPoint;
        playerCommandsExecutor.OnCommandProgressReduction -= uiPlayerStateDisplay.FillActionPoints;

        health.OnDamageTaken -= TakeDamage;
        playerCommandsExecutor.OnExecutionEnd -= EnableDamageable;
        playerCommandsExecutor.OnExecutionStart -= DisableDamageable;
    }

    private void TakeDamage()
    {
        uiPlayerStateDisplay.SetHealthValue(health.GetHealthPercentage());
    }

    private void EnableDamageable()
    {
        health.IsInvincible=false;
    }

    private void DisableDamageable()
    {
        health.IsInvincible = true;

    }
    public void Initialise()
    {
        health.Initialise();
        playerCommandsExecutor.SetStartValues();
        uiPlayerStateDisplay.SetHealthValue(health.GetHealthPercentage());
        uiPlayerStateDisplay.ChangeActionPointCount(5);
        EnableDamageable();
    }
    
}
