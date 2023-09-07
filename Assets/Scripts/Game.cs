using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    [SerializeField] private EnemyFactory enemyFactory;
    
    [SerializeField] private float rate=4;

    [SerializeField] private LevelGenerator levelGenerator;
    [SerializeField] private EnemySpawnPoint[] enemySpawnPoints;
    [SerializeField] private CommandsExecutor player;
    private float _progress;
    private EnemyBehaviorCollection _enemyBehaviorCollection=new ();

    private void Start()
    {
        SetStartValues();
    }

    public void SetStartValues()
    {
        player.transform.position=Vector3.zero;
        player.SetStartValues();
        
        enemyFactory.SetPlayer(player.transform);
        
        Rect[] spawnRects = new Rect[enemySpawnPoints.Length + 1];
        spawnRects[0] = new Rect(0, 0, 5, 5);
        for (var i = 0; i < enemySpawnPoints.Length; i++)
        {
            spawnRects[i+1] = enemySpawnPoints[i].GetSpawnRect();
        }        
        levelGenerator.LevelGeneratorSetup(spawnRects);
        
        ClearBehaviors();
    }

    private void ClearBehaviors()
    {
        _progress = 0;
        _enemyBehaviorCollection.Clear();
    }

    private void Update()
    {
        _progress += rate * Time.deltaTime;
        while (_progress>=1)
        {
            var enemy = enemyFactory.GetEnemy();
            enemy.transform.position = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)].GetRandomSpawnPosition();
            _enemyBehaviorCollection.Add(enemy);
            _progress--;
    
        }
    
        _enemyBehaviorCollection.GameUpdate();
    }
}
