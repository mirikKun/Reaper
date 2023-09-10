using UnityEngine;
using UnityEngine.AI;

[RequireComponent(
     typeof(Health),
    typeof(NavMeshAgent),
    typeof(EnemyDeathEffect))]
public class Enemy : GameBehavior
{
    [SerializeField] private Transform model;

    public EnemyFactory OriginFactory { get; set; }
    private Transform _destination;
    private NavMeshAgent _navMeshAgent;
    private Health _health;
    private EnemyDeathEffect _enemyDeathEffect;
    private float _speed;
    private bool dead;

    public void Initialise(Vector3 scale, float speed, Transform destination)
    {
        _speed = speed;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _health = GetComponent<Health>();
        _enemyDeathEffect = GetComponent<EnemyDeathEffect>();
        
        model.localScale = scale;
        _destination = destination;
        _navMeshAgent.speed = speed;
        
        _health.Initialise();
        _enemyDeathEffect.Initialise(_health);
        
        _health.OnDeath += Recycle;
    }


    public override void Recycle()
    {
        dead = true;
        OriginFactory.Reclaim(this);
    }

    public override bool GameUpdate()
    {
        if (dead)
            return false;
        _navMeshAgent.SetDestination(_destination.position);
        
        return true;
    }

    public override void Stop()
    {
        _navMeshAgent.speed = 0;
        _navMeshAgent.isStopped = true;
    }
    
    public override void Continue()
    {
        _navMeshAgent.speed = _speed;
        _navMeshAgent.isStopped = false;    }
}