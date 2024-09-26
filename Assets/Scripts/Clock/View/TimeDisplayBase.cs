using System;
using UnityEngine;

namespace NauRa.ClockApp.Clock.View
{
    public abstract class TimeDisplayBase : MonoBehaviour
    {
        public abstract void SetTime(DateTime? time, bool isInstant);
    }
}
