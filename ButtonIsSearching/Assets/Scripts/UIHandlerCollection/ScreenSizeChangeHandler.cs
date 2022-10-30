using StateManagement;
using UnityEngine;

namespace UIHandlerCollection
{
    
    /// <summary>
    /// Checker for screen resolution changes.
    /// </summary>
    /// <author> Jannick Mitsch </author>
    /// <date>07.01.2022</date>
    public class ScreenSizeChangeHandler
    {

        private Resolution lastResolution;

        public ScreenSizeChangeHandler()
        {
            lastResolution = Screen.currentResolution;
        }

        public void Update()
        {
            if (!lastResolution.Equals(Screen.currentResolution))
            {
                if (StateManager.GetInstance()._state == GameState.Play)
                {
                    StateManager.GetInstance()._playStateManager.OnScreenSizeChange();
                    lastResolution = Screen.currentResolution;
                }
                
            }
        }

    }
}