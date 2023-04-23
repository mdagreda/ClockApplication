using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using System;

/// <summary>
/// This class controls the functionality of the time to set a timer and start and stop a time.
/// </summary>
public class TimerController : MonoBehaviour
{
    /// <summary>
    /// The input panel where the time for the timer is set.
    /// </summary>
    [SerializeField] private GameObject InputPanel;

    /// <summary>
    /// The timer panel where the time countdown is visable.
    /// </summary>
    [SerializeField] private GameObject TimerPanel;
    
    /// <summary>
    /// The input field for hours.
    /// </summary>
    [SerializeField] private TMP_InputField HourInputField;

    /// <summary>
    /// The input filed for minutes.
    /// </summary>
    [SerializeField] private TMP_InputField MinuteInputField;

    /// <summary>
    /// The input filed for seconds.
    /// </summary>
    [SerializeField] private TMP_InputField SecondInputField;

    /// <summary>
    /// The Error message text for hours.
    /// </summary>
    [SerializeField] private TMP_Text HourErrorText;

    /// <summary>
    /// The error message text for minutes.
    /// </summary>
    [SerializeField] private TMP_Text MinuteErrorText;

    /// <summary>
    /// The error message text for seconds.
    /// </summary>
    [SerializeField] private TMP_Text SecondErrorText;

    /// <summary>
    /// The text for the timer time remaining.
    /// </summary>
    [SerializeField] private TMP_Text TimerText;

    /// <summary>
    /// The button to stop the timer.
    /// </summary>
    [SerializeField] private Button StopButton;

    /// <summary>
    /// The button to reset the timer.
    /// </summary>
    [SerializeField] private Button ResetButton;

    /// <summary>
    /// The toggle to start and pause the timer.
    /// </summary>
    [SerializeField] private Toggle StartToggle;

    /// <summary>
    /// The timer is finished audio source.
    /// </summary>
    [SerializeField] private AudioSource TimerAudioSource;

    /// <summary>
    /// The audio clip to play when the timer is finished.
    /// </summary>
    [SerializeField] private AudioClip TimerEndClip;

    /// <summary>
    /// The total seconds for the timer.
    /// </summary>
    private int totalSeconds = 0;

    /// <summary>
    /// True if the timer is running.
    /// </summary>
    private bool isTimerRunning = false;

    /// <summary>
    /// How many seconds have elasped on this timer.
    /// </summary>
    private float elapsedSeconds = 0f;

    /// <summary>
    /// True if the timer is paused.
    /// </summary>
    private bool TimerPaused = false;

    /// <summary>
    /// This is to to dispose of events if the object is destoryed.
    /// </summary>
    private CompositeDisposable disposables;

    /// <summary>
    /// At the start assigns all the events for the UI such as stoping, resetting, and pausing.
    /// </summary>
    private void Start()
    {
        disposables = new CompositeDisposable();
        StopButton.OnClickAsObservable().Subscribe(_ => StopTimer(false)).AddTo(disposables);
        ResetButton.OnClickAsObservable().Subscribe(_ => ClearInputFields()).AddTo(disposables);
        StartToggle.OnValueChangedAsObservable()
            .Subscribe(isOn =>
            {
                if (isOn)
                {
                    if (TimerPaused)
                    {
                        isTimerRunning = true;
                        TimerPaused = false;
                    }
                    else
                    {
                        if (GetTotalSecondsFromInputFields())
                        {
                            if (TimerAudioSource.isPlaying)
                            {
                                TimerAudioSource.Stop();
                            }
                            isTimerRunning = true;
                            InputPanel.SetActive(false);
                            TimerPanel.SetActive(true);
                        }
                        else
                        {
                            StartToggle.isOn = false;
                        }
                    }
                }
                else
                {
                    if (isTimerRunning)
                    {
                        TimerPaused = true;
                    }
                    isTimerRunning = false;
                }
            })
            .AddTo(disposables);
    }

    /// <summary>
    /// Updates the timer if the timer is running and stops it when the timer is finished.
    /// </summary>
    private void Update()
    {
        if (isTimerRunning)
        {
            elapsedSeconds += Time.deltaTime;
            if (elapsedSeconds >= 1f)
            {
                totalSeconds--;
                elapsedSeconds = 0f;
                if (totalSeconds <= 0)
                {
                    StopTimer(true);
                }
                else
                {
                    UpdateTimerText();
                }
            }
        }
    }

    /// <summary>
    /// Updates the timer with the time remaining.
    /// </summary>
    private void UpdateTimerText()
    {
        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;
        string timerString = $"{hours:00}:{minutes:00}:{seconds:00}";
        TimerText.text = timerString;
    }

    /// <summary>
    /// First this function does error checking ot make sure the numbers input for the timer are valid.
    /// Then the total second for the timer is detmerined and if the input is valid the fucntion will 
    /// return true.
    /// </summary>
    /// <returns>If the timer input is a valid time to use</returns>
    private bool GetTotalSecondsFromInputFields()
    {
        int hours;
        bool isValidTime = true;
        if (!int.TryParse(HourInputField.text, out hours) || hours < 0 || hours >= 1000000)
        {
            HourErrorText.text = "Enter a valid number (0-1,000,000)";
            isValidTime = false;
        }
        else
        {
            HourErrorText.text = "";
        }

        int minutes;
        if (!int.TryParse(MinuteInputField.text, out minutes) || minutes < 0 || minutes >= 60)
        {
            MinuteErrorText.text = "Enter a valid number (0-59)";
            isValidTime = false;
        }
        else
        {
            MinuteErrorText.text = "";
        }

        int seconds;
        if (!int.TryParse(SecondInputField.text, out seconds) || seconds < 0 || seconds >= 60)
        {
            SecondErrorText.text = "Enter a valid number (0-59)";
            isValidTime = false;
        }
        else
        {
            SecondErrorText.text = "";
        }

        totalSeconds = hours * 3600 + minutes * 60 + seconds;
        if(totalSeconds <= 0)
        {
            isValidTime = false;
        }
        UpdateTimerText();
        return isValidTime;
    }

    /// <summary>
    /// Stops the timer and switches back to the input panel. If the timer finished then a nosie is played.
    /// </summary>
    /// <param name="playNoise">If the timer finished then this should be true so a noise will be played.</param>
    private void StopTimer(bool playNoise)
    {
        isTimerRunning = false;
        TimerPaused = false;

        InputPanel.SetActive(true);
        TimerPanel.SetActive(false);

        StartToggle.isOn = false;

        if (playNoise)
        {
            TimerAudioSource.PlayOneShot(TimerEndClip);
        }
    }

    /// <summary>
    /// Clears the input fields back to default values.
    /// </summary>
    private void ClearInputFields()
    {
        HourInputField.text = "0";
        MinuteInputField.text = "0";
        SecondInputField.text = "0";
    }

    /// <summary>
    /// Destorys the disposables if not null when this object is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        disposables?.Dispose();
    }

}