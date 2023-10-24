using System;
using UnityEngine;

namespace Enemy
{
    public class Health : MonoBehaviour, IDamageable
    {
        [SerializeField] private int _currentHealth;
        [SerializeField] private int _maxHealth = 1;

        [field: SerializeField] public bool IsInvincible { get; set; }

        public int CurrentHealth
        {
            get => _currentHealth;
            private set => _currentHealth = value;
        }

        public int MaxHealth
        {
            get => _maxHealth;
            private set => _maxHealth = value;
        }

        public event Action OnDamageTaken;
        public event Action OnDeath;

        public void Initialise()
        {
            CurrentHealth = MaxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (IsInvincible)
            {
                return;
            }

            int damageTaken = Mathf.Clamp(damage, 0, CurrentHealth);
            CurrentHealth -= damageTaken;
            if (damageTaken != 0)
            {
                OnDamageTaken?.Invoke();
            }

            if (CurrentHealth == 0 && damageTaken != 0)
            {
                OnDeath?.Invoke();
            }
        }

        public float GetHealthPercentage()
        {
            return (float)CurrentHealth / MaxHealth;
        }
    }
}