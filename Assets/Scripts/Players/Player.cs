using Commands;
using Enemy;
using UnityEngine;
using Zenject;

namespace Players
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerCommandsExecutor _playerCommandsExecutor;
        [SerializeField] private Health _health;

        public PlayerCommandsExecutor PlayerCommandsExecutor => _playerCommandsExecutor;
        public Health Health => _health;

         private UIPlayerStateDisplay _uiPlayerStateDisplay;

         [Inject]
         private void Construct(UIPlayerStateDisplay uiPlayerStateDisplay)
         {
             _uiPlayerStateDisplay = uiPlayerStateDisplay;
      
         }
     
        private void OnEnable()
        {
        
            _playerCommandsExecutor.OnCommandAdding += _uiPlayerStateDisplay.FillActionPoint;
            _playerCommandsExecutor.OnCommandProgressReduction += _uiPlayerStateDisplay.FillActionPoints;
            _health.OnDamageTaken += TakeDamage;
            _playerCommandsExecutor.OnExecutionEnd += EnableDamageable;
            _playerCommandsExecutor.OnExecutionStart += DisableDamageable;
        }

        private void OnDisable()
        {
            _playerCommandsExecutor.OnCommandAdding -= _uiPlayerStateDisplay.FillActionPoint;
            _playerCommandsExecutor.OnCommandProgressReduction -= _uiPlayerStateDisplay.FillActionPoints;

            _health.OnDamageTaken -= TakeDamage;
            _playerCommandsExecutor.OnExecutionEnd -= EnableDamageable;
            _playerCommandsExecutor.OnExecutionStart -= DisableDamageable;
        }

        private void TakeDamage()
        {
            _uiPlayerStateDisplay.SetHealthValue(_health.GetHealthPercentage());
        }

        private void EnableDamageable()
        {
            _health.IsInvincible = false;
        }

        private void DisableDamageable()
        {
            _health.IsInvincible = true;
        }

        public void Initialise()
        {
            _health.Initialise();
            _playerCommandsExecutor.SetStartValues();
            _uiPlayerStateDisplay.SetHealthValue(_health.GetHealthPercentage());
            _uiPlayerStateDisplay.ChangeActionPointCount(5);
            EnableDamageable();
        }
    }
}