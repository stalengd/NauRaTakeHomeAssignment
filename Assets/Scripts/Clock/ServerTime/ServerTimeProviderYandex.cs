using System;
using Cysharp.Threading.Tasks;
using NauRa.ClockApp.Rest;

namespace NauRa.ClockApp.Clock.ServerTime
{
    public sealed class ServerTimeProviderYandex : IServerTimeProvider
    {
        private readonly RestClient _restClient;
        private readonly string _endpointUrl = "https://yandex.com/time/sync.json";

        public ServerTimeProviderYandex(RestClient restClient)
        {
            _restClient = restClient;
        }

        public async UniTask<DateTime?> GetCurrentTime()
        {
            var result = await _restClient.Get<YandexTimeResponse>(_endpointUrl);
            if (result.TryGet(out var timeResponse))
            {
                return new DateTime(TimeSpan.FromMilliseconds(timeResponse.time).Ticks, DateTimeKind.Utc);
            }
            return null;
        }
    }
}
