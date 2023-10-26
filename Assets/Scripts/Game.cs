using Enemy;
using Factories;
using Players;
using UI;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    [SerializeField] private EnemyFactory _enemyFactory;

    [SerializeField] private float _rate = 4;

    [SerializeField] private LevelGenerator _levelGenerator;
    [SerializeField] private EnemySpawnPoint[] _enemySpawnPoints;
    private Mediator _mediator;
    private Player _player;
    private float _progress;
    private bool _focusState;
    private EnemyBehaviorCollection _enemyBehaviorCollection = new();

    [Inject]
    private void Construct(Player injectedPlayer,Mediator mediator)
    {
        _mediator = mediator;
        PlayerInstall(injectedPlayer);
    }

    private void Start()
    {
        SetStartValues();
    }

    private void OnDisable()
    {
        _player.PlayerCommandsExecutor.OnCommandsReady -= EnterFocusState;
        _player.PlayerCommandsExecutor.OnExecutionStart -= ExitFocusState;
        _player.Health.OnDeath -= _mediator.OpenLoseScreen;
    }

    private void Update()
    {
        if (_focusState)
        {
            return;
        }
        EnemySpawnProgress();
        _enemyBehaviorCollection.GameUpdate();
    }

    private void EnemySpawnProgress()
    {
        _progress += _rate * Time.deltaTime;
        while (_progress >= 1)
        {
            Vector3 newPosition = _enemySpawnPoints[Random.Range(0, _enemySpawnPoints.Length)].GetRandomSpawnPosition();
            var enemy = _enemyFactory.GetEnemy();
            enemy.transform.position = newPosition;
            _enemyBehaviorCollection.Add(enemy);
            _progress--;
        }
    }

    public void SwitchFocusState()
    {
        if (_focusState)
            ExitFocusState();
        else
            EnterFocusState();
    }

    private void EnterFocusState()
    {
        _focusState = true;
        _mediator.TurnOnFocusBackground();
        _enemyBehaviorCollection.StopNavMeshMoving();
    }

    private void ExitFocusState()
    {
        _focusState = false;
        _mediator.TurnOffFocusBackground();
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
        _mediator.CloseLoseScreen();
    }


    private void ClearBehaviors()
    {
        _progress = 0;
        _enemyBehaviorCollection.Clear();
    }

    private void PlayerInstall(Player injectedPlayer)
    {

        _player = injectedPlayer;
        _player.PlayerCommandsExecutor.OnCommandsReady += EnterFocusState;
        _player.PlayerCommandsExecutor.OnExecutionStart += ExitFocusState;
        _player.Health.OnDeath +=_mediator.CloseLoseScreen;
        
        _player.transform.position = Vector3.zero;
        _player.Initialise();
    }
}