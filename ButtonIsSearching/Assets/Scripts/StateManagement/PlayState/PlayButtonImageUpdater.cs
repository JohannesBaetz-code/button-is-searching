using UnityEngine.UI;

namespace StateManagement.PlayState
{
    /// <summary>
    /// Inherits Methods for Update all PlayModeUI Buttons.
    /// </summary>
    /// <author> Jannick Mitsch </author>
    /// <date>07.01.2022</date>
    public class PlayButtonImageUpdater
    {
        private ImageReferenceManager irm;
        private PlayStateManager _playStateManager;

        public PlayButtonImageUpdater(PlayStateManager playStateManager)
        {
            irm  = ImageReferenceManager.GetInstance();
            _playStateManager = playStateManager;
        }

        public void UpdatePlaymodeButtonImages()
        {
            UpdatePlayPauseButtonImage();
            UpdateSpeedButtonImage();
            UpdateTextFieldShowSwitcherButtonImage();
        }

        private void UpdateTextFieldShowSwitcherButtonImage()
        {
            Image image = irm.TextFieldShowSwitcherButton.GetComponent<Image>();
            if (_playStateManager.PlayModeStepManager.ShowTextFields)
            {
                image.sprite = irm.TextFieldShowSwitcherButtonShowSprite;
            }
            else
            {
                image.sprite = irm.TextFieldShowSwitcherButtonUnshowSprite;
            }
        }

        private void UpdatePlayPauseButtonImage()
        {
            Image image = irm.PlayPauseButton.GetComponent<Image>();
            if (_playStateManager.PlayState == _playStateManager.Pause)
            {
                image.sprite = irm.PlayButton;
            }
            else
            {
                image.sprite = irm.PauseButton;
            }
        }

        private void UpdateSpeedButtonImage()
        {
            Image image = irm.SpeedButton.GetComponent<Image>();
            if (_playStateManager.Speed.CurrentSpeed == SpeedState.SpeedSlow)
            {
                image.sprite = irm.SpeedSlowButton;
            }else if (_playStateManager.Speed.CurrentSpeed == SpeedState.SpeedNormal)
            {
                image.sprite = irm.SpeedNormalButton;
            }else if (_playStateManager.Speed.CurrentSpeed == SpeedState.SpeedFast)
            {
                image.sprite = irm.SpeedFastButton;
            }
        }

        public void UpdateQuizButtonImages()
        {
            Image image = irm.FrageButton.GetComponent<Image>();
            if (image.sprite == irm.FragezeichenButton)
            {
                image.sprite = irm.AusrufezeichenButton;
            }
            else
            {
                image.sprite = irm.FragezeichenButton;
            }
        }
        
    }
}