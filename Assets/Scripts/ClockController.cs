using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;

/// <summary>
/// This class set the time on the clock to whatever the current time is according to the machine 
/// this app is running on.
/// </summary>
public class ClockController : MonoBehaviour
{
    [SerializeField] private TMP_Text[] currentTimeTexts;

    /// <summary>
    /// Setting up the update for the clock to update the current time every second
    /// </summary>
    void Start()
    {
        // Update the current time text every second
        Observable.Interval(System.TimeSpan.FromSeconds(1))
            .Subscribe(_ => {
                UpdateCurrentTime();
            })
            .AddTo(this);
    }

    /// <summary>
    /// Gets the current time according to the machine this app is running on and then updates 
    /// all the current time text objects that are assigned.
    /// </summary>
    void UpdateCurrentTime()
    {
        // Get the current system time
        System.DateTime currentTime = System.DateTime.Now;

        // Update the current time text to display the current time in 24 hour format
        string currentHour = currentTime.Hour.ToString("00");
        string currentMinute = currentTime.Minute.ToString("00");
        string currentSecond = currentTime.Second.ToString("00");

        string currentTimeString = $"{currentHour}:{currentMinute}:{currentSecond}";

        for (int i = 0; i < currentTimeTexts.Length; i++)
        {
            currentTimeTexts[i].text = currentTimeString;
        }
    }
}