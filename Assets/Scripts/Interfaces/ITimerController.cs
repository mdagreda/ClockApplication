using UnityEngine.UI;
using TMPro;

namespace ClockApplication
{
    /// <summary>
    /// This interface defines the functionality of the timer to set a timer and start and stop a timer.
    /// </summary>
    public interface ITimerController
    {
        /// <summary>
        /// Stops the timer and switches back to the input panel. If the timer finished then a nosie is played.
        /// </summary>
        /// <param name="isFinished">If the timer finished then this should be true so a noise will be played.</param>
        void StopTimer(bool isFinished);

        /// <summary>
        /// Clears the input fields back to default values.
        /// </summary>
        void ClearInputFields();

        /// <summary>
        /// First this function does error checking ot make sure the numbers input for the timer are valid.
        /// Then the total second for the timer is detmerined and if the input is valid the fucntion will 
        /// return true.
        /// </summary>
        /// <returns>If the timer input is a valid time to use</returns>
        bool GetAndValidateTimerInput();

        /// <summary>
        /// Handles starting the timer.
        /// </summary>
        void StartTimer();

        /// <summary>
        /// Handles pausing the timer.
        /// </summary>
        void PauseTimer();

        /// <summary>
        /// Manually assigns the input fields used in timer.
        /// </summary>
        /// <param name="hours">Number of hours for the timer</param>
        /// <param name="minutes">Number of minutes for the timer</param>
        /// <param name="seconds">Number of seconds for the timer</param>
        void InputTimerTime(int hours, int minutes, int seconds);

        /// <summary>
        /// Property to get the total seconds remaining on the timer.
        /// </summary>
        int TotalSecondsRemaining { get; }
    }
}