using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private EnemyFactory enemyFactory;
    
    [SerializeField] private float rate=4;

    [SerializeField] private LevelGenerator levelGenerator;
    [SerializeField] private EnemySpawnPoint[] enemySpawnPoints;
    private float _progress;
    private EnemyBehaviorCollection _enemyBehaviorCollection=new EnemyBehaviorCollection();

    private void Start()
    {
 
        
        Rect[] spawnRects = new Rect[enemySpawnPoints.Length + 1];
        spawnRects[0] = new Rect(0, 0, 5, 5);
        for (var i = 0; i < enemySpawnPoints.Length; i++)
        {
            spawnRects[i+1] = enemySpawnPoints[i].GetSpawnRect();
        }

        levelGenerator.LevelGeneratorSetup(spawnRects);
        enemyFactory.SetPlayer(player);
    }

    private void Update()
    {
        _progress += rate * Time.deltaTime;
        while (_progress>=1)
        {
            var enemy = enemyFactory.GetEnemy();
            enemy.transform.position = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)].GetSpawnPosition();
            _enemyBehaviorCollection.Add(enemy);
            _progress--;
    
        }
    
        _enemyBehaviorCollection.GameUpdate();
    }
}
