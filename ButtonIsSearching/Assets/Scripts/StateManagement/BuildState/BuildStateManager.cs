using System;
using FlagCollection;
using GraphCollection;
using GraphCollection.GraphGeneration;
using MapDrawCollection;
using MothCollection;
using SaveAndLoad;
using UnityEngine;
using UnityEngine.UI;

namespace StateManagement.BuildState
{
    /// <summary>
    /// Manages complete BuildMode-State.
    /// </summary>
    /// <author> Jannick Mitsch, Fanny Weidner, Johannes Baetz </author>
    /// <date>07.01.2022</date>
    public class BuildStateManager
    {
        private BuildingCreator _buildingCreator; //to get reference to ButtonHandler and Tilemap and co
        private MouseInputHandler _mouseInputHandler; //to get currentMousePosition

        private Vector3Int currentGridPos; //mouse Position on Grid
        private Vector3Int lastGridPos; //previous mouse Position on Grid

        private DrawMode _drawMode;
        private Biome _biome;
        private Algorithm _algorithm;

        private Texture2D[] _cursorTextures;

        private SaveSystem _tileMapSaver;
        public BuildInformation BuildInformation { get; set; }

        public BuildStateManager(Texture2D[] cursorTextures)
        {
            _cursorTextures = cursorTextures;
            _buildingCreator = BuildingCreator.GetInstance();
            _mouseInputHandler = MouseInputHandler.GetInstance();
            _mouseInputHandler.OnEnable();
            _drawMode = DrawMode.NONE;
            _biome = Biome.NONE;
            _algorithm = Algorithm.None;
        }

        public void Update()
        {
            //Call first to get current DrawMode and Mousepositions
            UpdateSelectedTool();
            UpdateMouseGridPositions();

            try
            {
                Cursor.SetCursor(SelectCursorByDrawMode(_drawMode, _biome, _algorithm), Vector2.zero, CursorMode.Auto);
            }
            catch (Exception e)
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                Debug.Log("Cursor Setting Fehler");
            }

            PreviewUpdater.GetInstance().UpdatePreviewWhileMouseDrawing(currentGridPos, _drawMode, _biome, _algorithm);
            if (_mouseInputHandler.HoldActive)
            {
                HandleMouseDrawing();
            }
        }

        public void UpdateUI()
        {
            //Sreenresolution changes:
            
        }

        public void OnEnable()
        {
            
        }

        public void OnDisable()
        {
            // _mouseInputHandler.OnDisable();
        }

        private void HandleMouseDrawing()
        {
            WayDrawer wayDrawer = new WayDrawer();
            TilemapEraser eraser = new TilemapEraser();
            switch (_drawMode)
            {
                case DrawMode.TILE_DRAWER:
                    if (wayDrawer.IsGroundAt(currentGridPos))
                    {
                        wayDrawer.ChangeBiomeAt(currentGridPos, _biome);
                    }
                    else
                    {
                        wayDrawer.DrawGround(currentGridPos, _biome);
                    }
                    break;
                case DrawMode.WAY_DRAWER:
                    if (lastGridPos != currentGridPos) wayDrawer.DrawWayBetween(lastGridPos, currentGridPos);
                    break;
                case DrawMode.FINISH_SETTER:
                    FinishSetter.GetInstance().SetNewFlagTo(currentGridPos);
                    break;
                case DrawMode.TILE_ERASER:
                    eraser.Erase(currentGridPos);
                    break;
                case DrawMode.WAY_ERASER:
                    if (lastGridPos != currentGridPos) eraser.EraseWayBetween(lastGridPos, currentGridPos);
                    break;
                case DrawMode.MOTH_SETTER:
                    MothSetter.GetInstance().SetNewMothTo(currentGridPos, _algorithm);
                    break;
                default:
                    Debug.Log("No Drawing Mode selected");
                    break;
            }
        }

        private Texture2D SelectCursorByDrawMode(DrawMode drawMode, Biome biome, Algorithm algorithm)
        {
            switch (drawMode)
            {
                case DrawMode.WAY_DRAWER:
                    return _cursorTextures[4];
                case DrawMode.TILE_DRAWER:
                    switch (biome)
                    {
                        case Biome.TREES:
                            return _cursorTextures[1];
                        case Biome.SNOW:
                            return _cursorTextures[2];
                        case Biome.ICE:
                            return _cursorTextures[3];
                    }
                    break;
                case DrawMode.FINISH_SETTER:
                    return _cursorTextures[5];
                case DrawMode.MOTH_SETTER:
                    switch (algorithm)
                    {
                        case Algorithm.DepthFirstSearch:
                            return _cursorTextures[6];
                        case Algorithm.BreadthFirstSearch:
                            return _cursorTextures[7];
                        case Algorithm.Dijkstra:
                            return _cursorTextures[8];
                        case Algorithm.AStar:
                            return _cursorTextures[9];
                    }
                    break;
                case DrawMode.TILE_ERASER:
                    return _cursorTextures[10];
                case DrawMode.WAY_ERASER:
                    return _cursorTextures[10];
            }
            return _cursorTextures[0];
        }

        private void UpdateSelectedTool()
        {
            _drawMode = _buildingCreator.SelectedDrawMode;
            _biome = _buildingCreator.SelectedBiome;
            _algorithm = _buildingCreator.SelectedAlgorithm;

            new BuildButtonImageUpdater().UpdateBuildButtonImages(_drawMode, _biome, _algorithm);
        }

        private void UpdateMouseGridPositions()
        {
            lastGridPos = currentGridPos;
            if (_mouseInputHandler.WorldPosMouse.x <= 0)
            {
                currentGridPos = _buildingCreator.GetTilemap(Window.MAP_WINDOW, TilemapType.PREVIEW)
                    .WorldToCell(_mouseInputHandler.WorldPosMouse);
            }
            else
            {
                currentGridPos = _buildingCreator.GetTilemap(Window.GRAPH_WINDOW, TilemapType.PREVIEW)
                    .WorldToCell(_mouseInputHandler.WorldPosMouse);
            }
        }
    }
}