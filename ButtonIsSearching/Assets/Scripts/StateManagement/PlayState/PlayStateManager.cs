using System;
using System.Collections.Generic;
using System.IO;
using GraphCollection;
using GraphCollection.GraphComponents;
using GraphCollection.SearchAlgorithm;
using MapDrawCollection;
using MothCollection;
using TMPro;
using UnityEngine;

namespace StateManagement.PlayState
{
    /// <summary> Manages the playstates in Playmode. </summary>
    /// <author> Johannes Bätz, Jannick Mitsch </author>
    /// Last Change: 21.12.2021
    public class PlayStateManager
    {
        private IPlayState _playState;
        public SpeedState Speed { get; set; }
        public PauseGame Pause { get; set; }
        public CardQuiz QuizManager { get; set; }
        public Graph Graph { get; set; }
        public Algorithm Algorithm { get; set; }
        public Vertex StartVertex { get; set; }
        public Vertex EndVertex { get; set; }
        public SearchManager SearchManager { get; set; }
        public ListManager ListManager { get; set; }
        public PlayModeStepManager PlayModeStepManager { get; set; }
        public LinkedList<GameObject> WeightSigns { get; set; }
        public LinkedList<GameObject> HeuristicSigns { get; set; }

        /// <summary> Constructor. </summary>
        public PlayStateManager()
        {
            Speed = new SpeedState(this);
            Pause = new PauseGame(this);
            PlayModeStepManager = new PlayModeStepManager(this);
            QuizManager = CardQuiz.GetInstance();
            _playState = Speed;
        }

        /// <summary> All calls that are neccessary, while the Game is in PlayMode. </summary>
        public void Update()
        {
            _playState = _playState.DoState(this);
            new PlayButtonImageUpdater(this).UpdatePlaymodeButtonImages();
        }

        /// <summary> Reacts to changes from the ScreenSize. </summary>
        public void OnScreenSizeChange()
        {
            PlayModeStepManager.SetPlayModeTextFields();
        }
        
        public void PrepareMapForPlayMode()
        {
            PlayModeStepManager.PrepareMapBeforePlayModeStart();
        }

        public void OnDisable()
        {
            try
            {
                MothSetter.GetInstance()
                    .SetNewMothTo(StartVertex.TileData.HexPos, MothSetter.GetInstance().MothAlgorithm);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void OnEnable()
        {
            SetUpSearchManager();
            PrepareMapForPlayMode();
            QuizManager = CardQuiz.GetInstance();
            QuizManager.ResetQuiz();
            QuizManager.currentQuestionaire();
        }

        /// <summary> Initializes the SearchManager and hands him all needed Information.
        /// If the SearchManager is initialized, it is handed all new Information. </summary>
        public void SetUpSearchManager()
        {
            if (SearchManager == null)
            {
                Debug.Log("created SearchManager");
                SearchManager = new SearchManager(Graph, Algorithm);
                SearchManager.Start = StartVertex;
                SearchManager.End = EndVertex;
            }
            else
            {
                SearchManager.Graph = Graph;
                SearchManager.CurrentAlgorithm = Algorithm;
                SearchManager.Start = StartVertex;
                SearchManager.End = EndVertex;
            }
            SearchManager.StartSearch();
            SetListManager();
        }

        /// <summary> Initializes the ListManager. </summary>
        public void SetListManager()
        {
            ListManager = new ListManager(SearchManager.VisitedEdgeList, SearchManager.VisistedVertexList,
                SearchManager.PathVertexList);
        }

        /// <summary> Changes the state to pause. </summary>
        public void OnClickPlayPause()
        {
            if (_playState == Pause)
            {
                _playState = Speed;
            }
            else
            {
                _playState = Pause;
            }
        }

        /// <summary> the ui goes one step further. </summary>
        public void OnClickNextStep()
        {
            _playState = Pause;
            Pause.OnClickNextStepButton();
        }

        /// <summary> the ui goes one step back. </summary>
        public void OnClickPreviousStep()
        {
            _playState = Pause;
            Pause.OnClickPreviousStepButton();
        }

        /// <summary> Changes the speed of the speedmode. </summary>
        public void OnClickSpeedMode()
        {
            switch (Speed.CurrentSpeed)
            {
                case SpeedState.SpeedSlow:
                    Speed.CurrentSpeed = SpeedState.SpeedNormal;
                    break;
                case SpeedState.SpeedNormal:
                    Speed.CurrentSpeed = SpeedState.SpeedFast;
                    break;
                case SpeedState.SpeedFast:
                    Speed.CurrentSpeed = SpeedState.SpeedSlow;
                    break;
            }
        }

        public void OnClickTextFieldShowSwitcher()
        {
            PlayModeStepManager.ShowTextFields = !PlayModeStepManager.ShowTextFields;
            if (PlayModeStepManager.ShowTextFields)
            {
                PlayModeStepManager.SetPlayModeTextFields();
            }
            else
            {
                PlayModeStepManager.DeletePlayModeTextFields();
            }
        }

        public void OnClickQuizButton()
        {
            new PlayButtonImageUpdater(this).UpdateQuizButtonImages();
        }

        /// <summary> Resets all drawn things while playmode was active. </summary>
        public void DisablePlayComponents()
        {
            QuizManager = CardQuiz.GetInstance();
            if (QuizManager != null)
            {
                QuizManager.ClearTextBox();
            }
            PlayModeStepManager.ResetDrawings();
        }

        public IPlayState PlayState
        {
            get => _playState;
            set => _playState = value;
        }
    }
}