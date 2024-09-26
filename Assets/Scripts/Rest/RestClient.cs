using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace NauRa.ClockApp.Rest
{
    public sealed class RestClient
    {
        public async UniTask<RestResult<T>> Get<T>(string endpointUrl)
        {
            var request = UnityWebRequest.Get(endpointUrl);
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var model = JsonUtility.FromJson<T>(request.downloadHandler.text);
                return RestResult<T>.FromSuccess(model);
            }
            return RestResult<T>.FromError();
        }
    }
}
