using System;
using System.Collections.Generic;
using System.Linq;
using GraphCollection.GraphComponents;
using UnityEngine;

namespace GraphCollection.SearchAlgorithm
{
    /// <summary> Superclass of all searchalgorithms which provides Lists for visited and chosen vertices. Also it handles the graphinput. </summary>
    /// <author>Johannes Bätz</author>
    public abstract class SearchAlgorithmParent
    {
        /// <summary> This list is for all visited Edges in their right order.
        /// In addition you can see all vertices that have not been visited, but would be in range to go for the algorithm. </summary>
        protected LinkedList<Edge>[] _visitedEdgeList;
        
        /// <summary> The list which is filled after the algorithm ran and is filled with the way the algorithm found to be the best. </summary>
        protected LinkedList<Vertex> _pathVertexList;
        
        /// <summary> The list the Algorithm fills while running and contains all ever visited vertices. </summary>
        protected LinkedList<Vertex> _visitedVertexList;
        protected bool[] _visitedVertecies;
        protected LinkedList<Edge>[] _adj;
        protected Vertex _start;
        protected Vertex _end;
        protected Graph _graph;

        /// <summary> The constructor to provide every SearchAlgorithm with the needed context. </summary>
        /// <param name="graph"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public SearchAlgorithmParent(Graph graph, Vertex start, Vertex end)
        {
            _graph = graph;
            int index = graph.VertexCount;
            _visitedVertecies = new bool[index];
            _visitedEdgeList = new LinkedList<Edge>[index];
            _visitedVertexList = new LinkedList<Vertex>();
            for (int i = 0; i < _visitedEdgeList.Length; i++)
                _visitedEdgeList[i] = new LinkedList<Edge>();
            for (int i = 0; i < _visitedVertecies.Length; i++)
                _visitedVertecies[i] = false;
            _adj = graph.Adj;
            _start = start;
            _end = end;
            _pathVertexList = new LinkedList<Vertex>();
        }

        public abstract void Search();

        public abstract bool Validate();

        public LinkedList<Vertex> PathVertexList
        {
            get => _pathVertexList;
            protected set => _pathVertexList = value;
        }
        
        /// <summary> Fills the VisitedEdgeList. </summary>
        protected void fillVisitedList()
        {
            int i = 0;
            foreach (var vertex in VisitedVertexList)
            {
                if (vertex.Equals(End)) return;
                _visitedVertecies[vertex.Position] = false;
                foreach (var edge in _adj[vertex.Position])
                {
                    if (_visitedVertecies[edge.GETOtherVertexPosition(vertex.Position)])
                    {
                        if (i >= _graph.VertexCount) break;
                        if (!VisitedEdgeList.Any(edgeList => edgeList.Contains(edge)))
                            VisitedEdgeList[i].AddLast(edge);
                        _visitedVertecies[edge.GETOtherVertexPosition(vertex.Position)] = false;
                    }
                }
                i++;
            }
        }
        
        /// <summary> Fills the PathVertexList. </summary>
        protected void fillPathList()
        {
            if (VisitedVertexList.Last.Value.Equals(End))
            {
                //rückwärts visited listen durchlaufen, um edges und verticies zu finden
                PathVertexList.AddFirst(End);
                Vertex currentVertex;
                for (int i = VisitedEdgeList.Length - 1; i >= 0; i--)
                {
                    currentVertex = PathVertexList.First.Value;
                    Debug.Log(currentVertex.Position);
                    foreach (var edge in VisitedEdgeList[i])
                    {
                        if (PathVertexList.First.Value.Equals(Start)) return;
                        if (edge.HasVertex(currentVertex))
                        {
                            PathVertexList.AddFirst(edge.GETOtherVertex(currentVertex));
                            break;
                        }
                    }
                }
            }
            else
                Debug.LogError("Liste falsch befüllt");
        }

        public LinkedList<Edge>[] VisitedEdgeList => _visitedEdgeList;

        public LinkedList<Vertex> VisitedVertexList
        {
            get => _visitedVertexList;
            protected set => _visitedVertexList = value;
        }
        public LinkedList<Edge>[] GETVisitedEdgeList() => VisitedEdgeList;
        public LinkedList<Vertex> GETPathVertexList() => PathVertexList;
        public LinkedList<Vertex> GETVisitedVertexList() => VisitedVertexList;
        public Vertex Start
        {
            get => _start;
            set => _start = value;
        }

        public Vertex End
        {
            get => _end;
            set => _end = value;
        }
    }
}