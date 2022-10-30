using UnityEngine;
using System;
using StateManagement;
using UnityEngine.SceneManagement;

namespace Button
{
    /// <summary> Script for a Button to change between Build and PlayMode </summary>
    public class ButtonSwitcher : MonoBehaviour
    {
        /// <summary> reference to where the StateManager can subscribe to. </summary>
        public event Action SwitchState;

        // public event Action MenuState;
        // [SerializeField] private GameObject Menu;
        // [SerializeField] private GameObject PauseMenu;
        // [SerializeField] private GameObject Build;
        // [SerializeField] private GameObject LoadListUI;
        
        /// <summary> Is called when the button is pressed. </summary>
        public void ChangeMode() => SwitchState();
        
        public void OnClickMenuButton()
        { 
            StateManager.GetInstance().OpenMenu();
        }

        public void goToBuildmode()
        {
            // Menu.SetActive(false);
            // ClosePauseMenu();
            // Build.SetActive(true);
            // // GameObject _loadListUI = GameObject.Find("LoadScreen");
            // LoadListUI.SetActive(false);
            StateManager.GetInstance().ChangeStateToBuildMode();
        }

        public void ClosePauseMenu()
        {
            StateManager.GetInstance().CloseAllMenus();
        }

        public void goToMainMenu()
        {
            StateManager.GetInstance().OpenMainMenu();
        }

        public void EndGame()
        {
            Application.Quit();
        }
        
    }
    
}