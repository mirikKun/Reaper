using UnityEngine;

public class EnemyDeathEffect:MonoBehaviour
{
    [SerializeField] private ParticleSystem deathParticle;
    private IDamageable _damageable;

    public void Initialise(IDamageable damageable)
    {
        _damageable = damageable;
        _damageable.OnDamageTaken += Death;
    }

    public void Death()
    {
        deathParticle.Play();
    }
}
