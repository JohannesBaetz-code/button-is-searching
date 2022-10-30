using System;
using UnityEngine;
using Button;
using FlagCollection;
using GraphCollection;
using GraphCollection.GraphComponents;
using GraphCollection.GraphGeneration;
using GraphCollection.SearchAlgorithm;
using MapDrawCollection;
using MothCollection;
using StateManagement.BuildState;
using StateManagement.PauseMenuState;
using StateManagement.PlayState;
using TMPro;
using UIHandlerCollection;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace StateManagement
{
    /// <summary> Handles the Build and Play states. </summary>
    /// <author> Johannes Bätz, Jannick Mitsch, Fanny Weidner </author>
    public class StateManager : Singleton<StateManager>
    {
        public GameState _state { get; set; }
        private GameState _lastGameState;
        public PlayStateManager _playStateManager { get; set; }
        public BuildStateManager _buildStateManager { get; set; }
        private PauseMenuStateManager _pauseMenuStateManager;
        private BuildInformation _buildInformation;
        [SerializeField] private GameObject switchStateButtonFromBuildMode;
        [SerializeField] private GameObject switchStateButtonFromPlayMode;
        [SerializeField] private Texture2D[] _cursorTexturesBuildMode;
        [SerializeField] private GameObject _mainMenuUI;
        [SerializeField] private GameObject _pauseMenuUI;
        [SerializeField] private GameObject _playModeUI;
        [SerializeField] private GameObject _buildModeUI;
        [SerializeField] private GameObject _loadingUI;
        [SerializeField] private GameObject musicPlayer;
        [SerializeField] private AudioClip buildModeMusic;
        [SerializeField] private AudioClip playModeMusic;


        private ScreenSizeChangeHandler _screenSizeChangeHandler;

        /// <summary> Initializes all important components when Game starts. </summary>
        private void Awake()
        {
            Debug.Log("init StateManager");
            _state = GameState.Menu;
            _playStateManager = new PlayStateManager();
            _buildStateManager = new BuildStateManager(_cursorTexturesBuildMode);
            _pauseMenuStateManager = new PauseMenuStateManager();
            switchStateButtonFromBuildMode.GetComponent<ButtonSwitcher>().SwitchState += changeState;
            switchStateButtonFromPlayMode.GetComponent<ButtonSwitcher>().SwitchState += changeState;
            // menuButtonFromBuildMode.GetComponent<ButtonSwitcher>().MenuState += ChangePauseMenu;
            // menuButtonFromPlayMode.GetComponent<ButtonSwitcher>().MenuState += ChangePauseMenu;
            // menuButtonFromPauseMode.GetComponent<ButtonSwitcher>().MenuState += ChangePauseMenu;
            _screenSizeChangeHandler = new ScreenSizeChangeHandler();

            try
            {
                Cursor.SetCursor(_cursorTexturesBuildMode[0], Vector2.zero, CursorMode.Auto);
            }
            catch (Exception e)
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
        }

        /// <summary> Unity update function, which handles the gamestates. </summary>
        private void Update()
        {
            switch (_state)
            {
                case GameState.Build:
                    Debug.Log("Buildmode");
                    _buildStateManager.Update();
                    break;
                case GameState.Play:
                    Debug.Log("Playmode");
                    _playStateManager.Update();
                    break;
                case GameState.Menu:
                    Debug.Log("Spiel wurde pausiert!");
                    _pauseMenuStateManager.Update();
                    break;
            }

            _screenSizeChangeHandler.Update();
        }

        /// <summary> Switches the state between BuildMode and PlayMode. </summary>
        public void changeState()
        {
            try
            {
                Cursor.SetCursor(_cursorTexturesBuildMode[0], Vector2.zero, CursorMode.Auto);
            }
            catch (Exception e)
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }

            _buildInformation = new BuildInformation();

            if (_state == GameState.Play)
            {
                ChangeStateToBuildMode();
            }
            else
            {
                ChangeStateToPlaymode();
            }
        }

        public void ChangeStateToBuildMode()
        {
            CloseAllMenus();
            _state = GameState.Build;
            disablePlayComponents();
            enableBuildComponents();
        }

        public void ChangeStateToPlaymode()
        {
            CloseAllMenus();
            _state = GameState.Play;
            if (IsMapCompleted())
            {
                disableBuildComponents();
                enablePlayComponents();
            }
            else
            {
                Debug.LogError("Map nicht vollständig gezeichnet!");
                _state = GameState.Build;
                BuildingCreator.GetInstance()
                    .ShowAlertMessage(
                        "Du musst ein Start und ein Ziel setzen!\nStart und Ziel brauchen eine Verbindung durch einen Weg!");
            }
        }

        public void OpenMainMenu()
        {
            if (_state != GameState.Build)
            {
                ChangeStateToBuildMode();
            }

            OpenMenu();
            _mainMenuUI.SetActive(true);
        }

        public void OpenMenu()
        {
            if (_state != GameState.Menu)
            {
                _lastGameState = _state;
            }

            _state = GameState.Menu;
            enablePauseMenuComponents();
        }

        public void CloseAllMenus()
        {
            _state = _lastGameState;
            disableMenuComponents();
        }

        /// <summary> Disables all needed components for PauseMenu. </summary>
        private void disableMenuComponents()
        {
            Debug.Log("disable the Menu");
            _pauseMenuUI.SetActive(false);
            _mainMenuUI.SetActive(false);
            _loadingUI.SetActive(false);
        }

        /// <summary> Checks if the graph and all needed information for playmode are given. </summary>
        /// <returns> True if all information is valid and false if not. </returns>
        private bool IsMapCompleted()
        {
            if (!MothSetter.GetInstance().IsMothSetted() || !FinishSetter.GetInstance().IsFinishSetted) return false;

            SearchManager sm = new SearchManager(_buildInformation.Graph,
                _buildInformation.StartVertex, _buildInformation.EndVertex, Algorithm.DepthFirstSearch);

            return sm.Validate();
        }

        /// <summary> Enables all needed components for BuildMode. </summary>
        private void enableBuildComponents()
        {
            _buildModeUI.SetActive(true);
            _buildStateManager.BuildInformation = _buildInformation;
            musicPlayer.GetComponent<AudioSource>().clip = buildModeMusic;
            musicPlayer.GetComponent<AudioSource>().Play();
            _buildStateManager.OnEnable();
        }

        /// <summary> Disables all needed components for BuildMode. </summary>
        private void disableBuildComponents()
        {
            _buildModeUI.SetActive(false);
            BuildingCreator.GetInstance().BuildingButtonHandler.UnsetDrawMode();
        }

        /// <summary> Enables all needed components for PlayMode. </summary>
        private void enablePlayComponents()
        {
            _playModeUI.SetActive(true);
            _playStateManager.Graph = _buildInformation.Graph;
            _playStateManager.Algorithm = _buildInformation.Algorithm;
            _playStateManager.StartVertex = _buildInformation.StartVertex;
            _playStateManager.EndVertex = _buildInformation.EndVertex;
            _playStateManager.OnEnable();
            musicPlayer.GetComponent<AudioSource>().clip = playModeMusic;
            musicPlayer.GetComponent<AudioSource>().Play();
        }

        /// <summary> Disables all needed components for PlayMode. </summary>
        private void disablePlayComponents()
        {
            _playModeUI.SetActive(false);
            _playStateManager.DisablePlayComponents();
        }

        /// <summary> Enables all needed components for PauseMenu. </summary>
        private void enablePauseMenuComponents()
        {
            Debug.Log("enable the Menu");
            _pauseMenuUI.SetActive(true);
        }
    }

    /// <summary> the states that are existing </summary>
    public enum GameState
    {
        Build,
        Play,
        Menu
    }
}