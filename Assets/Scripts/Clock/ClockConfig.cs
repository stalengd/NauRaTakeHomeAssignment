using System;
using UnityEngine;

namespace NauRa.ClockApp.Clock
{
    [CreateAssetMenu(menuName = "Configs/Clock")]
    public sealed class ClockConfig : ScriptableObject
    {
        [SerializeField] private float _currentTimeLoadIntervalSeconds;

        public TimeSpan CurrentTimeLoadInterval => TimeSpan.FromSeconds(_currentTimeLoadIntervalSeconds);
    }
}