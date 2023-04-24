using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClockApplication
{
    /// <summary>
    /// The Stopwatch controller handles the logic of starting, stopping and reseting the stopwatch
    /// as well as updating the UI to display how much time has elasped.
    /// </summary>
    public interface IStopwatchController
    {
        /// <summary>
        /// Resets the toggle for the stopwatch to false if it is on and reset the current lap time to 0.
        /// </summary>
        void ResetStopwatch();

        /// <summary>
        /// Stops the stopwatch from running.
        /// </summary>
        void StopStopwatch();

        /// <summary>
        /// Starts the stopwatch running.
        /// </summary>
        void StartStopwatch();

        /// <summary>
        /// This is the current lap time the stopwatch has recorded property.
        /// </summary>
        float CurrentLapTime { get; }
    }
}