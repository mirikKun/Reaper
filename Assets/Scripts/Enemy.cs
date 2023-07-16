using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : GameBehavior
{
    [SerializeField] private Transform model;
    
    public EnemyFactory OriginFactory { get; set; }
    private Transform _destination;
    private NavMeshAgent _navMeshAgent;

    public void Initialise(Vector3 scale,Transform destination)
    {
        model.localScale = scale;
        _navMeshAgent=GetComponent<NavMeshAgent>();
        _destination = destination;
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
