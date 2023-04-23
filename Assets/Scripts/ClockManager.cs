using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.EventSystems;

/// <summary>
/// The clock manager handles switching between the different tabs in the clock app.
/// </summary>
public class ClockManager : MonoBehaviour
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
    /// An enum for the different tabs in the clock app
    /// </summary>
    public enum TabType
    {
        ClockTab,
        StopwatchTab,
        TimerTab
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
        ClockTabToggle.OnValueChangedAsObservable()
            .Where(isOn => isOn)
            .Subscribe(_ => ChangeTab(TabType.ClockTab))
            .AddTo(this);

        StopwatchTabToggle.OnValueChangedAsObservable()
            .Where(isOn => isOn)
            .Subscribe(_ => ChangeTab(TabType.StopwatchTab))
            .AddTo(this);

        TimerTabToggle.OnValueChangedAsObservable()
            .Where(isOn => isOn)
            .Subscribe(_ => ChangeTab(TabType.TimerTab))
            .AddTo(this);
    }

    /// <summary>
    /// Handles changing tabs by adjusting their visibility and if they are blocking raycasts.
    /// </summary>
    /// <param name="tab">The tab to switch to</param>
    void ChangeTab(TabType tab)
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
