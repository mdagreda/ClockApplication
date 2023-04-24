using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace ClockApplication
{
    /// <summary>
    /// The clock manager handles switching between the different tabs in the clock app.
    /// </summary>
    public class ClockManager : MonoBehaviour, IClockManager
    {
        /// <summary>
        /// The toggle to switch to the clock tab.
        /// </summary>
        [SerializeField] private Toggle ClockTabToggle;

        /// <summary>
        /// The toggle to switch to the stopwatch tab.
        /// </summary>
        [SerializeField] private Toggle StopwatchTabToggle;

        /// <summary>
        /// The toggle to switch to the timer tab.
        /// </summary>
        [SerializeField] private Toggle TimerTabToggle;

        /// <summary>
        /// The Clock panel canvas group. Used for controlling visability and blocking raycasts.
        /// </summary>
        [SerializeField] private CanvasGroup ClockPanelCanvasGroup;

        /// <summary>
        /// The Stopwatch panel canvas group. Used for controlling visability and blocking raycasts.
        /// </summary>
        [SerializeField] private CanvasGroup StopWatchPanelCanvasGroup;

        /// <summary>
        /// The Timer panel canvas group. Used for controlling visability and blocking raycasts.
        /// </summary>
        [SerializeField] private CanvasGroup TimerPanelCanvasGroup;

        /// <summary>
        /// The Top of the screen clock panel canvas group. Used for controlling visability and blocking raycasts.
        /// </summary>
        [SerializeField] private CanvasGroup TopClockPanelCanvasGroup;

        /// <summary>
        /// The clock controller controlling the clocks in this clock manager.
        /// </summary>
        [SerializeField] private ClockController clock;

        /// <summary>
        /// The stopwatch controller controlling the stopwatch in this clock manager.
        /// </summary>
        [SerializeField] private StopwatchController stopwatch;

        /// <summary>
        /// The timer controller controlling the timer in this clock manager.
        /// </summary>
        [SerializeField] private TimerController timerForClock;

        /// <summary>
        /// This is to to dispose of events if the object is destroyed.
        /// </summary>
        private CompositeDisposable disposables;

        /// <summary>
        /// Local instance variable for clock manager.
        /// </summary>
        private static IClockManager instance;

        /// <summary>
        /// The Instance property to access the clock manager as a singleton.
        /// </summary>
        public static IClockManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<ClockManager>();
                }

                return instance;
            }
        }

        /// <summary>
        /// The property to acess the clock controller used in this clock manager.
        /// </summary>
        public IClockController Clock {
            get { return clock; }
        }

        /// <summary>
        /// The property to acess the stopwatch controller used in this clock manager.
        /// </summary>
        public IStopwatchController Stopwatch {
            get { return stopwatch; }
        }

        /// <summary>
        /// The property to acess the timer controller used in this clock manager.
        /// </summary>
        public ITimerController TimerForClock
        {
            get { return timerForClock; }
        }

        /// <summary>
        /// Sets the instance of the clock manager to this object and if not null destorys this object.
        /// </summary>
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            SetupEvents();
        }

        /// <summary>
        /// Setsup all the changing tab events for the toggles.
        /// </summary>
        void SetupEvents()
        {
            disposables = new CompositeDisposable();

            ClockTabToggle.OnValueChangedAsObservable()
                .Where(isOn => isOn)
                .Subscribe(_ => ChangeTab(TabType.ClockTab))
                .AddTo(disposables);

            StopwatchTabToggle.OnValueChangedAsObservable()
                .Where(isOn => isOn)
                .Subscribe(_ => ChangeTab(TabType.StopwatchTab))
                .AddTo(disposables);

            TimerTabToggle.OnValueChangedAsObservable()
                .Where(isOn => isOn)
                .Subscribe(_ => ChangeTab(TabType.TimerTab))
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
        /// Handles changing tabs by adjusting their visibility and if they are blocking raycasts.
        /// </summary>
        /// <param name="tab">The tab to switch to</param>
        public void ChangeTab(TabType tab)
        {

            HideCanvasGroups();

            switch (tab)
            {
                case TabType.ClockTab:
                    ClockPanelCanvasGroup.alpha = 1;
                    break;
                case TabType.StopwatchTab:
                    StopWatchPanelCanvasGroup.alpha = 1;
                    StopWatchPanelCanvasGroup.blocksRaycasts = true;
                    TopClockPanelCanvasGroup.alpha = 1;
                    break;
                case TabType.TimerTab:
                    TimerPanelCanvasGroup.alpha = 1;
                    TimerPanelCanvasGroup.blocksRaycasts = true;
                    TopClockPanelCanvasGroup.alpha = 1;
                    break;
                default:
                    Debug.LogError("Tab type functionality not implemented.");
                    break;
            }
        }

        /// <summary>
        /// Turns off visibility for all canvas groups and turns off block raycast for ones that have
        /// interactive functions.
        /// </summary>
        void HideCanvasGroups()
        {
            ClockPanelCanvasGroup.alpha = 0;
            StopWatchPanelCanvasGroup.alpha = 0;
            StopWatchPanelCanvasGroup.blocksRaycasts = false;
            TimerPanelCanvasGroup.alpha = 0;
            TimerPanelCanvasGroup.blocksRaycasts = false;
            TopClockPanelCanvasGroup.alpha = 0;
        }
    }
}
