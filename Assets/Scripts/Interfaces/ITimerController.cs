using UnityEngine.UI;
using TMPro;

namespace ClockApplication
{
    public interface ITimerController
    {
        void StopTimer(bool isFinished);
        void ClearInputFields();
        bool GetAndValidateTimerInput();
    }
}