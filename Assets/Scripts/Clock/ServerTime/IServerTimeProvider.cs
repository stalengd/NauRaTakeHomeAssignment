using System;
using Cysharp.Threading.Tasks;

namespace NauRa.ClockApp.Clock.ServerTime
{
    public interface IServerTimeProvider
    {
        UniTask<DateTime?> GetCurrentTime();
    }
}
