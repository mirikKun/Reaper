using UnityEngine;
using UnityEngine.AI;

[RequireComponent(
     typeof(EnemyHealth),
    typeof(NavMeshAgent),
    typeof(EnemyDeathEffect))]
public class Enemy : GameBehavior
{
    [SerializeField] private Transform model;

    public EnemyFactory OriginFactory { get; set; }
    private Transform _destination;
    private NavMeshAgent _navMeshAgent;
    private EnemyHealth _enemyHealth;
    private EnemyDeathEffect _enemyDeathEffect;

    public void Initialise(Vector3 scale, float speed, Transform destination)
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _enemyHealth = GetComponent<EnemyHealth>();
        _enemyDeathEffect = GetComponent<EnemyDeathEffect>();
        
        model.localScale = scale;
        _destination = destination;
        _navMeshAgent.speed = speed;
        
        _enemyHealth.Initialise();
        _enemyDeathEffect.Initialise(_enemyHealth);
        
        _enemyHealth.OnDeath += Recycle;
    }

    public override void Recycle()
    {
        OriginFactory.Reclaim(this);
    }

    public override bool GameUpdate()
    {
        _navMeshAgent.SetDestination(_destination.position);
        return true;
    }

}