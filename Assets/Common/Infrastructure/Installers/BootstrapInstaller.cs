using Players;
using UnityEngine;
using Zenject;

namespace Common.Infrastructure.Installers
{
    public class BootstrapInstaller:MonoInstaller
    {
        public override void InstallBindings()
        {
            BindGameInputService();
        }

        private void BindGameInputService()
        {
            Container.BindInterfacesTo<GameInput>().AsSingle();
        }
    }
}