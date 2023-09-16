using UnityEngine;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    [SerializeField] private EnemyFactory enemyFactory;

    [SerializeField] private float rate = 4;

    [SerializeField] private LevelGenerator levelGenerator;
    [SerializeField] private EnemySpawnPoint[] enemySpawnPoints;
    [SerializeField] private Player player;
    [SerializeField] private UIGameStateSwitch uiGameStateSwitch;
    private float _progress;
    private bool _focusState;
    private EnemyBehaviorCollection _enemyBehaviorCollection = new();

    private void Start()
    {
        SetStartValues();
        player.PlayerCommandsExecutor.OnCommandsReady += EnterFocusState;
        player.PlayerCommandsExecutor.OnExecutionStart += ExitFocusState;
        player.Health.OnDeath+=uiGameStateSwitch.TurnOnLoseScreen;
    }

    private void OnDisable()
    {
        player.PlayerCommandsExecutor.OnCommandsReady -= EnterFocusState;
        player.PlayerCommandsExecutor.OnExecutionStart -= ExitFocusState;
        player.Health.OnDeath-=uiGameStateSwitch.TurnOnLoseScreen;

    }

    public void SwitchFocusState()
    {
        if (_focusState)
        {
            ExitFocusState();
        }
        else
        {
            EnterFocusState();
        }
    }
    private void EnterFocusState()
    {
        _focusState = true;
        uiGameStateSwitch.TurnOnFocusBackground();
        _enemyBehaviorCollection.StopNavMeshMoving();
    }

    private void ExitFocusState()
    {
        _focusState = false;
        uiGameStateSwitch.TurnOffFocusBackground();
        _enemyBehaviorCollection.ContinueNavMeshMoving();

    }

    public void SetStartValues()
    {
        player.transform.position = Vector3.zero;
        player.Initialise();

        enemyFactory.SetPlayer(player.transform);

        Rect[] spawnRects = new Rect[enemySpawnPoints.Length + 1];
        spawnRects[0] = new Rect(0, 0, 5, 5);
        for (var i = 0; i < enemySpawnPoints.Length; i++)
        {
            spawnRects[i + 1] = enemySpawnPoints[i].GetSpawnRect();
        }

        levelGenerator.LevelGeneratorSetup(spawnRects);

        ClearBehaviors();
        uiGameStateSwitch.TurnOffLoseScreen();
    }


    private void ClearBehaviors()
    {
        _progress = 0;
        _enemyBehaviorCollection.Clear();
    }

    private void Update()
    {
        if (_focusState)
        {
            return;
        }

        _progress += rate * Time.deltaTime;
        while (_progress >= 1)
        {
            var enemy = enemyFactory.GetEnemy();
            enemy.transform.position =
                enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)].GetRandomSpawnPosition();
            _enemyBehaviorCollection.Add(enemy);
            _progress--;
        }

        _enemyBehaviorCollection.GameUpdate();
    }
}