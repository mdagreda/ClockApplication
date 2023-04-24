using UniRx;

namespace ClockApplication
{
    /// <summary>
    /// The clock manager handles switching between the different tabs in the clock app.
    /// </summary>
    public interface IClockManager
    {
        /// <summary>
        /// Handles changing tabs by adjusting their visibility and if they are blocking raycasts.
        /// </summary>
        /// <param name="tab">The tab to switch to</param>
        void ChangeTab(TabType tab);

        /// <summary>
        /// The property to acess the clock controller used in this clock manager.
        /// </summary>
        IClockController Clock { get; }

        /// <summary>
        /// The property to acess the stopwatch controller used in this clock manager.
        /// </summary>
        IStopwatchController Stopwatch { get; }

        /// <summary>
        /// Local instance variable for clock manager.
        /// </summary>
        static IClockManager Instance { get; }
    }
}
