using System;
using UnityEngine;

namespace Factories
{
    [CreateAssetMenu]
    public class EnemyFactory: GameObjectFactory
    {
        [SerializeField] private EnemyConfig _enemyConfig;
        [Serializable]
        private class EnemyConfig
        {
            public Enemy.Enemy Prefab;
            [FloatRangeSlider(0.3f, 10f)] public FloatRange Scale = new FloatRange(1f);
            [FloatRangeSlider(0.3f, 10f)] public FloatRange Speed = new FloatRange(3f);
        }

        private Transform playerTransform;
        public void SetPlayer(Transform player)
        {
            playerTransform = player;
        }
        public Enemy.Enemy GetEnemy()
        {
            Enemy.Enemy inctance = CreateGameObjectInstance(_enemyConfig.Prefab);
            inctance.OriginFactory = this;
            var scale = Vector3.one*_enemyConfig.Scale.RandomValueInRange;
            inctance.Initialise(scale,_enemyConfig.Speed.RandomValueInRange,playerTransform) ;
            return inctance;
        }

        public void Reclaim(Enemy.Enemy entity)
        {
            Destroy(entity.gameObject);
        }
    }
}
