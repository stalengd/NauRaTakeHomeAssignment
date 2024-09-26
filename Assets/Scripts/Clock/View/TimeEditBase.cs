using System;
using R3;
using UnityEngine;

namespace NauRa.ClockApp.Clock.View
{
    public abstract class TimeEditBase : MonoBehaviour
    {
        public abstract ReactiveProperty<DateTime?> Value { get; }
        public abstract void SetActive(bool isActive);
    }
}
