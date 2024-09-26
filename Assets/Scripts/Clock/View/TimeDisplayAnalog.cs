using System;
using DG.Tweening;
using UnityEngine;

namespace NauRa.ClockApp.Clock.View
{
    public sealed class TimeDisplayAnalog : TimeDisplayBase
    {
        [Header("Hands")]
        [SerializeField] private Transform _hourHand;
        [SerializeField] private Transform _minuteHand;
        [SerializeField] private Transform _secondHand;

        [Header("Animation")]
        [SerializeField] private float _tweenDurationSeconds = 0.5f;
        [SerializeField] private Ease _tweenEase = Ease.InOutQuad;
        [SerializeField] private float _tweenEaseOvershoot = 0.5f;

        public override void SetTime(DateTime? dateTime, bool isInstant)
        {
            var time = dateTime.GetValueOrDefault(DateTime.MinValue).TimeOfDay;
            SetHand(_hourHand, GetHoursAngle(time), isInstant);
            SetHand(_minuteHand, GetMinutesAngle(time), isInstant);
            SetHand(_secondHand, GetSecondsAngle(time), isInstant);
        }

        private void SetHand(Transform hand, float angle, bool isInstant)
        {
            var duration = isInstant ? 0f : _tweenDurationSeconds;
            var tweener = hand.DOLocalRotate(new(0, 0, Mathf.Repeat(angle, 360)), duration)
                .From(hand.rotation.eulerAngles)
                .SetEase(_tweenEase, _tweenEaseOvershoot)
                .SetAutoKill();
            tweener.fullPosition = 0f;
        }

        private static float GetHoursAngle(TimeSpan time)
        {
            return -(float)time.TotalHours / 12 * 360f;
        }

        private static float GetMinutesAngle(TimeSpan time)
        {
            return -(float)time.TotalMinutes / 60 * 360f;
        }

        private static float GetSecondsAngle(TimeSpan time)
        {
            return -(float)time.TotalSeconds / 60 * 360f;
        }
    }
}
