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
        /// <summary>
        /// Checks to see if the text returned for the clock is valid for a 24 hour clock that shows the 
        /// hours and minutes.
        /// </summary>
        [Test]
        public void CheckTextValidFor24HourClock()
        {
            GameObject clockTestObject = new GameObject();

            ClockController clockController = clockTestObject.AddComponent<ClockController>();

            string timeString = clockController.GetCurrentTimeText();

            string pattern = @"^([01]?[0-9]|2[0-3]):[0-5][0-9]$";
            bool isMatch = Regex.IsMatch(timeString, pattern);

            Assert.IsTrue(isMatch, "The time string '{0}' is not a valid 24-hour time.", timeString);

            Object.DestroyImmediate(clockTestObject);

        }
    }
}
