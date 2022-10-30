using System.Collections;
using System.Collections.Generic;
using GraphCollection.GraphComponents;
using UnityEngine;

namespace GraphCollection
{
    /// <summary> The Graph class. You can create a graph, add edges and get the Vertex- and EdgeCount. </summary>
    /// <author>Johannes Bätz, Fanny Weidner</author>
    public class Graph
    {
        //Properties
        private int _vertexCount { get; set; }
        private int _edgeCount { get; }
        
        /// <summary> Adjency List (needs to be serialised)</summary>
        private LinkedList<Edge>[] _adj { get; }
        private List<Vertex> nodes { get; }
        private List<Edge> edges { get; }

        /// <summary> Constructor creates a graph without any edges and vertices, but determines the size of the graph. </summary>
        /// <param name="vertexCount">defines the size of the Graph by generating that much array members in adjency list.</param>
        ///<author>Johannes Bätz, Fanny Weidner</author>
        public Graph(int vertexCount)
        {
            VertexCount = vertexCount;
            _adj = new LinkedList<Edge>[vertexCount];
            for (int i = 0; i < _adj.Length; i++)
            {
                _adj[i] = new LinkedList<Edge>();
            }
            nodes = new List<Vertex>();
            edges = new List<Edge>();
        }
        
        /// <summary> adds an edge to the adjency list (fields are empty before adding an edge). </summary>
        /// <param name="edge">The instance of an edge.</param>
        ///<author>Johannes Bätz</author>
        public void AddEdge(Edge edge)
        {
            Vertex v = edge.V;
            Vertex w = edge.W;
            nodes.Add(v);
            nodes.Add(w);
            edges.Add(edge);
            _adj[v.Position].AddFirst(edge);
            _adj[w.Position].AddFirst(edge);
        }

        //Getter and Setter
        public LinkedList<Edge>[] Adj => _adj;
        public int VertexCount
        {
            get => _vertexCount;
            set => _vertexCount = value;
        }

        public int EdgeCount => _edgeCount;
        public List<Vertex> Nodes => nodes;
        public List<Edge> Edges => edges;
    }
}
