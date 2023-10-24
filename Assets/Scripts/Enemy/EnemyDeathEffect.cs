using UnityEngine;

namespace Enemy
{
    public class EnemyDeathEffect:MonoBehaviour
    {
         [SerializeField] private ParticleSystem _deathParticle;
         [SerializeField] private float _destroyAfter=2;
        private IDamageable _damageable;

        public void Initialise(IDamageable damageable)
        {
            _damageable = damageable;
            _damageable.OnDamageTaken += Death;
        }

        public void Death()
        {
            _deathParticle.transform.SetParent(transform.parent);
            _deathParticle.Play();
            Destroy(_deathParticle.gameObject,_destroyAfter);
        }
    }
}
