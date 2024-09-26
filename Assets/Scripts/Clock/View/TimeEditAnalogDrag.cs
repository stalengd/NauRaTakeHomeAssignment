using System;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NauRa.ClockApp.Clock.View
{
    public sealed class TimeEditAnalogDrag : TimeEditBase, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Transform _hourHand;
        [SerializeField] private Transform _minuteHand;
        [SerializeField] private Transform _secondHand;

        [SerializeField] private float _maxAngleToSelectHand = 5f;

        public override ReactiveProperty<DateTime?> Value { get; } = new();

        private Transform _selectedHand;

        public override void SetActive(bool isActive)
        {
            enabled = isActive;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _selectedHand = null;
            if (IsOverHand(_hourHand, eventData.position))
            {
                _selectedHand = _hourHand;
            }
            else if (IsOverHand(_minuteHand, eventData.position))
            {
                _selectedHand = _minuteHand;
            }
            else if (IsOverHand(_secondHand, eventData.position))
            {
                _selectedHand = _secondHand;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_selectedHand == null)
            {
                return;
            }
            var toPositionAngle = Quaternion.LookRotation(Vector3.forward, 
                eventData.position - (Vector2)_selectedHand.position).eulerAngles.z;
            var normalized = Mathf.Repeat(-toPositionAngle / 360f, 1f);
            if (normalized == 1f)
            {
                normalized = 0f;
            }
            var dateTime = Value.Value.GetValueOrDefault(DateTime.MinValue);
            var time = dateTime.TimeOfDay;
            if (_selectedHand == _hourHand)
            {
                if (time.Hours < 12)
                {
                    time = TimeSpan.FromHours(normalized * 12f);
                }
                else
                {
                    time = TimeSpan.FromHours(12 + normalized * 12f);
                }
            }
            else if (_selectedHand == _minuteHand)
            {
                time = TimeSpan.FromHours(time.Hours) + TimeSpan.FromMinutes(normalized * 60f);
            }
            else if (_selectedHand == _secondHand)
            {
                time = TimeSpan.FromHours(time.Hours) + 
                    TimeSpan.FromMinutes(time.Minutes) + 
                    TimeSpan.FromSeconds(normalized * 60f);
            }
            Value.Value = dateTime.Date + time;
        }

        public void OnEndDrag(PointerEventData eventData)
        {

        }

        private bool IsOverHand(Transform hand, Vector2 position)
        {
            var toPositionAngle = Quaternion.LookRotation(Vector3.forward, position - (Vector2)hand.position);
            var angleBetween = Quaternion.Angle(toPositionAngle, hand.rotation);
            return angleBetween <= _maxAngleToSelectHand;
        }
    }
}
