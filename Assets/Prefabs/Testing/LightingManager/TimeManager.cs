using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private const int SECONDS_IN_DAY = 86400;
    private const int SECONDS_IN_HOUR = 60 * 60;
    private const int SECONDS_IN_MINUTE = 60;
    [SerializeField, ReadOnly] private int seconds;
    [SerializeField, ReadOnly] private int hours;
    [SerializeField, ReadOnly] private int minutes;
    [SerializeField, ReadOnly] private int days;
    [SerializeField, ReadOnly] private string _formattedTime;

    [SerializeField, ReadOnly] private float _currentTime;
    [SerializeField, ReadOnly] private int _totalTime;
    [SerializeField] public float scale;

    public static TimeManager Instance;
    public event Action<int> onProcessTick;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        seconds = 0;
        hours = 0;
        minutes = 0;
        days = 0;
        _currentTime = 0.0f;
        _totalTime = 0;
        // scale = 1.0f;
    }

    private void Update()
    {
        ProcessTick();
        CalculateTime();
        // Debug.Log(TimeSpan.FromSeconds(_totalTime).ToString(@"hh\:mm\:ss"));
        // Debug.Log(TimeSpan.Parse(TimeSpan.FromSeconds(_totalTime).ToString(@"hh\:mm\:ss")).TotalSeconds);
    }

    public void ProgressTime(int timeToProgress)
    {
        _totalTime += timeToProgress;
    }

    private void ProcessTick()
    {
        _currentTime += Time.deltaTime * scale;
        if (!(_currentTime >= 1.0f)) return;
        var tick = Mathf.FloorToInt(_currentTime);
        _totalTime += tick;
        _currentTime -= tick;
        onProcessTick?.Invoke(tick);
    }

    public float GetCurrentTick()
    {
        return _currentTime;
    }

    public int GetTotalTime()
    {
        return _totalTime;
    }

    private void CalculateTime()
    {
        if (!Application.isPlaying) return;

        days = _totalTime / SECONDS_IN_DAY;
        hours = (_totalTime / SECONDS_IN_HOUR) % 24;
        minutes = (_totalTime / SECONDS_IN_MINUTE) % 60;
        seconds = _totalTime % 60;

        _formattedTime = $"{hours % 12:00}:{minutes:00}:{seconds:00}";
    }

    public String FormattedTime(int seconds)
    {
        int hoursFormat = (seconds / SECONDS_IN_HOUR);
        int minutesFormat = (seconds / SECONDS_IN_MINUTE) % 60;
        seconds %= 60;
        return $"{hoursFormat:00}:{Mathf.Abs(minutesFormat):00}:{Mathf.Abs(seconds):00}";
    }
}
