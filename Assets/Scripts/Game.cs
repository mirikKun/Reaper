using Enemy;
using Factories;
using Players;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    [SerializeField] private EnemyFactory _enemyFactory;

    [SerializeField] private float _rate = 4;

    [SerializeField] private LevelGenerator _levelGenerator;
    [SerializeField] private EnemySpawnPoint[] _enemySpawnPoints;
    [SerializeField] private Player _player;
    [SerializeField] private UIGameStateSwitch _uiGameStateSwitch;
    private float _progress;
    private bool _focusState;
    private EnemyBehaviorCollection _enemyBehaviorCollection = new();

    [Inject]
    private void Construct(Player injectedPlayer)
    {
        _player = injectedPlayer;
        _player.PlayerCommandsExecutor.OnCommandsReady += EnterFocusState;
        _player.PlayerCommandsExecutor.OnExecutionStart += ExitFocusState;
        _player.Health.OnDeath += _uiGameStateSwitch.TurnOnLoseScreen;
        _player.transform.position = Vector3.zero;
        _player.Initialise();
    }

    private void Start()
    {
        SetStartValues();
    }

    private void OnDisable()
    {
        _player.PlayerCommandsExecutor.OnCommandsReady -= EnterFocusState;
        _player.PlayerCommandsExecutor.OnExecutionStart -= ExitFocusState;
        _player.Health.OnDeath -= _uiGameStateSwitch.TurnOnLoseScreen;
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
        _uiGameStateSwitch.TurnOnFocusBackground();
        _enemyBehaviorCollection.StopNavMeshMoving();
    }

    private void ExitFocusState()
    {
        _focusState = false;
        _uiGameStateSwitch.TurnOffFocusBackground();
        _enemyBehaviorCollection.ContinueNavMeshMoving();
    }

    public void SetStartValues()
    {
        _enemyFactory.SetPlayer(_player.transform);

        Rect[] spawnRects = new Rect[_enemySpawnPoints.Length + 1];
        spawnRects[0] = new Rect(0, 0, 5, 5);
        for (var i = 0; i < _enemySpawnPoints.Length; i++)
        {
            spawnRects[i + 1] = _enemySpawnPoints[i].GetSpawnRect();
        }

        _levelGenerator.LevelGeneratorSetup(spawnRects);

        ClearBehaviors();
        _uiGameStateSwitch.TurnOffLoseScreen();
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

        _progress += _rate * Time.deltaTime;
        while (_progress >= 1)
        {
            Vector3 newPosition = _enemySpawnPoints[Random.Range(0, _enemySpawnPoints.Length)].GetRandomSpawnPosition();
            var enemy = _enemyFactory.GetEnemy();
            enemy.transform.position = newPosition;
            _enemyBehaviorCollection.Add(enemy);
            _progress--;
        }

        _enemyBehaviorCollection.GameUpdate();
    }
}