using System;
using System.Collections;
using System.Collections.Generic;
using MapDrawCollection;
using MothCollection;
using StateManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Button
{
    /// <summary>
    /// Updates TutorialOpenObject and OnClick handles TutorialWindowOpening.
    /// </summary>
    /// <author> Raphael Mueller, Jannick Mitsch </author>
    /// <date>07.01.2022</date>
    public class ButtonTutorial : MonoBehaviour
    {
        [SerializeField] private Transform spawnPos;
        [SerializeField] private GameObject tutorialWindowAStarPrefab;
        [SerializeField] private GameObject tutorialWindowBreadthPrefab;
        [SerializeField] private GameObject tutorialWindowDepthPrefab;
        [SerializeField] private GameObject tutorialWindowDijkstraPrefab;
        [SerializeField] private GameObject daddy;
        [SerializeField] private GameObject textField;

        private Algorithm shownAlgorithm;

        public void Update()
        {
            //Updates TextField and Button for TutorialOpening:
            
            shownAlgorithm = BuildingCreator.GetInstance().SelectedAlgorithm;
            if (shownAlgorithm == Algorithm.None)
            {
                shownAlgorithm = MothSetter.GetInstance().MothAlgorithm;
            }

            string text;
            this.GetComponent<UnityEngine.UI.Button>().interactable = true;
            switch (shownAlgorithm)
            {
                case Algorithm.DepthFirstSearch:
                    text = "Tiefensuche";
                    break;
                case Algorithm.BreadthFirstSearch:
                    text = "Breitensuche";
                    break;
                case Algorithm.Dijkstra:
                    text = "Dijkstra";
                    break;
                case Algorithm.AStar:
                    text = "A*-Algorithmus";
                    break;
                default:
                    text = "Motte auswaehlen!";
                    this.GetComponent<UnityEngine.UI.Button>().interactable = false;
                    break;
            }
            textField.GetComponent<TextMeshProUGUI>().SetText(text);
        }

        public void OpenTutorialWindow()
        {
            GameObject tutorialPrefab;
            switch (shownAlgorithm)
            {
                case Algorithm.DepthFirstSearch:
                    tutorialPrefab = tutorialWindowDepthPrefab;
                    break;
                case Algorithm.BreadthFirstSearch:
                    tutorialPrefab = tutorialWindowBreadthPrefab;
                    break;
                case Algorithm.Dijkstra:
                    tutorialPrefab = tutorialWindowDijkstraPrefab;
                    break;
                case Algorithm.AStar:
                    tutorialPrefab = tutorialWindowAStarPrefab;
                    break;
                default:
                    tutorialPrefab = tutorialWindowDepthPrefab;
                    break;
            }

            GameObject tutorial = Instantiate(tutorialPrefab,
                new Vector3(spawnPos.position.x + 10, spawnPos.position.y + 80, spawnPos.position.z),
                spawnPos.rotation) as GameObject;
            tutorial.transform.SetParent(daddy.transform);
            tutorial.GetComponent<RectTransform>().localScale = new Vector3(0.74f, 0.76f, 0.74f);
        }
    }
}