using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CommandsExecutor commandsExecutor;
    public Health health;
    [SerializeField] private UIPlayerStateDisplay uiPlayerStateDisplay;

    private void OnEnable()
    {
        health.OnDamageTaken += TakeDamage;
    }
    private void OnDisable()
    {
        health.OnDamageTaken -= TakeDamage;
    }

    private void TakeDamage()
    {
        uiPlayerStateDisplay.SetHealthValue(health.GetHealthPercentage());
    }
    public void Initialise()
    {
        health.Initialise();
        commandsExecutor.SetStartValues();
        uiPlayerStateDisplay.SetHealthValue(health.GetHealthPercentage());
    }
    
}
