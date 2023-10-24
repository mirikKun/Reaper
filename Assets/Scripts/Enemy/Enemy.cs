using System.Collections;
using Factories;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    [RequireComponent(
        typeof(Health),
        typeof(NavMeshAgent),
        typeof(EnemyDeathEffect))]
    public class Enemy : GameBehavior
    {
         [SerializeField] private Transform _model;

        public EnemyFactory OriginFactory { get; set; }
        private Transform _destination;
        private NavMeshAgent _navMeshAgent;
        private Health _health;
        private EnemyDeathEffect _enemyDeathEffect;
        private float _speed;
        private bool _started;
        private bool _dead;

        public void Initialise(Vector3 scale, float speed, Transform destination)
        {
            _speed = speed;
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _health = GetComponent<Health>();
            _enemyDeathEffect = GetComponent<EnemyDeathEffect>();

            _model.localScale = scale;
            _destination = destination;
            _navMeshAgent.speed = speed;

            _health.Initialise();
            _enemyDeathEffect.Initialise(_health);

            _health.OnDeath += Recycle;
            StartCoroutine(EnableAgentWithDelay());
        }


        public override void Recycle()
        {
            _dead = true;
            OriginFactory.Reclaim(this);
        }

        public override bool GameUpdate()
        {
        
            if (_dead)
                return false;
            if(_started)
            {
                _navMeshAgent.SetDestination(_destination.position);
            }
            return true;
        }

        public override void Stop()
        {
            if (_dead)
                return ;
            _navMeshAgent.speed = 0;
        }

        public override void Continue()
        {
            if (_dead)
                return ;
            _navMeshAgent.speed = _speed;
        }

        private IEnumerator EnableAgentWithDelay()
        {
            yield return new WaitForSeconds(0.5f);
            _navMeshAgent.enabled = true;
            _started = true;
        }
    }
}