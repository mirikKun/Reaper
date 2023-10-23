using Commands;
using Players;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Common.Infrastructure.Installers
{
    public class LocationInstaller : MonoInstaller
    {
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform _heroPrefab;
        [SerializeField] private PlayerMarkers _playerMarkers;
        [SerializeField] private UIPlayerStateDisplay _playerStateDisplay;

        public override void InstallBindings()
        {
            RegisterUI();
            BindPlayerMarkers();
            BindHero();
        }

        private void RegisterUI()
        {
            Container
                .Bind<UIPlayerStateDisplay>()
                .FromInstance(_playerStateDisplay)
                .AsSingle();
        }

        private void BindPlayerMarkers()
        {
            Container
                .Bind<PlayerMarkers>()
                .FromInstance(_playerMarkers)
                .AsSingle();
        }

        private void BindHero()
        {
            Player player =
                Container.InstantiatePrefabForComponent<Player>(_heroPrefab, _startPoint.position, Quaternion.identity,
                    null);
            Container
                .Bind<Player>()
                .FromInstance(player)
                .AsSingle();           
            Container
                .Bind<PlayerCommandsExecutor>()
                .FromInstance(player.GetComponent<PlayerCommandsExecutor>())
                .AsSingle();
        
        }
    }
}