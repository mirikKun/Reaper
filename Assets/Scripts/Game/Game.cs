using Enemy;
using Factories;
using Players;
using UI;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Game
{
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

        public void SwitchFocusState()
        {
            if (_focusState)
                ExitFocusState();
            else
                EnterFocusState();
        }

        public void SetStartValues()
        {
            _enemyFactory.SetPlayer(_player.transform);

            var spawnRects = GetSpawnAreaRects();
            _levelGenerator.LevelGeneratorSetup(spawnRects);
        
            _mediator.CloseLoseScreen();

            ClearBehaviors();
            SetPlayerDefaultValues();
        }

        private void EnterFocusState()
        {
            _focusState = true;
            _mediator.TurnOnFocusBackground();
            _enemyBehaviorCollection.StopNavMeshMoving();
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

        private void ExitFocusState()
        {
            _focusState = false;
            _mediator.TurnOffFocusBackground();
            _enemyBehaviorCollection.ContinueNavMeshMoving();
        }

        private Rect[] GetSpawnAreaRects()
        {
            Rect[] spawnRects = new Rect[_enemySpawnPoints.Length + 1];
            spawnRects[0] = new Rect(0, 0, 5, 5);
            for (var i = 0; i < _enemySpawnPoints.Length; i++)
            {
                spawnRects[i + 1] = _enemySpawnPoints[i].GetSpawnRect();
            }

            return spawnRects;
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
            _player.Health.OnDeath +=_mediator.OpenLoseScreen;
        
            SetPlayerDefaultValues();
        }

        private void SetPlayerDefaultValues()
        {
            _player.transform.position = Vector3.zero;
            _player.Initialise();
        }
    }
}