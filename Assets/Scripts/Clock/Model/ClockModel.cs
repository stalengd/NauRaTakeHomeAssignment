using System;
using R3;

namespace NauRa.ClockApp.Clock.Model
{
    public sealed class ClockModel
    {
        public ReadOnlyReactiveProperty<DateTime?> CurrentTime => _currentTime;
        public ReadOnlyReactiveProperty<DateTime?> AlarmTime => _alarmTime;
        public Observable<DateTime> AlarmRinged => _alarmRinged;

        private DateTime? _baselineTime;
        private TimeSpan _passedFromBaseline;
        private readonly ReactiveProperty<DateTime?> _currentTime = new(null);
        private readonly ReactiveProperty<DateTime?> _alarmTime = new(null);
        private readonly ReactiveCommand<DateTime> _alarmRinged = new();

        public void SetAbsoluteTime(DateTime time)
        {
            _baselineTime = time;
            _passedFromBaseline = TimeSpan.Zero;
            RefreshCurrentTime();
        }

        public void SetAlarm(DateTime time)
        {
            _alarmTime.Value = time;
        }

        public void ClearAlarm()
        {
            _alarmTime.Value = null;
        }

        public void Tick(TimeSpan amount)
        {
            if (amount < TimeSpan.Zero)
            {
                throw new ArgumentException("You should not tick negative amount of time", nameof(amount));
            }
            _passedFromBaseline += amount;
            var prevTime = _currentTime.Value;
            RefreshCurrentTime();
            TryRingAlarm(prevTime, _currentTime.Value);
        }

        private void RefreshCurrentTime()
        {
            if (_baselineTime is not { } baselineTime)
            {
                _currentTime.Value = null;
                return;
            }
            _currentTime.Value = baselineTime + _passedFromBaseline;
        }

        private bool TryRingAlarm(DateTime? prevTimeMaybe, DateTime? currentTimeMaybe)
        {
            if (prevTimeMaybe is not { } prevTime ||
                currentTimeMaybe is not { } currentTime ||
                _alarmTime.Value is not { } alarmTime)
            {
                return false;
            }
            if (prevTime.TimeOfDay >= alarmTime.TimeOfDay || currentTime.TimeOfDay < alarmTime.TimeOfDay)
            {
                return false;
            }
            _alarmRinged.Execute(alarmTime);
            return true;
        }
    }
}