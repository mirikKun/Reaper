using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Pillar : MonoBehaviour
{
    [SerializeField] private Transform model;
    public ObstaclesFactory OriginFactory { get; set; }

    public void Initialise(Vector3 scale, Quaternion rotation)
    {
        model.localScale = scale;
        model.rotation = rotation;
    }
    
    public void Recycle()
    {
        OriginFactory.Reclaim(this);
    }

    public virtual void GameUpdate()
    {
        
    }
}
