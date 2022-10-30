using System.Collections.Generic;
using System.Linq;
using GraphCollection.GraphComponents;
using MapDrawCollection;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GraphCollection.GraphGeneration
{
    /// <summary>
    ///     The GraphComponent Class. It takes hold of all necessary components (Edges, Vertices) that the player
    ///     draws in the BuildMode in order to generate a Graph in the Graph Class.
    /// </summary>
    /// <author>Fanny Weidner, Johannes Bätz</author>
    public class GraphComponent
    {
        /// <summary> Gets every existing Tilemap bound to the Graph Component in the Scene </summary>
        private readonly BuildingCreator _bc = BuildingCreator.GetInstance();

        /// <summary> All necessary, placed Tiles in the groundMap </summary>
        private IEnumerable<TileData.TileData> _allTiles;

        /// <summary> Variables that help through the process </summary>
        private TileData.TileData _currentTile;
        private WayDirection _PATHDIRECTION;
        private int edgeIndex;
        private Vertex foundVertex;
        
        
        /// <param name="GraphComponent Constructor">Creates an object of GraphComponent to generate graph to the
        /// currently active map when required</param>
        /// <author>Johannes Bätz</author>
        public GraphComponent()
        {
            _VertexList = new List<Vertex>();
            _EdgesList = new List<Edge>();

            // Creates new Graph
            checkMap();
        }

        public List<Edge> _EdgesList { get; set; }

        // New graph creation
        public Graph _Graph { get; set; }

        // Intern List of our created Vertices and Edges
        public List<Vertex> _VertexList { get; set; }


        /// <summary>
        ///     This method generates all Tiles that have been placed on a hexagonal map and generates an
        ///     IEnumberable containing all necessary Tilemap AND TileData
        /// </summary>
        /// <param name="IEnumberable TileData">Enumberable Interface containing every Data of a Map's Tiles</param>
        /// <author>Fanny Weidner</author>
        public IEnumerable<TileData.TileData> GetAllTiles(Tilemap tilemap, bool needPath)
        {
            // var bounds = tileMap.cellBounds;
            var mapDrawer = new TilemapDrawer();
            var allTilePos = mapDrawer.GetAllPosInDrawArea();

            foreach (var pos in allTilePos)
                if (!needPath)
                {
                    if (mapDrawer.IsGroundAt(pos))
                    {
                        var tile = tilemap.GetTile(pos);
                        if (tile == null) continue;

                        var tileData = new TileData.TileData(
                            pos.x,
                            pos.y,
                            pos,
                            tilemap,
                            mapDrawer.GetBiomeOfGroundAt(pos)
                        );
                        yield return tileData;
                    }
                }
                else
                {
                    if (mapDrawer.IsWayAt(pos))
                    {
                        var tile = tilemap.GetTile(pos);
                        if (tile == null) continue;

                        var tileData = new TileData.TileData(
                            pos.x,
                            pos.y,
                            pos,
                            tilemap,
                            mapDrawer.GetBiomeOfGroundAt(pos)
                        );
                        yield return tileData;
                    }
                }
        }

        /// <summary> Cycles through all PathMaps to generate possible placed PathTiles </summary>
        /// <param name="CheckMap">Cycles through all PathMaps to check for possible Edges</param>
        /// <author> Fanny Weidner </author>
        private void checkMap()
        {
            //gets all little PinTiles (actually connected ones)

            _allTiles = GetAllTiles(_bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.PIN), true);

            var i = 0;

            foreach (var pinTile in _allTiles)
            {
                _currentTile = pinTile;

                // get Position of the currently checked Tile
                var col = (int) pinTile.ColPos;
                var row = (int) pinTile.RowPos;

                var v = new Vertex(i);
                v.X = col;
                v.Y = row;
                v.TileData = pinTile;
                v.TileType = pinTile.TileType;

                //creates a vertex for every pin and adds it to our vertex list
                _VertexList.Add(v);
                i++;
            }


            // Cycles through all possible Path Directions and checks if the Path is taken in that direction.
            // Generates an Edge and Vertices accordingly. Edge Creation called through Action Event
            if (_VertexList != null)
            {
                // _graph.VertexCount = _vertexList.Count;
                _Graph = new Graph(_VertexList.Count);

                var j = 0;
                var p = 0;

                foreach (var vertex in _VertexList)
                {
                    j++;

                    //current positon of tile that the vertex belongs to
                    var col = (int) vertex.TileData.ColPos;
                    var row = (int) vertex.TileData.RowPos;

                    //helpful variables
                    var hasTile = false;
                    var position = new Vector3Int(col, row, 0);

                    // cycles through all Paths for every vertex and checks if on the vertex position there is a path
                    // If yes -> in what direction?
                    if (new TilemapDrawer().IsWayAt(position))
                    {
                        _currentTile = vertex.TileData;

                        if (_bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_RIGHT_TOP).HasTile(position))
                        {
                            _PATHDIRECTION = WayDirection.RIGHT_TOP;
                            if (findVertexInList(findVertex(vertex)))
                                createEdge(vertex, foundVertex);
                        }

                        if (_bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_RIGHT).HasTile(position))
                        {
                            _PATHDIRECTION = WayDirection.RIGHT;
                            if (findVertexInList(findVertex(vertex)))
                                createEdge(vertex, foundVertex);
                        }

                        if (_bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_RIGHT_BOTTOM).HasTile(position))
                        {
                            _PATHDIRECTION = WayDirection.RIGHT_BOTTOM;
                            if (findVertexInList(findVertex(vertex)))
                                createEdge(vertex, foundVertex);
                        }

                        if (_bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_LEFT_BOTTOM).HasTile(position))
                        {
                            _PATHDIRECTION = WayDirection.LEFT_BOTTOM;
                            if (findVertexInList(findVertex(vertex)))
                                createEdge(vertex, foundVertex);
                        }

                        if (_bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_LEFT).HasTile(position))
                        {
                            _PATHDIRECTION = WayDirection.LEFT;
                            if (findVertexInList(findVertex(vertex)))
                                createEdge(vertex, foundVertex);
                        }

                        if (_bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_LEFT_TOP).HasTile(position))
                        {
                            _PATHDIRECTION = WayDirection.LEFT_TOP;
                            if (findVertexInList(findVertex(vertex)))
                                createEdge(vertex, foundVertex);
                        }
                    }
                    else
                    {
                        Debug.LogError("Fehler in GraphComponent");
                    }
                }
            }
            else
            {
                Debug.Log("VertexList == null!");
            }
        }


        /// <summary> Creates Edge </summary>
        /// <param name="Edge Creation">Creates Edge Component to put into Graph.</param>
        /// <author> Fanny Weidner </author>
        private void createEdge(Vertex oneVertex, Vertex secondVertex)
        {
            if (_PATHDIRECTION == null) return;

            //check if they already exist
            if (_EdgesList.Any(edch => edch.HasVertex(oneVertex) && edch.HasVertex(secondVertex)))
                return;

            var edge = new Edge(oneVertex, secondVertex, edgeIndex);
            edgeIndex++;
            
            _EdgesList.Add(edge);
            _Graph.AddEdge(edge);
        }

        /// <summary> Finds Second Vertices based on the position of the First and the PathDirection on top</summary>
        /// <param name="Find Second Vertex">Creates the Second Vertex necessary for the Edge Creation</param>
        /// <author> Fanny Weidner </author>
        public Vector3Int findVertex(Vertex knownVertex)
        {
            var vertex = new TilemapDrawer().CalculateNextPosByDirection(knownVertex.TileData.HexPos, _PATHDIRECTION);
            return vertex;
        }

        private bool findVertexInList(Vector3Int position)
        {
            var found = false;

            foreach (var vertex in _VertexList)
                if (vertex.IsVertexCoordinatesEqual(position))
                {
                    found = true;
                    foundVertex = vertex;
                    
                    break;
                }

            return found;
        }
    }
}