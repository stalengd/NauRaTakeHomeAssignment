using System;
using Cysharp.Threading.Tasks;
using NauRa.ClockApp.Rest;

namespace NauRa.ClockApp.Clock.ServerTime
{
    public sealed class ServerTimeProviderTimeApiIo : IServerTimeProvider
    {
        private readonly RestClient _restClient;
        private readonly string _endpointUrl = "https://timeapi.io/api/time/current/zone?timeZone=Etc%2FUTC";

        public ServerTimeProviderTimeApiIo(RestClient restClient)
        {
            _restClient = restClient;
        }

        public async UniTask<DateTime?> GetCurrentTime()
        {
            var result = await _restClient.Get<TimeApiIoTimeResponse>(_endpointUrl);
            if (result.TryGet(out var timeResponse))
            {
                return DateTime.Parse(timeResponse.dateTime);
            }
            return null;
        }
    }
}
