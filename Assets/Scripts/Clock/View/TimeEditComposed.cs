using System;
using System.Linq;
using R3;
using UnityEngine;

namespace NauRa.ClockApp.Clock.View
{
    public sealed class TimeEditComposed : TimeEditBase
    {
        [SerializeField] private TimeEditBase[] _timeEdits;

        public override ReactiveProperty<DateTime?> Value { get; } = new();

        private DisposableBag _disposables;
        private bool _isValueBlocked = false;

        private void Awake()
        {
            _timeEdits.Select(x => x.Value)
                .Merge()
                .Subscribe(OnInnerValuesChange)
                .AddTo(ref _disposables);
            Value
                .Subscribe(ChangeInnerValues)
                .AddTo(ref _disposables);
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }

        public override void SetActive(bool isActive)
        {
            foreach (var timeEdit in _timeEdits)
            {
                timeEdit.SetActive(isActive);
            }
        }

        private void OnInnerValuesChange(DateTime? value)
        {
            if (_isValueBlocked)
            {
                return;
            }
            Value.Value = value;
        }

        private void ChangeInnerValues(DateTime? value)
        {
            _isValueBlocked = true;
            foreach (var timeEdit in _timeEdits)
            {
                timeEdit.Value.Value = value;
            }
            _isValueBlocked = false;
        }
    }
}
