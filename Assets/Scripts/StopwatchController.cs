using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;

namespace ClockApplication
{
    /// <summary>
    /// The Stopwatch controller handles the logic of starting, stopping and reseting the stopwatch
    /// as well as updating the UI to display how much time has elasped.
    /// </summary>
    public class StopwatchController : MonoBehaviour, IStopwatchController
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
        /// This is to to dispose of events if the object is destroyed.
        /// </summary>
        private CompositeDisposable disposables;

        /// <summary>
        /// This is the current lap time the stopwatch has recorded property.
        /// </summary>
        public float CurrentLapTime
        {
            get { return currentLapTime; }
        }

        /// <summary>
        /// Setsup the events for the buttons and updating the stopwatch time.
        /// </summary>
        private void Start()
        {
            disposables = new CompositeDisposable();

            StopwatchToggle.OnValueChangedAsObservable()
                .Subscribe(isOn =>
                {
                    if (isOn)
                    {
                        StartStopwatch();
                    }
                    else
                    {
                        StopStopwatch();
                    }
                })
                .AddTo(disposables);

            ClearButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    ResetStopwatch();
                })
                .AddTo(disposables);

            Observable.EveryUpdate()
                .Where(_ => stopwatchRunning)
                .Subscribe(_ =>
                {
                    UpdateCurrentLapTime();
                    UpdateCurrentLapTimeText();
                })
                .AddTo(disposables);
        }

        /// <summary>
        /// Destorys the disposables if not null when this object is destroyed.
        /// </summary>
        private void OnDestroy()
        {
            disposables?.Dispose();
        }

        /// <summary>
        /// Stops the stopwatch from running.
        /// </summary>
        public void StopStopwatch()
        {
            stopwatchRunning = false;
        }

        /// <summary>
        /// Starts the stopwatch running.
        /// </summary>
        public void StartStopwatch()
        {
            stopwatchRunning = true;
        }

        /// <summary>
        /// Resets the toggle for the stopwatch to false if it is on and reset the current lap time to 0.
        /// </summary>
        public void ResetStopwatch()
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
}
