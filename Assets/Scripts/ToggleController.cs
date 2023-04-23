using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class controls functionality internal to the toggle object such as change the text color
/// when the toggle state changes.
/// </summary>
public class ToggleController : MonoBehaviour
{
    [SerializeField]
    private Text ToggleText;

    [SerializeField]
    private Color UnselectedColor;

    [SerializeField]
    private Color SelectedColor;

    /// <summary>
    /// Changes the toggle text color based on the toggle state passed in.
    /// </summary>
    /// <param name="toggleState">The current state of the toggle</param>
    public void ToggleTextColor(bool toggleState)
    {
        ToggleText.color = toggleState ? SelectedColor : UnselectedColor;
    }
}
