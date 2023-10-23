using Players;
using UnityEngine;
using Zenject;

namespace Common.Infrastructure.Installers
{
    public class BootstrapInstaller:MonoInstaller
    {
        [SerializeField]private GameInput _gameInputPrefab;
        public override void InstallBindings()
        {
            BindGameInputService();
        }

        private void BindGameInputService()
        {
            Container
                .Bind<IGameInput>()
                .To<GameInput>()
                .FromComponentInNewPrefab(_gameInputPrefab)
                .AsSingle();
        }
    }
}