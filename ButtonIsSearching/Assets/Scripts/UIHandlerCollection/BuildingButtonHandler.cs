using System.Collections;
using System.Collections.Generic;
using FlagCollection;
using GraphCollection;
using GraphCollection.SearchAlgorithm;
using MapDrawCollection;
using MothCollection;
using StateManagement;
using UnityEngine;
using UnityEngine.UI;

namespace UIHandlerCollection
{
    
    /// <summary>
    /// Handles OnClick Methods of the BuildingUI.
    /// Communicates with BuildingCreator for setting DrawMode-variables.
    /// </summary>
    /// <author> Jannick Mitsch </author>
    /// <date>07.01.2022</date>
    public class BuildingButtonHandler : MonoBehaviour
    {
        private BuildingCreator bc;
        private ImageReferenceManager irm;

        private void Awake()
        {
            bc = BuildingCreator.GetInstance();
            irm = ImageReferenceManager.GetInstance();
            bc.BuildingButtonHandler = this;
            bc.SelectedAlgorithm = Algorithm.None;
            bc.SelectedBiome = Biome.NONE;
            bc.SelectedDrawMode = DrawMode.NONE;
        }

        public void TreeDrawingButtonClicked()
        {
            if (bc.SelectedDrawMode == DrawMode.TILE_DRAWER && bc.SelectedBiome == Biome.TREES)
            {
                UnsetDrawMode();
            }
            else
            {
                SetDrawMode(DrawMode.TILE_DRAWER, Biome.TREES);
            }
        }

        public void SnowDrawingButtonClicked()
        {
            if (bc.SelectedDrawMode == DrawMode.TILE_DRAWER && bc.SelectedBiome == Biome.SNOW)
            {
                UnsetDrawMode();
            }
            else
            {
                SetDrawMode(DrawMode.TILE_DRAWER, Biome.SNOW);
            }
        }

        public void IceDrawingButtonClicked()
        {
            if (bc.SelectedDrawMode == DrawMode.TILE_DRAWER && bc.SelectedBiome == Biome.ICE)
            {
                UnsetDrawMode();
            }
            else
            {
                SetDrawMode(DrawMode.TILE_DRAWER, Biome.ICE);
            }
        }

        public void EraserButtonClicked()
        {
            if (bc.SelectedDrawMode == DrawMode.TILE_ERASER || bc.SelectedDrawMode == DrawMode.WAY_ERASER)
            {
                UnsetDrawMode();
            }
            else
            {
                SetDrawMode(DrawMode.TILE_ERASER);
            }
        }

        public void WayEraserButtonClicked()
        {
            if (bc.SelectedDrawMode == DrawMode.WAY_ERASER)
            {
                UnsetDrawMode();
                SetDrawMode(DrawMode.TILE_ERASER);
            }
            else
            {
                SetDrawMode(DrawMode.WAY_ERASER);
            }
        }

        public void WayDrawingButtonClicked()
        {
            if (bc.SelectedDrawMode == DrawMode.WAY_DRAWER)
            {
                UnsetDrawMode();
            }
            else
            {
                SetDrawMode(DrawMode.WAY_DRAWER);
            }
        }

        public void FinishButtonClicked()
        { 
            SetDrawMode(DrawMode.FINISH_SETTER);
            
            if (FinishSetter.GetInstance().IsFinishSetted)
            {
                FinishSetter.GetInstance().UnsetFlag();
            }
        }

        public void BredthSearchClicked()
        {
            SetDrawMode(DrawMode.MOTH_SETTER, Algorithm.BreadthFirstSearch);
            
            if (MothSetter.GetInstance().IsMothSetted() &&
                MothSetter.GetInstance().MothAlgorithm == Algorithm.BreadthFirstSearch)
            {
                MothSetter.GetInstance().UnsetMoth();
            }
        }

        public void DepthSearchClicked()
        {
            SetDrawMode(DrawMode.MOTH_SETTER, Algorithm.DepthFirstSearch);
            
            if (MothSetter.GetInstance().IsMothSetted() &&
                MothSetter.GetInstance().MothAlgorithm == Algorithm.DepthFirstSearch)
            {
                MothSetter.GetInstance().UnsetMoth();
            }
        }

        public void DijkstraSearchClicked()
        {
            SetDrawMode(DrawMode.MOTH_SETTER, Algorithm.Dijkstra);
            
            if (MothSetter.GetInstance().IsMothSetted() && MothSetter.GetInstance().MothAlgorithm == Algorithm.Dijkstra)
            {
                MothSetter.GetInstance().UnsetMoth();
            }
        }

        public void AStarSearchClicked()
        {
            SetDrawMode(DrawMode.MOTH_SETTER, Algorithm.AStar);
            
            if (MothSetter.GetInstance().IsMothSetted() && MothSetter.GetInstance().MothAlgorithm == Algorithm.AStar)
            {
                MothSetter.GetInstance().UnsetMoth();
            }
        }

        public void SetDrawMode(DrawMode mode)
        {
            if (mode == DrawMode.TILE_DRAWER || mode == DrawMode.MOTH_SETTER) return;
            UnsetDrawMode();
            bc.SelectedDrawMode = mode;
        }

        public void SetDrawMode(DrawMode mode, Biome biome)
        {
            if (mode != DrawMode.TILE_DRAWER) return;
            UnsetDrawMode();
            bc.SelectedDrawMode = mode;
            bc.SelectedBiome = biome;
        }

        public void SetDrawMode(DrawMode mode, Algorithm algorithm)
        {
            if (mode != DrawMode.MOTH_SETTER) return;
            UnsetDrawMode();
            bc.SelectedDrawMode = mode;
            bc.SelectedAlgorithm = algorithm;
        }

        public void UnsetDrawMode()
        {
            bc.SelectedDrawMode = DrawMode.NONE;
            bc.SelectedBiome = Biome.NONE;
            bc.SelectedAlgorithm = Algorithm.None;
        }
        
    }
}