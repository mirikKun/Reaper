
using System;

public interface IDamageable
{
   public int CurrentHealth { get; }
   public int MaxHealth { get;  }

   public event Action OnDamageTaken;
   public event Action OnDeath;
   public void TakeDamage(int damage);
}
