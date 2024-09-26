using Zenject;

namespace NauRa.ClockApp.Rest
{
    public sealed class RestInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<RestClient>()
                .ToSelf()
                .AsSingle();
        }
    }
}
