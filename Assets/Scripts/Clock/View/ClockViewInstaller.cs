using UnityEngine;
using Zenject;

namespace NauRa.ClockApp.Clock.View
{
    public sealed class ClockViewInstaller : MonoInstaller
    {
        [SerializeField] private ClockView _view;

        public override void InstallBindings()
        {
            Container.Bind<ClockView>()
                .FromInstance(_view);
            Container.BindInterfacesAndSelfTo<ClockPresenter>()
                .AsSingle();
        }
    }
}
