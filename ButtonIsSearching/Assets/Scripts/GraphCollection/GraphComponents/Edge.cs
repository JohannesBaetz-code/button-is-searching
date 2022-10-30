using System;
using UnityEngine;

namespace GraphCollection.GraphComponents
{
    /// <summary> The Edge class. Determines the methods and vars to use it for the Tiles in Unity and
    /// for the SearchAlgorithms.</summary>
    ///<author>Johannes Bätz</author>
    public class Edge
    {
        /// <summary> ID for Unity work. </summary>
        public int ID { get; }
        public Vertex V { get; }
        public Vertex W { get; }
        private int _edgeWeight;

        /// <summary> Creates an edge with the minimum needed input. Since we use eges without direction v and w
        /// are not defined as start or end vertex. </summary>
        /// <param name="v"> The start or end vertex of the edge. </param>
        /// <param name="w"> The corresponding vertex. </param>
        /// <param name="id"> ID to identify each edge. </param>
        /// <author>Johannes Bätz</author>
        public Edge(Vertex v, Vertex w, int id)
        {
            ID = id;
            V = v;
            W = w;
            EdgeWeight = (W.GetTileWeight() + V.GetTileWeight())/2;
        }

        /// <summary> Resets heuristic and calculated weights after dijkstra or a* worked on a graph. </summary>
        public void Reset()
        {
            V.Heuristic = 0;
            V.PathWeight = 0;
            W.Heuristic = 0;
            W.PathWeight = 0;
        }
        
        /// <param name="currentVertexPosition"> Position of known vertex. </param>
        /// <returns> Position of other vertex of the edge or -1 since it is unavailable as a valid position. </returns>
        public int GETOtherVertexPosition(int currentVertexPosition)
        {
            if (currentVertexPosition == V.Position)
                return W.Position;
            else if (currentVertexPosition == W.Position)
                return V.Position;
            else 
                return -1;
        }

        /// <param name="currentVertexPosition"> Position of known vertex. </param>
        /// <returns> Other vertex of the edge or null. </returns>
        public Vertex GETOtherVertex(int currentVertexPosition)
        {
            if (currentVertexPosition == V.Position) 
                return W;
            else if (currentVertexPosition == W.Position)
                return V;
            else 
                return null;
        }

        /// <param name="currentVertexPosition"> Known vertex. </param>
        /// <returns> Other vertex of the edge or null. </returns>
        public Vertex GETOtherVertex(Vertex x)
        {
            if (x.Equals(V))
                return W;
            else if (x.Equals(W))
                return V;
            else
                return null;
        }

        /// <param name="position">Known position of an Vertex.</param>
        /// <returns>Vertex with the given position or null.</returns>
        public Vertex GETVertexByPosition(int position)
        {
            if (position == V.Position)
                return V;
            if (position == W.Position)
                return W;
            return null;
        }

        /// <summary> Generates automatically the weight for the unvisited vertex for dijkstra and
        /// stores it in the vertex instance. </summary>
        /// <param name="x">The vertex you want to calculate the weight for.</param>
        /// <returns>IF valid vertex: The weight for the vertex (way from start to vertex).
        /// ELSE: -1: one vertex has a negative weight.
        ///       -2: if a vertex is passed in, that is not included in the graph.
        ///       -3: if an undefined Error occurred and nothing worked.</returns>
        public int GETEdgeDijkstraWeight(Vertex x)
        {
            if (V.PathWeight < 0 || W.PathWeight < 0)
                return -1;
            if (!HasVertex(x))
                return -2;
            if (x.Equals(V))
            {
                V.PathWeight = EdgeWeight + W.PathWeight;
                return V.PathWeight;
            }
            else
            {
                W.PathWeight = EdgeWeight + V.PathWeight;
                return W.PathWeight;
            }
        }

        /// <summary> Generates automatically the weight for the unvisited vertex for aStar and
        /// stores it in the vertex instance. </summary>
        /// <param name="x">The vertex you want to calculate the weight for.</param>
        /// <returns>IF valid vertex: The weight for the vertex (way from start to vertex with heuristic).
        /// ELSE: -1: one vertex has a negative weight.
        ///       -2: if a vertex is passed in, that is not included in the graph.
        ///       -3: if an undefined Error occurred and nothing worked.</returns>
        public int GETEdgeAStarWeight(Vertex x = null)
        {
            if (V.PathWeight < 0 || W.PathWeight < 0)
                return -1;
            if (!HasVertex(x))
                return -2;
            if (x.Equals(V))
            {
                V.PathWeight = EdgeWeight + W.PathWeight + V.Heuristic;
                return V.PathWeight;
            }
            else
            {
                W.PathWeight = EdgeWeight + V.PathWeight + W.Heuristic;
                return W.PathWeight;
            }
        }

        /// <summary> Checks if the vertex is part of the edge. </summary>
        /// <param name="x"> The vertex you want to know about. </param>
        /// <returns>"true" if the vertex is part of the edge and "false" if not.</returns>
        public bool HasVertex(Vertex x)
        {
            return x.Equals(V) || x.Equals(W);
        }

        public int EdgeWeight
        {
            get => _edgeWeight;
            private set
            {
                _edgeWeight = value;
            }
        }
    }
}
