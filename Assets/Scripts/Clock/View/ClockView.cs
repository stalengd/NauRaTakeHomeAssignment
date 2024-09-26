﻿using System;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace NauRa.ClockApp.Clock.View
{
    public sealed class ClockView : MonoBehaviour
    {
        [SerializeField] private Button _editAlarmButton;
        [SerializeField] private Button _clearAlarmButton;
        [SerializeField] private GameObject _editingIndicator;
        [SerializeField] private TimeDisplayBase _currentTimeDisplay;
        [SerializeField] private TimeDisplayBase _alarmDisplay;
        [SerializeField] private TimeEditBase _edit;

        [Header("Alarm Ring")]
        [SerializeField] private GameObject _alarmRingScreen;
        [SerializeField] private Button _stopAlarmRingButton;

        public Observable<Unit> EditAlarmClicked => _editAlarmButton.OnClickAsObservable();
        public Observable<Unit> ClearAlarmClicked => _clearAlarmButton.OnClickAsObservable();
        public Observable<Unit> StopAlarmRingClicked => _stopAlarmRingButton.OnClickAsObservable();
        public Observable<DateTime?> TimeEdited => _edit.Value;

        public void SetTime(DateTime? time, bool isInstant)
        {
            _currentTimeDisplay.SetTime(time, isInstant);
        }

        public void SetAlarm(DateTime? time, bool isInstant)
        {
            _alarmDisplay.SetTime(time, isInstant);
            _edit.Value.Value = time;
        }

        public void ToggleAlarmEdit(bool isEditActive)
        {
            _edit.SetActive(isEditActive);
            _editingIndicator.SetActive(isEditActive);
        }

        public void RingAlarm()
        {
            Debug.Log("Playing Clock Alarm");
            _alarmRingScreen.SetActive(true);
        }

        public void StopRingAlarm()
        {
            _alarmRingScreen.SetActive(false);
        }
    }
}
