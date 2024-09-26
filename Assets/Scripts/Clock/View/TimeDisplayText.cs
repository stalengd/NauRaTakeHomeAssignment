using System;
using System.Text;
using TMPro;
using UnityEngine;

namespace NauRa.ClockApp.Clock.View
{
    public sealed class TimeDisplayText : TimeDisplayBase
    {
        [SerializeField] private string _format = "{0:HH:mm:ss}";
        [SerializeField] private string _noneTimeString = "--:--:--";
        [SerializeField] private TMP_Text _text;

        private readonly StringBuilder _buffer = new();

        public override void SetTime(DateTime? time, bool isInstant)
        {
            if (time is { } someTime)
            {
                _buffer.Clear();
                _buffer.AppendFormat(_format, someTime);
                _text.SetText(_buffer);
            }
            else
            {
                _text.text = _noneTimeString;
            }
        }
    }
}
