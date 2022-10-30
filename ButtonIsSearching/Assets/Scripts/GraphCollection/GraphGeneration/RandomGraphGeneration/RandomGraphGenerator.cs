using System;
using System.Collections.Generic;
using System.Linq;
using GraphCollection.GraphComponents;
using MapDrawCollection;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GraphCollection.GraphGeneration.RandomGraphGeneration
{
    /// <summary>
    ///     The RandomGraphGeneration takes care of all aspects when it comes to creating a PHYSICAL REPRESENTATION
    ///     of a randomly generated graph. (GraphComponent later creates the actual Graph out of it)
    /// </summary>
    /// <author>Fanny Weidner </author>
    public class RandomGraphGenerator : MonoBehaviour
    {
        /// Helpful Vertices
        private Vertex _currentVertex;

        /// Safety Lists for eges and vertices
        private readonly List<Edge> _edgesList = new List<Edge>();

        /// PATHDIRECTION instance
        private WayDirection _PATHDIRECTION;

        private Vertex _startingVertex;
        private readonly List<Vertex> _vertexList = new List<Vertex>();
        private BuildingCreator bc;
        private Vertex foundvertex;

        /// Helpful Variables for paths, vertices + drawing
        private int randomPathDirection;

        private bool vertexIsInList;


        /// <summary>
        ///     "GoButton" is activated upon pressing the current Debug Button for this function. This will receive a proper
        ///     Button within the scene.
        /// </summary>
        /// <param name="Debug Go">Starts all Processes to generate Random Graph</param>
        /// <author> Fanny Weidner </author>
        public void DebugGo()
        {
            //Get a Building Creator to manage the drawing and delete all exisiting Paths and Pins
            bc = BuildingCreator.GetInstance();
            bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.PIN).ClearAllTiles();
            bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_LEFT).ClearAllTiles();
            bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_LEFT_TOP).ClearAllTiles();
            bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_LEFT_BOTTOM).ClearAllTiles();
            bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_RIGHT).ClearAllTiles();
            bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_RIGHT_TOP).ClearAllTiles();
            bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_RIGHT_BOTTOM).ClearAllTiles();

            //Clear the Lists
            _vertexList.Clear();
            _edgesList.Clear();

            //Fill Map with maximum amount of Tiles
            fillMap();

            // Generate a Starting Vertex
            _startingVertex = new Vertex(0);
            generateStartVertex();
            _currentVertex = _startingVertex;

            // Generate the Random Vertices and Edges
            _vertexList.Add(_startingVertex);
            randomVertex();
            randomEdgePath();
        }

        /// <summary>
        ///     As paths can only be drawn on a filled map, and in order to ease the player's experience with the randomGraph
        ///     the complete map is filled with random tiles.
        /// </summary>
        /// <param name="fillMap">The whole Map is filled with random Tiles</param>
        /// <author> Fanny Weidner </author>
        private void fillMap()
        {
            var tilemapDrawer = new TilemapDrawer();
            var allTileCoordinates = tilemapDrawer.GetAllPosInDrawArea();
            int randomMapTileType;

            foreach (var pos in allTileCoordinates)
            {
                randomMapTileType = Random.Range(0, 3);

                if (randomMapTileType == 0) tilemapDrawer.DrawGround(pos, Biome.TREES);

                if (randomMapTileType == 1) tilemapDrawer.DrawGround(pos, Biome.SNOW);

                if (randomMapTileType == 2) tilemapDrawer.DrawGround(pos, Biome.ICE);
            }
        }

        // VERTEX GENERATION -------------------------------------------------------------------------------------------
        /// <summary>
        ///     This generates a quick starting point on the map that is the basis for all other vertex calculations.
        /// </summary>
        /// <param name="Generate Startvertex">The Starting Point of a Random Graph Generation</param>
        /// <author> Fanny Weidner </author>
        private void generateStartVertex()
        {
            // create random x and y in the bounds of the tilemap
            var x = Random.Range(-4, +4);
            var y = Random.Range(-4, +4);

            var startingVertexPos = new Vector3Int(x, y, 0);

            // make sure its in the bounds, if not -> generate a new one
            if (Math.Abs(startingVertexPos.y) <= 4
                && startingVertexPos.x - Math.Abs(startingVertexPos.y) / 2 >= -4
                && startingVertexPos.x + Math.Abs(startingVertexPos.y) / 2 + Math.Abs(startingVertexPos.y % 2) <= 4)
            {
                _startingVertex.X = startingVertexPos.x;
                _startingVertex.Y = startingVertexPos.y;
                _currentVertex = _startingVertex;
            }
            else
            {
                generateStartVertex();
            }
        }


        /// <summary>
        ///     Within a basic range, a certain amount of Vertices is generated. Based on the position of the startvertex,
        ///     the next vertices are generated in a general consecutive line and not completely scattered across the tiles.
        /// </summary>
        /// <param name="Random Vertex">Creates a random amount of vertices with random positions</param>
        /// <author> Fanny Weidner </author>
        private void randomVertex()
        {
            Debug.Log("We can generate vertices");
            // We generate a random amount of Vertices between 10 and 30, taking away the starting Vertex
            var randomVertexCount = Random.Range(10, 51) - 1;

            // generate a 1 and 0 for a random Bool that generates two different kinds of Vertices sets.
            // If Bool false -> Cluster of Vertices, If Bool true -> consecutive lines of vertices

            var randomFun = Random.Range(0, 2);
            bool verticeCluster;

            if (randomFun == 0) verticeCluster = true;
            else verticeCluster = false;

            Debug.Log("RandomVertices: " + randomVertexCount);

            // For the number of Vertices we wants, we generate random positions in consecutive positions on the tilemap
            for (var i = 0; i < randomVertexCount; i++)
            {
                var v = new Vertex(i);
                Debug.Log("CurrentVertex X: " + _currentVertex.X + " currentvertex Y: " + _currentVertex.Y);

                v.X = findVertex(_currentVertex, verticeCluster).X;
                v.Y = findVertex(_currentVertex, verticeCluster).Y;
                var vertexposition = new Vector3Int((int) v.X, (int) v.Y, 0);

                // If the vertex already exists, we generate a different one 
                if (findVertexInList(v))
                {
                    v.X = findVertex(_currentVertex, true).X;
                    v.Y = findVertex(_currentVertex, true).Y;
                }
                else
                {
                    Debug.Log("draw Vertices");
                    // As long as it does not appear doubles, we set a pin on the map and add the vertex to our list
                    new WayDrawer().SetNormalPinAt(vertexposition);
                    _vertexList.Add(v);
                }

                _currentVertex = v;
            }
        }


        /// <summary>
        ///     Methods generate a random number for a path and give it to this method so _PATHDIRECTION switches.
        ///     This is necessary for Vertices generation and random Path Generation.
        /// </summary>
        /// <param name="Generate Random Direction">Picks a Path direction based on a random number given.</param>
        /// <author> Fanny Weidner </author>
        private void generateRandomDirection(int pathNumber)
        {
            switch (pathNumber)
            {
                case 1:
                    _PATHDIRECTION = WayDirection.LEFT_TOP;
                    break;
                case 2:
                    _PATHDIRECTION = WayDirection.LEFT;
                    break;
                case 3:
                    _PATHDIRECTION = WayDirection.LEFT_BOTTOM;
                    break;
                case 4:
                    _PATHDIRECTION = WayDirection.RIGHT_BOTTOM;
                    break;
                case 5:
                    _PATHDIRECTION = WayDirection.RIGHT;
                    break;
                case 6:
                    _PATHDIRECTION = WayDirection.RIGHT_TOP;
                    break;
            }
        }


        /// <summary>
        ///     Calculates a Vertex Position in a certain pathdirection based off of a given Vertex. If we have a path direction
        ///     (if its just been generated), weHavePathDirection is false, if its true, then a new path direction can be generated
        ///     and set
        ///     (in case a direction for a Vertices is not valid)
        /// </summary>
        /// <param name="Find Vertex">Calculate Vertex Position in a certain Path Direction</param>
        /// <author> Fanny Weidner </author>
        public Vertex findVertex(Vertex givenVertex, bool weHavePathDirection)
        {
            foundvertex = new Vertex(0);

            if (!weHavePathDirection)
            {
                randomPathDirection = Random.Range(1, 7);
                generateRandomDirection(randomPathDirection);
            }

            var tilemapDrawer = new TilemapDrawer();

            var vertexPos = new Vector3Int((int) givenVertex.X, (int) givenVertex.Y, 0);
            var direction = tilemapDrawer.CalculateNextPosByDirection(vertexPos, _PATHDIRECTION);

            var pos = new Vector3Int(direction.x, direction.y, 0);

            if (Math.Abs(pos.y) <= 4
                && pos.x - Math.Abs(pos.y) / 2 >= -4
                && pos.x + Math.Abs(pos.y) / 2 + Math.Abs(pos.y % 2) <= 4)
            {
                foundvertex = new Vertex(0);
                foundvertex.X = pos.x;
                foundvertex.Y = pos.y;
            }
            else
            {
                findVertex(_currentVertex, false);
            }

            return foundvertex;
        }


        /// <summary>
        ///     Checks if the Vertex is already in the list as to not create a new Vertex with the same numbers.
        /// </summary>
        /// <param name="Find Vertex in List">Is the given Vertex in the _vertexList</param>
        /// <author> Fanny Weidner </author>
        private bool findVertexInList(Vertex ver)
        {
            var found = false;
            var pos = new Vector3Int((int) ver.X, (int) ver.Y, 0);
            foreach (var vertex in _vertexList)
                if (vertex.IsVertexCoordinatesEqual(pos))
                {
                    found = true;
                    foundvertex = vertex;
                    break;
                }

            return found;
        }


        // EDGE GENERATION ---------------------------------------------------------------------------------------------
        /// <summary>
        ///     Calculates a random number of paths for each Vertex given. Each path is in a random direction with no duplicates
        ///     (no double edges) Based on the PathDirection and the given Vertices, an Edge can be generated
        /// </summary>
        /// <param name="random Edge Path">Creates a random amount of Paths + Edges for each Vertex</param>
        /// <author> Fanny Weidner </author>
        private void randomEdgePath()
        {
            Debug.Log("Create Random Edge");
            int randomPathNumber;
            var randomPaths = new List<int>();
            int randomPathDirection;

            foreach (var v in _vertexList)
            {
                randomPathNumber = Random.Range(1, 7);

                for (var i = 0; i < randomPathNumber; i++)
                {
                    randomPathDirection = Random.Range(1, 7);
                    randomPaths.Add(randomPathDirection);
                    randomPaths = checkForDuplicates(randomPaths);
                }

                foreach (var pathNumber in randomPaths)
                {
                    generateRandomDirection(pathNumber);
                    if (findVertexInList(findVertex(v, true))) createEdge(v, foundvertex);
                }
            }
        }

        /// <summary>
        ///     This makes sure that there are no duplicate Paths and therefore no duplicate Edges in our random generation
        /// </summary>
        /// <param name="Check For Duplicates">Makes sure there are no duplicate numbers in the random generation</param>
        /// <author> Fanny Weidner </author>
        private List<int> checkForDuplicates(List<int> array)
        {
            var hashSet = new HashSet<int>();
            for (var i = 0; i < array.Count; i++) hashSet.Add(array[i]);

            array = hashSet.ToList();

            return array;
        }


        /// <summary> Creates Edge </summary>
        /// <param name="Edge Creation">Creates Edge Component to put into Graph and if valid draws a path on the Tilemap</param>
        /// <author> Fanny Weidner </author>
        private void createEdge(Vertex oneVertex, Vertex secondVertex)
        {
            if (_PATHDIRECTION == null) return;

            //check if they already exist
            if (_edgesList.Any(edch => edch.HasVertex(oneVertex) && edch.HasVertex(secondVertex))) return;

            var edge = new Edge(oneVertex, secondVertex, 1);

            _edgesList.Add(edge);

            var pos1 = new Vector3Int((int) edge.V.X, (int) edge.V.Y, 0);
            var pos2 = new Vector3Int((int) edge.W.X, (int) edge.W.Y, 0);

            new WayDrawer().DrawWayBetween(pos1, pos2);
            Debug.Log("Edge Drawing");
        }
    }
}