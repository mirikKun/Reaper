using System;
using UnityEngine;

[CreateAssetMenu]
public class EnemyFactory: GameObjectFactory
    {
        [SerializeField] private EnemyConfig _enemyConfig;
        [Serializable]
        private class EnemyConfig
        {
            public Enemy Prefab;
            [FloatRangeSlider(1f, 10f)] public FloatRange Scale = new FloatRange(3f);
        }

        private Transform playerTransform;
        public void SetPlayer(Transform player)
        {
            playerTransform = player;
        }
        public Enemy GetEnemy()
        {
            Enemy inctance = CreateGameObjectInstance(_enemyConfig.Prefab);
            inctance.OriginFactory = this;
            var scale = Vector3.one*_enemyConfig.Scale.RandomValueInRange;
            inctance.Initialise(scale,playerTransform) ;
            return inctance;
        }

        public void Reclaim(Enemy entity)
        {
            Destroy(entity.gameObject);
        }
    }
