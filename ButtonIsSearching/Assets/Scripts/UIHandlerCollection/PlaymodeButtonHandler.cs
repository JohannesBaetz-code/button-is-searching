using System;
using StateManagement;
using StateManagement.PlayState;
using UnityEngine;
using UnityEngine.UI;

namespace UIHandlerCollection
{
    /// <summary>
    /// Handles most of PlayModeUI Button-OnClick-Methods.
    /// Informs the PlayStateManager when Button is clicked.
    /// </summary>
    /// <author> Jannick Mitsch </author>
    /// <date>07.01.2022</date>
    public class PlaymodeButtonHandler : MonoBehaviour
    {
        private PlayStateManager _playStateManager;
        private ImageReferenceManager irm;

        public void Awake()
        {
            _playStateManager = StateManager.GetInstance()._playStateManager;
        }

        public void OnClickPlayPause()
        {
            _playStateManager.OnClickPlayPause();
            Debug.Log("PlayPause");
        }

        public void OnClickNextStep()
        {
            _playStateManager.OnClickNextStep();
            Debug.Log("NEXT");
        }

        public void OnClickPreviousStep()
        {
            _playStateManager.OnClickPreviousStep();
            Debug.Log("PREVIOUS");
        }

        public void OnClickSpeedMode()
        {
            _playStateManager.OnClickSpeedMode();
            Debug.Log("SPEEEEEED");
        }

        public void OnClickQuizButton()
        {
            _playStateManager.OnClickQuizButton();
            Debug.Log("QUIZZZZZZ");
        }

        public void OnClickWeightSwitcherButton()
        {
            _playStateManager.OnClickTextFieldShowSwitcher();
            Debug.Log("WEIGHTTT SIGGNSS");
        }
        
        
    }
}