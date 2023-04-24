using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace ClockApplication.Tests
{
    public class ClockTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void CheckTextValidFor24HourClock()
        {
            ClockController clock = new ClockController();

            string timeString = clock.GetCurrentTimeText();

            string pattern = @"^([01]?[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$";
            bool isMatch = Regex.IsMatch(timeString, pattern);
            Debug.Log("Current time is: " + timeString);

            Assert.IsTrue(isMatch, "The time string '{0}' is not a valid 24-hour time.", timeString);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator CheckClockTextWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
