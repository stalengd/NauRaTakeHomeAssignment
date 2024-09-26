using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NauRa.ClockApp.Clock.ServerTime;
using UnityEngine;
using Zenject;

namespace NauRa.ClockApp.Clock.Model
{
    public sealed class ClockService : ITickable
    {
        private readonly ClockModel _clockModel;
        private readonly ClockConfig _clockConfig;
        private readonly List<IServerTimeProvider> _serverTimeProviders;

        private const double TickIntervalSeconds = 0.5;
        private double _prevTickTime;
        private bool _isLoadingTime = false;
        private double _lastLoadTimestamp = double.MinValue;

        public ClockService(ClockModel clockModel, ClockConfig clockConfig, List<IServerTimeProvider> serverTimeProviders)
        {
            _clockModel = clockModel;
            _clockConfig = clockConfig;
            _serverTimeProviders = serverTimeProviders;
        }

        public UniTask Initialize()
        {
            return LoadCurrentTime(fallbackOnFailure: true);
        }

        public void Tick()
        {
            var localTime = Time.unscaledTimeAsDouble;
            if (localTime - _prevTickTime >= TickIntervalSeconds)
            {
                _clockModel.Tick(TimeSpan.FromSeconds(localTime - _prevTickTime));
                _prevTickTime = localTime;
            }
            if (IsTimeShouldBeReloaded())
            {
                LoadCurrentTime().Forget();
            }
        }

        private async UniTask LoadCurrentTime(bool fallbackOnFailure = false)
        {
            if (_isLoadingTime)
            {
                return;
            }
            _isLoadingTime = true;
            var results = await UniTask.WhenAll(_serverTimeProviders.Select(x => x.GetCurrentTime()));
            _lastLoadTimestamp = Time.unscaledTimeAsDouble;
            var successCount = 0;
            foreach (var result in results)
            {
                if (result.HasValue)
                {
                    successCount++;
                }
            }
            var averageTime = DateTime.MinValue;
            foreach (var result in results)
            {
                if (result is { } time)
                {
                    averageTime += TimeSpan.FromTicks(time.Ticks / successCount);
                }
            }
            if (successCount > 0)
            {
                Debug.Log("Current time was successfully loaded from the server.");
                _clockModel.SetAbsoluteTime(averageTime);
                _isLoadingTime = false;
                return;
            }
            if (fallbackOnFailure)
            {
                Debug.Log("Current time can not be loaded from the server, falling back to local.");
                _clockModel.SetAbsoluteTime(DateTime.UtcNow);
            }
            else
            {
                Debug.Log("Current time can not be loaded from the server, current time unchanged.");
            }
            _isLoadingTime = false;
        }

        private bool IsTimeShouldBeReloaded()
        {
            var localTime = Time.unscaledTimeAsDouble;
            return !_isLoadingTime &&
                (localTime - _lastLoadTimestamp) >= _clockConfig.CurrentTimeLoadInterval.TotalSeconds;
        }
    }
}