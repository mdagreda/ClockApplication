using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using System;

namespace ClockApplication
{
    /// <summary>
    /// This class controls the functionality of the timer to set a timer and start and stop a timer.
    /// </summary>
    public class TimerController : MonoBehaviour, ITimerController
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
        /// The toggle to start and pause the timer.
        /// </summary>
        [SerializeField] private Toggle ChangeViewToggle;

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
        private int totalSecondsRemaining = 0;

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
        /// Property to get the total seconds remaining on the timer.
        /// </summary>
        public int TotalSecondsRemaining
        {
            get { return totalSecondsRemaining; }
        }

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
                        StartTimer();
                    }
                    else
                    {
                        PauseTimer();
                    }
                })
                .AddTo(disposables);

            ChangeViewToggle.OnValueChangedAsObservable()
                .Subscribe(isOn =>
                {
                    if (isOn)
                    {
                        TogglePanelView(true);
                    }
                    else
                    {
                        TogglePanelView(false);
                    }
                })
                .AddTo(disposables);
        }

        /// <summary>
        /// Handles starting the timer.
        /// </summary>
        public void StartTimer()
        {
            if (TimerPaused)
            {
                isTimerRunning = true;
                TimerPaused = false;
            }
            else
            {
                if (GetAndValidateTimerInput())
                {
                    if (TimerAudioSource.isPlaying)
                    {
                        TimerAudioSource.Stop();
                    }
                    isTimerRunning = true;
                    ChangeViewToggle.isOn = true;
                }
                else
                {
                    StartToggle.isOn = false;
                }
            }
        }

        /// <summary>
        /// Handles pausing the timer.
        /// </summary>
        public void PauseTimer()
        {
            if (isTimerRunning)
            {
                TimerPaused = true;
            }
            isTimerRunning = false;
        }

        /// <summary>
        /// Manually assigns the input fields used in timer.
        /// </summary>
        /// <param name="hours">Number of hours for the timer</param>
        /// <param name="minutes">Number of minutes for the timer</param>
        /// <param name="seconds">Number of seconds for the timer</param>
        public void InputTimerTime(int hours, int minutes, int seconds)
        {
            HourInputField.text = hours.ToString();

            MinuteInputField.text = minutes.ToString();

            SecondInputField.text = seconds.ToString();
        }

        /// <summary>
        /// Changes the view between the input panel and the timer panel.
        /// </summary>
        /// <param name="toTimerPanel">If true show the timer panel and if false show the input panel</param>
        private void TogglePanelView(bool toTimerPanel)
        {
            if (toTimerPanel)
            {
                InputPanel.SetActive(false);
                TimerPanel.SetActive(true);
            }
            else
            {
                InputPanel.SetActive(true);
                TimerPanel.SetActive(false);
            }
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
                    totalSecondsRemaining--;
                    elapsedSeconds = 0f;
                    if (totalSecondsRemaining <= 0)
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
            int hours = totalSecondsRemaining / 3600;
            int minutes = (totalSecondsRemaining % 3600) / 60;
            int seconds = totalSecondsRemaining % 60;
            string timerString = $"{hours:00}:{minutes:00}:{seconds:00}";
            TimerText.text = timerString;
        }

        /// <summary>
        /// First this function does error checking ot make sure the numbers input for the timer are valid.
        /// Then the total second for the timer is detmerined and if the input is valid the fucntion will 
        /// return true.
        /// </summary>
        /// <returns>If the timer input is a valid time to use</returns>
        public bool GetAndValidateTimerInput()
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

            totalSecondsRemaining = hours * 3600 + minutes * 60 + seconds;
            if (totalSecondsRemaining <= 0)
            {
                isValidTime = false;
            }
            UpdateTimerText();
            return isValidTime;
        }

        /// <summary>
        /// Stops the timer and switches back to the input panel. If the timer finished then a nosie is played.
        /// </summary>
        /// <param name="isFinished">If the timer finished then this should be true so a noise will be played.</param>
        public void StopTimer(bool isFinished)
        {
            isTimerRunning = false;
            TimerPaused = false;

            ChangeViewToggle.isOn = false;

            StartToggle.isOn = false;

            TimerText.text = "00:00:00";

            if (isFinished)
            {
                TimerAudioSource.PlayOneShot(TimerEndClip);
            }
        }

        /// <summary>
        /// Clears the input fields back to default values.
        /// </summary>
        public void ClearInputFields()
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
}