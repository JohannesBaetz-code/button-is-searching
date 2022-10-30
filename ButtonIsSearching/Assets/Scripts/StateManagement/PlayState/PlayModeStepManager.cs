using System;
using System.Collections.Generic;
using FlagCollection;
using GraphCollection.GraphComponents;
using MapDrawCollection;
using MothCollection;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace StateManagement.PlayState
{
    /// <summary>
    /// Converts the Data from ListManager(Output of SearchAlgorithms) to visible Map Objects.
    /// </summary>
    /// <author> Jannick Mitsch </author>
    /// <date>07.01.2022</date>
    public class PlayModeStepManager
    {
        private PlayStateManager _playStateManager;
        private PlayModeMapDrawer _playModeMapDrawer;
        private BuildingCreator bc;
        private Vertex visitedIterator;
        public Vertex VisitedIterator => visitedIterator;
        private Vertex pathIterator;
        private LinkedList<GameObject> weightSigns;
        private LinkedList<GameObject> heuristicSigns;
        public bool ShowTextFields;

        public PlayModeStepManager(PlayStateManager playStateManager)
        {
            _playStateManager = playStateManager;
            _playModeMapDrawer = new PlayModeMapDrawer();
            bc = BuildingCreator.GetInstance();
            visitedIterator = null;
            weightSigns = new LinkedList<GameObject>();
            heuristicSigns = new LinkedList<GameObject>();
            ShowTextFields = true;
        }

        public void PrepareMapBeforePlayModeStart()
        {
            visitedIterator = _playStateManager.ListManager.NextStepVisitedVertex();
            SetPlayModeTextFields();

            if (_playStateManager.Algorithm == Algorithm.Dijkstra || _playStateManager.Algorithm == Algorithm.AStar)
            {
                DrawNextPosition();
                DrawPreviousPosition();
                DrawPreviousPosition();
            }

            DesaturateAllTiles();
            SaturateCurrentVisibleTiles();
        }

        public void ResetDrawings()
        {
            DeletePlayModeTextFields();
            if (FinishSetter.GetInstance().IsFinishSetted)
            {
                FinishSetter.GetInstance().SetNewFlagTo(FinishSetter.GetInstance().GetFinishPos());
            }
            ClearAllPlayModeDrawings();
            //Wichtig erst danach aufrufen:
            if(MothSetter.GetInstance().IsMothSetted()){
                MothSetter.GetInstance().SetNewMothTo(_playStateManager.StartVertex.TileData.HexPos,
                MothSetter.GetInstance().MothAlgorithm);
            }
        }

        public void DeletePlayModeTextFields()
        {
            DestroyallHeuristicSigns();
            DestroyAllWeightSigns();
        }

        public void SetPlayModeTextFields()
        {
            if (_playStateManager.Algorithm == Algorithm.Dijkstra || _playStateManager.Algorithm == Algorithm.AStar)
            {
                RedrawWeightSigns(visitedIterator);
                if (_playStateManager.Algorithm == Algorithm.AStar) SetAllHeuristicSigns();
            }
        }

        public void DesaturateAllTiles()
        {
            foreach (Vector3Int pos in new TilemapDrawer().GetAllPosInDrawArea())
            {
                _playModeMapDrawer.DesaturateAt(pos);
            }
        }

        private void ClearAllPlayModeDrawings()
        {
            _playModeMapDrawer.RemoveAllWayColorChanges();
            _playModeMapDrawer.ClearDesaturation();
            _playModeMapDrawer.ResetAllPins();
        }

        /// <summary>
        /// Marks all ways in MapView that are on the walkPath of the Moth.
        /// </summary>
        private void DrawAllPathWays()
        {
            Vertex lastVertex = null;
            foreach (Vertex vertex in _playStateManager.ListManager.PathVertexList)
            {
                if (lastVertex != null && vertex != null)
                {
                    _playModeMapDrawer.SetPathWayBetween(lastVertex.TileData.HexPos, vertex.TileData.HexPos);
                }

                lastVertex = vertex;
            }
        }

        /// <summary>
        /// Instantiate at the position of all vertecies/pins a textfield and sets the heuristic of AStar-Algorithm to it.
        /// </summary>
        public void SetAllHeuristicSigns()
        {
            DestroyallHeuristicSigns();
            foreach (LinkedList<Edge> edgeList in _playStateManager.Graph.Adj)
            {
                foreach (Edge edge in edgeList)
                {
                    Vector3 worldPos = bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.PIN)
                        .CellToWorld(edge.V.TileData.HexPos);
                    GameObject textField = SetTextFieldAt(worldPos, Sign.HEURISTIC);
                    SetValueToTextField(textField, edge.V.Heuristic);
                    heuristicSigns.AddLast(textField);

                    Vector3 worldPos2 = bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.PIN)
                        .CellToWorld(edge.W.TileData.HexPos);
                    GameObject textField2 = SetTextFieldAt(worldPos2, Sign.HEURISTIC);
                    SetValueToTextField(textField2, edge.W.Heuristic);
                    heuristicSigns.AddLast(textField2);
                }
            }
        }

        public void DestroyallHeuristicSigns()
        {
            foreach (GameObject gameObject in heuristicSigns)
            {
                GameObject.Destroy(gameObject);
            }

            heuristicSigns = new LinkedList<GameObject>();
        }

        /// <summary>
        /// Instantiate at the position/MiddlePoint of all Edges/Paths a textfield and sets the weight to it.
        /// ONLY Dijkstra and AStar-Algorithm.
        /// </summary>
        private void SetWeightSigns(Vertex vertex)
        {
            if (vertex == null) return;
            if (!ShowTextFields) return;
            if (_playStateManager.Algorithm != Algorithm.Dijkstra &&
                _playStateManager.Algorithm != Algorithm.AStar) return;
            foreach (Edge edge in _playStateManager.Graph.Adj[vertex.Position])
            {
                Tilemap tilemapGraph = bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.PIN);
                Vector3 worldPos = (tilemapGraph.CellToWorld(edge.V.TileData.HexPos) +
                                    tilemapGraph.CellToWorld(edge.W.TileData.HexPos)) / 2;

                GameObject textField = SetTextFieldAt(worldPos, Sign.WEIGHT);
                SetValueToTextField(textField, edge.EdgeWeight);
                weightSigns.AddLast(textField);
            }
        }

        /// <summary>
        /// Resets and Instantiates Sign-GameObjects on Edges in GraphView.
        /// </summary>
        /// <param name="vertex">visitedVertexIterator</param>
        public void RedrawWeightSigns(Vertex vertex)
        {
            DestroyAllWeightSigns();
            if (_playStateManager.ListManager.PassedVisitedVerticies.Count <= 0)
            {
                Vertex firstVertex = _playStateManager.ListManager.NextStepVisitedVertex();
                SetWeightSigns(firstVertex);
                _playStateManager.ListManager.PreviousStepVisitedVertex();
                return;
            }
            foreach (Vertex passedVertex in _playStateManager.ListManager.PassedVisitedVerticies)
            {
                SetWeightSigns(passedVertex);
            }

            SetWeightSigns(vertex);
        }

        private void DestroyAllWeightSigns()
        {
            foreach (GameObject gameObject in weightSigns)
            {
                GameObject.Destroy(gameObject);
            }

            weightSigns = new LinkedList<GameObject>();
        }

        private void SetValueToTextField(GameObject textField, int value)
        {
            textField.GetComponentInChildren<TextFieldTextSetter>().SetText(value);
        }

        private GameObject GetSignPrefab(Sign sign)
        {
            if (sign == Sign.WEIGHT)
            {
                return bc.GetWeightSignPrefab();
            }
            else
            {
                return bc.GetHeuristicSignPrefab();
            }
        }

        /// <summary>
        /// Sets TextField Prefabs of WeightSigns and HeuristicSigns.
        /// </summary>
        /// <param name="worldPos"></param>
        /// <param name="sign"></param>
        /// <returns>instantiated Prefab-GameObject</returns>
        private GameObject SetTextFieldAt(Vector3 worldPos, Sign sign)
        {
            GameObject text = GameObject.Instantiate(GetSignPrefab(sign), worldPos, new Quaternion());

            text.transform.SetParent(bc.GetTextFieldCanvasObject().transform, true);

            Camera camera = MouseInputHandler.GetInstance()._camera;
            text.transform.position = camera.WorldToScreenPoint(worldPos);

            return text;
        }

        /// <summary>
        /// With Methods from ListManager is be drawn the Next Step of PlayMode.
        /// </summary>
        public void DrawNextPosition()
        {
            Vertex lastVertex = visitedIterator;
            visitedIterator = _playStateManager.ListManager.NextStepVisitedVertex();

            if (lastVertex == visitedIterator || lastVertex == null)
            {
                lastVertex = visitedIterator;
                visitedIterator = _playStateManager.ListManager.NextStepVisitedVertex();
            }

            if (visitedIterator != null)
            {
                _playModeMapDrawer.SaturateAt(visitedIterator.TileData.HexPos);
                _playModeMapDrawer.MoveMothPinTo(visitedIterator.TileData.HexPos);

                SetWeightSigns(visitedIterator);

                if (lastVertex != null)
                {
                    _playModeMapDrawer.SaturateAt(lastVertex.TileData.HexPos);
                    _playModeMapDrawer.SetPinToVisited(lastVertex.TileData.HexPos);
                }

                SaturateCurrentVisibleTiles();
            }
            else
            {
                DrawAllPathWays();
                lastVertex = pathIterator;
                pathIterator = _playStateManager.ListManager.NextStepPathVertex();

                if (lastVertex == pathIterator)
                {
                    lastVertex = pathIterator;
                    pathIterator = _playStateManager.ListManager.NextStepPathVertex();
                }

                if (pathIterator != null)
                {
                    if (lastVertex != null)
                    {
                        MothSetter.GetInstance().MoveMoth(lastVertex.TileData.HexPos, pathIterator.TileData.HexPos);
                    }
                    else
                    {
                        DrawNextPosition();
                    }
                }
            }
        }

        private void SaturateCurrentVisibleTiles()
        {
            foreach (var pos in GetAllPosWithVisitedOrMothOrNeighboursPin())
            {
                _playModeMapDrawer.SaturateAt(pos);
            }
        }

        /// <summary>
        /// With Methods from ListManager is be drawn the Previous Step of PlayMode.
        /// </summary>
        public void DrawPreviousPosition()
        {
            Vertex lastVertex = pathIterator;
            pathIterator = _playStateManager.ListManager.PreviousStepPathVertex();

            if (lastVertex == pathIterator || lastVertex == null)
            {
                lastVertex = pathIterator;
                pathIterator = _playStateManager.ListManager.PreviousStepPathVertex();
            }

            if (pathIterator != null)
            {
                if (lastVertex != null)
                {
                    MothSetter.GetInstance().MoveMoth(lastVertex.TileData.HexPos, pathIterator.TileData.HexPos);
                }
            }
            else
            {
                lastVertex = visitedIterator;
                visitedIterator = _playStateManager.ListManager.PreviousStepVisitedVertex();

                if (visitedIterator != null)
                {
                    if (lastVertex == visitedIterator)
                    {
                        lastVertex = visitedIterator;
                        visitedIterator = _playStateManager.ListManager.PreviousStepVisitedVertex();
                    }

                    RedrawWeightSigns(visitedIterator);


                    _playModeMapDrawer.MoveMothPinTo(visitedIterator.TileData.HexPos);
                    if (lastVertex != null)
                    {
                        _playModeMapDrawer.ResetPinAt(lastVertex.TileData.HexPos);
                    }


                    foreach (var pos in _playModeMapDrawer.GetAllPosInDrawArea())
                    {
                        _playModeMapDrawer.DesaturateAt(pos);
                    }

                    SaturateCurrentVisibleTiles();
                }
            }
        }

        private bool IsKindOfVisitedPinAt(Vector3Int pos)
        {
            Tilemap pinMap = bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.PIN);
            TileBase normalPin = bc.GetTileBase(Window.GRAPH_WINDOW, TileBaseType.PIN_NORMAL);
            TilemapDrawer tmd = new TilemapDrawer();
            if (!tmd.IsWayAt(pos) || pinMap.GetTile(pos) == normalPin) return false;
            return true;
        }

        private bool IsWayToNormalPinAt(Vector3Int pos, WayDirection direction)
        {
            TilemapDrawer tmd = new TilemapDrawer();
            if (!tmd.IsWayAt(pos) || direction == WayDirection.NONE) return false;
            Tilemap pinMap = bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.PIN);
            TileBase normalPin = bc.GetTileBase(Window.GRAPH_WINDOW, TileBaseType.PIN_NORMAL);
            Vector3Int pos2 = tmd.CalculateNextPosByDirection(pos, direction);
            if (tmd.IsWayAt(pos2) &&
                pinMap.GetTile(pos2) == normalPin &&
                tmd.IsWayInDirection(pos, direction)) return true;
            return false;
        }

        /// <summary>
        /// Returns all Positions where a normal/red Pin is.
        /// Some Position are multiple times added.
        /// </summary>
        /// <returns></returns>
        private Vector3Int[] GetAllPosWithVisitedOrMothOrNeighboursPin()
        {
            TilemapDrawer tmd = new TilemapDrawer();
            Vector3Int[] allPosFromDrawArea = tmd.GetAllPosInDrawArea();
            Vector3Int[] array = new Vector3Int[allPosFromDrawArea.Length];
            int index = 0;

            foreach (Vector3Int pos in allPosFromDrawArea)
            {
                if (IsKindOfVisitedPinAt(pos))
                {
                    array[index] = pos;
                    index++;
                    //Tile has Connection to Neighbour-Tile:
                    if (IsWayToNormalPinAt(pos, WayDirection.RIGHT_TOP))
                    {
                        array[index] = tmd.CalculateNextPosByDirection(pos, WayDirection.RIGHT_TOP);
                        index++;
                    }

                    if (IsWayToNormalPinAt(pos, WayDirection.RIGHT))
                    {
                        array[index] = tmd.CalculateNextPosByDirection(pos, WayDirection.RIGHT);
                        index++;
                    }

                    if (IsWayToNormalPinAt(pos, WayDirection.RIGHT_BOTTOM))
                    {
                        array[index] = tmd.CalculateNextPosByDirection(pos, WayDirection.RIGHT_BOTTOM);
                        index++;
                    }

                    if (IsWayToNormalPinAt(pos, WayDirection.LEFT_BOTTOM))
                    {
                        array[index] = tmd.CalculateNextPosByDirection(pos, WayDirection.LEFT_BOTTOM);
                        index++;
                    }

                    if (IsWayToNormalPinAt(pos, WayDirection.LEFT))
                    {
                        array[index] = tmd.CalculateNextPosByDirection(pos, WayDirection.LEFT);
                        index++;
                    }

                    if (IsWayToNormalPinAt(pos, WayDirection.LEFT_TOP))
                    {
                        array[index] = tmd.CalculateNextPosByDirection(pos, WayDirection.LEFT_TOP);
                        index++;
                    }
                }
            }

            Vector3Int[] output = new Vector3Int[index];
            for (int i = 0; i < index; i++)
            {
                output[i] = array[i];
            }

            return output;
        }

        private enum Sign
        {
            WEIGHT,
            HEURISTIC
        }
    }
}