using System;
using UnityEngine;

[CreateAssetMenu]
public class EnemyFactory: GameObjectFactory
    {
        [SerializeField] private EnemyConfig _pillarConfig;
        [Serializable]
        private class EnemyConfig
        {
            public Pillar Prefab;
            [FloatRangeSlider(1f, 10f)] public FloatRange Scale = new FloatRange(3f);

        }
    }
