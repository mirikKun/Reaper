using System;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu]
public class ObstaclesFactory : GameObjectFactory
{
    [SerializeField] private ObstacleConfig _pillarConfig;
    [Serializable]
    private class ObstacleConfig
    {
        public Pillar Prefab;
        [FloatRangeSlider(1f, 10f)] public FloatRange Scale = new FloatRange(3f);
        [FloatRangeSlider(-30f, 30f)] public FloatRange Angle = new FloatRange(0f);

    }

    public float MaxScale => _pillarConfig.Scale.Max;
    public Pillar GetPillar()
    {
        Pillar inctance = CreateGameObjectInstance(_pillarConfig.Prefab);
        inctance.OriginFactory = this;
        var scale = _pillarConfig.Scale.RandomValueInRange;
        inctance.Initialise(new Vector3(1,3,1)*scale,
            Quaternion.Euler(
                _pillarConfig.Angle.RandomValueInRange,
                Random.Range(0,360),
                _pillarConfig.Angle.RandomValueInRange)) ;
        return inctance;
    }

    public void Reclaim(Pillar entity)
    {
        Destroy(entity.gameObject);
    }

  
}
