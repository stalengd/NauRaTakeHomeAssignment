using NauRa.ClockApp.Clock.ServerTime;
using UnityEngine;
using Zenject;

namespace NauRa.ClockApp.Clock.Model
{
    public sealed class ClockModelInstaller : MonoInstaller
    {
        [SerializeField] private ClockConfig _clockConfig;

        public override void InstallBindings()
        {
            Container.Bind<ClockModel>()
                .ToSelf()
                .AsSingle();
            Container.BindInterfacesAndSelfTo<ClockService>()
                .AsSingle();
            Container.Bind<ClockConfig>()
                .FromInstance(_clockConfig);

            Container.Bind<IServerTimeProvider>()
                .To<ServerTimeProviderTimeApiIo>()
                .AsCached();
            Container.Bind<IServerTimeProvider>()
                .To<ServerTimeProviderYandex>()
                .AsCached();
        }
    }
}