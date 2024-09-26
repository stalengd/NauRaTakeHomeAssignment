using System;
using UnityEngine;

namespace NauRa.ClockApp.Clock.View
{
    public sealed class TimeDisplayComposed : TimeDisplayBase
    {
        [SerializeField] private TimeDisplayBase[] _displays;

        public override void SetTime(DateTime? time, bool isInstant)
        {
            foreach (var view in _displays)
            {
                view.SetTime(time, isInstant);
            }
        }
    }
}
