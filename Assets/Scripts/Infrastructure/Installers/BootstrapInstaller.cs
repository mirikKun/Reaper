using Players;
using Zenject;

namespace Infrastructure.Installers
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