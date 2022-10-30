using System.Collections.Generic;
using System.Linq;
using GraphCollection.GraphComponents;
using UnityEngine;

namespace GraphCollection.SearchAlgorithm
{
    /// <summary> Class for the DepthFirstSearch search-algorithm </summary>
    /// <author>Johannes Bätz</author>
    public class DepthFirstSearch : SearchAlgorithmParent
    {
        private bool _endAllIterations;
        /// <summary> Constructor of the DepthFirstSearch class. Don't instantiate your own DepthFirstSearch, use the GraphManager! </summary>
        /// <param name="graph">The graph you are traversing.</param>
        /// <param name="start">The start vertex.</param>
        /// <param name="end">The vertex to stop the traversing at.</param>
        public DepthFirstSearch(Graph graph, Vertex start, Vertex end) : base(graph, start, end) { }

        /// <summary> The method to start the algorithm and fill all needed lists. </summary>
        public override void Search()
        {
            _endAllIterations = false;
            for (int i = 0; i < _visitedVertecies.Length; i++)
                _visitedVertecies[i] = false;
            deapthSearch(_start);
            fillVisitedList();
            fillPathList();
        }

        /// <summary> Method to validate wether or not a StartVertex and an EndVertex are connected through Verticies. </summary>
        /// <returns> True if it is reachable and false if not. </returns>
        public override bool Validate()
        {
            _endAllIterations = false;
            for (int i = 0; i < _visitedVertecies.Length; i++)
                _visitedVertecies[i] = false;
            deapthSearch(_start);
            return VisitedVertexList.Last.Value.Equals(End);
        }
        
        /// <summary> Starts the actual algorithm. </summary>
        /// <param name="v"> the given vertex to continue the search with. </param>
        /// <param name="edgeArrayIndex">  </param>
        private void deapthSearch(Vertex v)
        {
            if (_endAllIterations) return;
            // Debug.Log("DepthSearch Vertex: " + v.TileData.HexPos);
            _visitedVertecies[v.Position] = true;
            if (!VisitedVertexList.Contains(v))
                VisitedVertexList.AddLast(v);
            Debug.Log("EdgeArrayIndex: ");
            _endAllIterations = compareVerticies(v);
            LinkedList<Edge> list = _adj[v.Position];
            getNeighbours(v, list);
        }

        private bool compareVerticies(Vertex v)
        {
            return v.Position == _end.Position;
        }

        /// <summary> searches all connected and unmarked neighbours from given marked vertex. </summary>
        /// <param name="v"> the vertex you have. </param>
        /// <param name="list"> all edges connected to the vertex </param>
        private void getNeighbours(Vertex v, LinkedList<Edge> list)
        {
            foreach (var edge in list)
            {
                if (!_visitedVertecies[edge.GETOtherVertexPosition(v.Position)])
                    deapthSearch(edge.GETOtherVertex(v.Position));
            }
        }
    }
}