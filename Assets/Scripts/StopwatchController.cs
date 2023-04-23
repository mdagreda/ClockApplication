using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;

/// <summary>
/// The Stopwatch controller handles the logic of starting, stopping and reseting the stopwatch
/// as well as updating the UI to display how much time has elasped.
/// </summary>
public class StopwatchController : MonoBehaviour
{
    /// <summary>
    /// The current Lap time text.
    /// </summary>
    [SerializeField] private TMP_Text CurrentLapTimeText;

    /// <summary>
    /// The Stopwatch toggle object that will trigger starting and stopping the stopwatch.
    /// </summary>
    [SerializeField] private Toggle StopwatchToggle;

    /// <summary>
    /// This is the clear button that will reset the time on the stopwatch back to 0.
    /// </summary>
    [SerializeField] private Button ClearButton;

    /// <summary>
    /// This is the current lap time the stopwatch has recorded.
    /// </summary>
    private float currentLapTime = 0f;

    /// <summary>
    /// True if the stopwatch is running.
    /// </summary>
    private bool stopwatchRunning = false;

    /// <summary>
    /// Setsup the events for the buttons and updating the stopwatch time.
    /// </summary>
    private void Start()
    {
        StopwatchToggle.OnValueChangedAsObservable()
            .Subscribe(isOn => {
                if (isOn)
                {
                    stopwatchRunning = true;
                }
                else
                {
                    stopwatchRunning = false;
                }
            })
            .AddTo(this);

        ClearButton.OnClickAsObservable()
            .Subscribe(_ => {
                ResetStopwatch();
            })
            .AddTo(this);

        Observable.EveryUpdate()
            .Where(_ => stopwatchRunning)
            .Subscribe(_ => {
                UpdateCurrentLapTime();
                UpdateCurrentLapTimeText();
            })
            .AddTo(this);
    }

    /// <summary>
    /// Resets the toggle for the stopwatch to false if it is on and reset the current lap time to 0.
    /// </summary>
    private void ResetStopwatch()
    {
        if (StopwatchToggle.isOn)
        {
            StopwatchToggle.isOn = false;
        }
        currentLapTime = 0f;
        UpdateCurrentLapTimeText();
    }

    /// <summary>
    /// Updates the current lap time.
    /// </summary>
    private void UpdateCurrentLapTime()
    {
        currentLapTime += Time.deltaTime;
    }

    /// <summary>
    /// Creates the text for the lap time and sets it to the current lap time text object.
    /// </summary>
    private void UpdateCurrentLapTimeText()
    {
        int minutes = Mathf.FloorToInt(currentLapTime / 60f);
        int seconds = Mathf.FloorToInt(currentLapTime % 60f);
        int milliseconds = Mathf.FloorToInt((currentLapTime - Mathf.Floor(currentLapTime)) * 100f);

        string currentLapTimeString = $"{minutes:00}:{seconds:00}.{milliseconds:00}";
        CurrentLapTimeText.text = currentLapTimeString;
    }
}
