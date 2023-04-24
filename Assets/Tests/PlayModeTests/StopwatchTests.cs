using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace ClockApplication.Tests
{
    public class StopwatchTests
    {
        private IStopwatchController stopwatchController;

        private bool firstTestRunning = false;

        private bool sceneLoaded = false;

        [UnitySetUp]
        public IEnumerator Setup()
        {
            if (!sceneLoaded)
            {
                // Load the scene
                SceneManager.LoadScene("ClockApplicationMainScene");

                // Wait for the scene to load
                yield return new WaitForSeconds(1f);

                // Find the StopwatchController in the scene
                stopwatchController = ClockManager.Instance.Stopwatch;

                sceneLoaded = true;
            }
        }

        [Order(1)]
        [UnityTest]
        public IEnumerator StopwatchStartsAndStopsAfter5Seconds()
        {
            firstTestRunning = true;

            // Start the stopwatch
            stopwatchController.StartStopwatch();

            // Wait for 5 seconds
            yield return new WaitForSeconds(5f);

            // Stop the stopwatch
            stopwatchController.StopStopwatch();

            // Wait an extra second to make sure the stop worked.
            yield return new WaitForSeconds(1f);

            // Check if the stopwatch time is close to 5 seconds
            float expectedTime = 5f;
            float delta = 0.1f;
            Assert.That(stopwatchController.CurrentLapTime, Is.EqualTo(expectedTime).Within(delta));

            firstTestRunning = false;
        }

        [Order(2)]
        [UnityTest]
        public IEnumerator CheckResetStopwatch()
        {
            while (firstTestRunning)
            {
                yield return null;
            }

            // Start the stopwatch
            stopwatchController.StartStopwatch();

            // Wait for 1 seconds
            yield return new WaitForSeconds(1f);

            stopwatchController.ResetStopwatch();
            Assert.That(stopwatchController.CurrentLapTime, Is.EqualTo(0));
        }
    }
}
