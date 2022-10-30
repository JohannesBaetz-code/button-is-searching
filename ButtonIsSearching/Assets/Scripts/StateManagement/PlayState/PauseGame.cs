using Button;
using GraphCollection.GraphComponents;
using MapDrawCollection;
using UnityEngine;

namespace StateManagement.PlayState
{
    /// <summary>
    /// Script which is called, Pause-GameState should be active.
    /// </summary>
    /// <author> Johannes Baetz, Jannick Mitsch</author>
    /// <date>07.01.2022</date>
    public class PauseGame : IPlayState
    {
        private PlayStateManager _playStateManager;

        public PauseGame(PlayStateManager playStateManager)
        {
            _playStateManager = playStateManager;
        }
        
        public IPlayState DoState(PlayStateManager playStateManager)
        {
            return playStateManager.Pause;
        }

        public void OnClickNextStepButton()
        {
            _playStateManager.PlayModeStepManager.DrawNextPosition();
        }

        public void OnClickPreviousStepButton()
        {
            _playStateManager.PlayModeStepManager.DrawPreviousPosition();
        }

    }
}