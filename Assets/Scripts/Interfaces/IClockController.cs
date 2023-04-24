namespace ClockApplication
{
    /// <summary>
    /// This Interface is the basis for a clock that find the time and can return that time a string.
    /// this app is running on.
    /// </summary>
    public interface IClockController
    {
        /// <summary>
        /// Gets the current time according to the machine this app.
        void UpdateCurrentTime();

        /// <summary>
        /// Get the current time in text.
        /// </summary>
        /// <returns>The current time as a string</returns>
        string GetCurrentTimeText();
    }
}
