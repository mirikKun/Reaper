using Factories;
using UnityEngine;
using UnityEngine.Serialization;

[SelectionBase]
public class Pillar : MonoBehaviour
{
    [SerializeField] private Transform _model;
    public ObstaclesFactory OriginFactory { get; set; }

    public void Initialise(Vector3 scale, Quaternion rotation)
    {
        _model.localScale = scale;
        _model.rotation = rotation;
    }
    
    public void Recycle()
    {
        OriginFactory.Reclaim(this);
    }


}
