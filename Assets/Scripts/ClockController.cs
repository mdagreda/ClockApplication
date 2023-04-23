using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;

public class ClockController : MonoBehaviour
{
    [SerializeField] private TMP_Text[] currentTimeTexts;

    // Start is called before the first frame update
    void Start()
    {
        // Update the current time text every second
        Observable.Interval(System.TimeSpan.FromSeconds(1))
            .Subscribe(_ => {
                UpdateCurrentTime();
            })
            .AddTo(this);
    }

    void UpdateCurrentTime()
    {
        // Get the current system time
        System.DateTime currentTime = System.DateTime.Now;

        // Update the current time text to display the current time in 24 hour format
        string currentHour = currentTime.Hour.ToString("00");
        string currentMinute = currentTime.Minute.ToString("00");
        string currentSecond = currentTime.Second.ToString("00");

        string currentTimeString = $"{currentHour}:{currentMinute}.{currentSecond}";

        for (int i = 0; i < currentTimeTexts.Length; i++)
        {
            currentTimeTexts[i].text = currentTimeString;
        }
    }
}