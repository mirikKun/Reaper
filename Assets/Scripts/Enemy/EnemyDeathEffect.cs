using UnityEngine;

namespace Enemy
{
    public class EnemyDeathEffect:MonoBehaviour
    {
        [SerializeField] private ParticleSystem deathParticle;
        [SerializeField] private float destroyAfter=2;
        private IDamageable _damageable;

        public void Initialise(IDamageable damageable)
        {
            _damageable = damageable;
            _damageable.OnDamageTaken += Death;
        }

        public void Death()
        {
            deathParticle.transform.SetParent(transform.parent);
            deathParticle.Play();
            Destroy(deathParticle.gameObject,destroyAfter);
        }
    }
}
