using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace ClockApplication.Tests
{
    /// <summary>
    /// The testing script for playtime test on the timer
    /// </summary>
    public class TimerTests
    {
        /// <summary>
        /// The timer controller to test.
        /// </summary>
        private ITimerController timerController;

        /// <summary>
        /// Bool to store if the scene has already been loaded so it does not need to be loaded again.
        /// </summary>
        private bool sceneLoaded = false;

        /// <summary>
        /// Loads the scene and gets the timer reference.
        /// </summary>
        /// <returns></returns>
        [UnitySetUp]
        public IEnumerator Setup()
        {

            if (!sceneLoaded)
            {
                for (int i = 0; i < SceneManager.sceneCount; i++)
                {
                    Scene scene = SceneManager.GetSceneAt(i);

                    if (scene.name == "ClockApplicationMainScene")
                    {
                        sceneLoaded = true;
                    }
                }

                if (!sceneLoaded)
                {
                    // Load the scene
                    SceneManager.LoadScene("ClockApplicationMainScene", LoadSceneMode.Single);
                    sceneLoaded = true;
                }
                // Wait for the scene to load
                yield return new WaitForSeconds(1f);

                // Find the TImerController in the scene
                timerController = ClockManager.Instance.TimerForClock;
            }

            yield return null;
        }

        /// <summary>
        /// Checks if the error handling for invalid seconds is working.
        /// </summary>
        [Order(1)]
        [UnityTest]
        public IEnumerator TestInvalidSecondsInput()
        {
            timerController.InputTimerTime(0, 0, 90);

            Assert.IsFalse(timerController.GetAndValidateTimerInput());

            yield return null;
        }

        /// <summary>
        /// Checks if the error handling for invalid minutes is working.
        /// </summary>
        [Order(2)]
        [UnityTest]
        public IEnumerator TestInvalidMinutesInput()
        {
            timerController.InputTimerTime(0, 90, 0);

            Assert.IsFalse(timerController.GetAndValidateTimerInput());

            yield return null;
        }

        /// <summary>
        /// Checks if the error handling for invalid hours is working.
        /// </summary>
        [Order(3)]
        [UnityTest]
        public IEnumerator TestInvalidHoursInput()
        {
            timerController.InputTimerTime(100000000, 0, 0);

            Assert.IsFalse(timerController.GetAndValidateTimerInput());

            yield return null;
        }

        /// <summary>
        /// Tests that a valid input into the timer works.
        /// </summary>
        [Order(4)]
        [UnityTest]
        public IEnumerator TestValidInput()
        {
            timerController.InputTimerTime(1, 1, 1);

            Assert.IsTrue(timerController.GetAndValidateTimerInput());

            yield return null;
        }

        /// <summary>
        /// Test if running the timer works.
        /// First assert checks if after a second of running a 5 second timer if the remaining time is right.
        /// Second assert checks the reamining time after pausing for and resuming.
        /// Third assert checks that the timer is done after the timer is resumed and enough time has elasped.
        /// </summary>
        [Order(5)]
        [UnityTest]
        public IEnumerator Test5SecondTimer()
        {
            yield return new WaitForSeconds(5f);

            timerController.InputTimerTime(0, 0, 5);

            timerController.StartTimer();

            yield return new WaitForSeconds(1f);

            int delta = 1;

            Assert.That(timerController.TotalSecondsRemaining, Is.EqualTo(4).Within(delta));

            timerController.PauseTimer();

            yield return new WaitForSeconds(3f);

            Assert.That(timerController.TotalSecondsRemaining, Is.EqualTo(4).Within(delta));

            timerController.StartTimer();

            yield return new WaitForSeconds(4f);

            Assert.That(timerController.TotalSecondsRemaining, Is.EqualTo(0).Within(delta));
        }
    }
}
