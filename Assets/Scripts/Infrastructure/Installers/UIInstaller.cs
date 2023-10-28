using UI;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class UIInstaller : MonoInstaller
    {
        [SerializeField] private Mediator _mediator;

        public override void InstallBindings()
        {
            Container
                .Bind<Mediator>()
                .FromInstance(_mediator)
                .AsSingle();
        }
    }
}