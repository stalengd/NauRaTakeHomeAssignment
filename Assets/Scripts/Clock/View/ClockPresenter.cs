using System;
using NauRa.ClockApp.Clock.Model;
using R3;
using Zenject;

namespace NauRa.ClockApp.Clock.View
{
    public sealed class ClockPresenter : IInitializable, IDisposable
    {
        private readonly ClockModel _clockModel;
        private readonly ClockView _clockView;

        private DisposableBag _disposables;
        private bool _isEditing = false;
        private DateTime? _editedTime;

        public ClockPresenter(ClockModel clockModel, ClockView clockView)
        {
            _clockModel = clockModel;
            _clockView = clockView;
        }

        public void Initialize()
        {
            DisplayTime(_clockModel.CurrentTime.CurrentValue, isInstant: true);
            DisplayAlarm(_clockModel.AlarmTime.CurrentValue, isInstant: true);
            _clockView.ToggleAlarmEdit(false);
            _clockModel.CurrentTime
                .Subscribe(OnCurrentTimeChanged)
                .AddTo(ref _disposables);
            _clockModel.AlarmTime
                .Subscribe(OnAlarmTimeChanged)
                .AddTo(ref _disposables);
            _clockModel.AlarmRinged
                .Subscribe(OnAlarmRing)
                .AddTo(ref _disposables);
            _clockView.EditAlarmClicked
                .Subscribe(_ => OnEditAlarmPressed())
                .AddTo(ref _disposables);
            _clockView.ClearAlarmClicked
                .Subscribe(_ => OnClearAlarmPressed())
                .AddTo(ref _disposables);
            _clockView.TimeEdited
                .Subscribe(OnTimeEdited)
                .AddTo(ref _disposables);
            _clockView.StopAlarmRingClicked
                .Subscribe(_ => OnStopAlarmRingPressed())
                .AddTo(ref _disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        private void DisplayTime(DateTime? time, bool isInstant = false)
        {
            _clockView.SetTime(time, isInstant);
        }

        private void DisplayAlarm(DateTime? time, bool isInstant = false)
        {
            _clockView.SetAlarm(time, isInstant);
        }

        private void OnEditAlarmPressed()
        {
            _isEditing = !_isEditing;
            _clockView.ToggleAlarmEdit(_isEditing);
            if (_isEditing)
            {
                _editedTime = null;
            }
            else
            {
                if (_editedTime.HasValue)
                {
                    _clockModel.SetAlarm(_editedTime.Value);
                }
            }
        }

        private void OnClearAlarmPressed()
        {
            _clockModel.ClearAlarm();
        }

        private void OnTimeEdited(DateTime? time)
        {
            _editedTime = time;
            DisplayAlarm(time, isInstant: true);
        }

        private void OnCurrentTimeChanged(DateTime? time)
        {
            DisplayTime(time, isInstant: false);
        }

        private void OnAlarmTimeChanged(DateTime? time)
        {
            DisplayAlarm(time, isInstant: false);
        }

        private void OnAlarmRing(DateTime time)
        {
            _clockView.RingAlarm();
        }

        private void OnStopAlarmRingPressed()
        {
            _clockView.StopRingAlarm();
        }
    }
}
