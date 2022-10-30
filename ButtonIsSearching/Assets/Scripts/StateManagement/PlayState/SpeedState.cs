using UnityEngine;
using GraphCollection.SearchAlgorithm;

namespace StateManagement.PlayState
{
    /// <summary>
    /// Script which is called, Speed-GameState should be active.
    /// </summary>
    /// <author> Jannick Mitsch </author>
    /// <date>07.01.2022</date>
    public class SpeedState : IPlayState
    {
        //time in sek between current and nextStep
        public const float SpeedSlow  = 2f;
        public const float SpeedNormal = 1f;
        public const float SpeedFast = 0.5f;
        public float CurrentSpeed { get; set; }
        private float lastSystemTime;
        private PlayStateManager _playStateManager;

        public SpeedState(PlayStateManager playStateManager)
        {
            CurrentSpeed = SpeedNormal;
            _playStateManager = playStateManager;
        }
        
        public IPlayState DoState(PlayStateManager playStateManager)
        {
            if (IsWaitTimeOver())
            {
                lastSystemTime = Time.time;
                _playStateManager.PlayModeStepManager.DrawNextPosition();
            }
            return playStateManager.Speed;
        }
        
        private bool IsWaitTimeOver()
        {
            if (Time.time - lastSystemTime > CurrentSpeed)
            {
                return true;
            }
            return false;
        }
        
    }
}