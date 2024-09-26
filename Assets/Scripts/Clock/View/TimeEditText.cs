using System;
using System.Globalization;
using R3;
using TMPro;
using UnityEngine;

namespace NauRa.ClockApp.Clock.View
{
    public sealed class TimeEditText : TimeEditBase
    {
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private string _format = "hh\\:mm\\:ss";

        public override ReactiveProperty<DateTime?> Value { get; } = new();

        private void Awake()
        {
            _inputField.onEndEdit.AddListener(OnTextChanged);
            Value
                .Subscribe(_ => SetTextToCurrentTime());
        }

        public override void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
            if (isActive)
            {
                SetTextToCurrentTime();
            }
        }

        private void OnTextChanged(string text)
        {
            var formatter = CultureInfo.InvariantCulture;
            if (!TimeSpan.TryParseExact(_inputField.text, _format, formatter, out var time))
            {
                SetTextToCurrentTime();
                return;
            }
            Value.Value = Value.Value.GetValueOrDefault(DateTime.MinValue).Date + time;
        }

        private void SetTextToCurrentTime()
        {
            if (!enabled)
            {
                return;
            }
            _inputField.text = Value.Value.GetValueOrDefault(DateTime.MinValue).TimeOfDay.ToString(_format);
        }
    }
}
