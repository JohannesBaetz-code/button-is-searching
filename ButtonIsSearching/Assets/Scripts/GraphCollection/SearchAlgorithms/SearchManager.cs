using System;
using System.Collections.Generic;
using GraphCollection.GraphComponents;
using UnityEngine;

namespace GraphCollection.SearchAlgorithm
{
    /// <summary> The manager class for the algorithms. It communicates to other classes and handles in and outputs.
    /// Just call this one if you want to use an algorithm! </summary>
    /// <author> Johannes Bätz </author>
    public class SearchManager
    {
        public Graph Graph { get; set; }
        public Vertex Start { get; set; }
        public Vertex End { get; set; }
        public Algorithm CurrentAlgorithm { get; set; }
        private LinkedList<Edge>[] _visitedEdgeList;
        private LinkedList<Vertex> _pathVertexList;
        private LinkedList<Vertex> _visitedVertexList;
        private SearchAlgorithmParent _currentSearch;
        
        /// <summary> Constructor of SearchManager. </summary>
        /// <param name="graph"> Put in an instance of an graph. </param>
        /// <param name="currentAlgorithm"> Put in an enum value, of an algorithm you want to run. The standard algorithm is DepthFirstSearch. </param>
        /// <param name="algorithmLevel"> Put in an Enum value of the View you want in here. The standard view is TileView. </param>
        public SearchManager(Graph graph, Algorithm currentAlgorithm = Algorithm.DepthFirstSearch)
        {
            Graph = graph;
            CurrentAlgorithm = currentAlgorithm;
        }

        /// <summary> Constructor of SearchManager. </summary>
        /// <param name="graph"> Put in an instance of an graph. </param>
        /// <param name="start"> The vertex an algorithm will start from. </param>
        /// <param name="end"> The vertex where the Algorithm stopps. </param>
        /// <param name="currentAlgorithm"> Put in an enum value, of an algorithm you want to run. The standard algorithm is DepthFirstSearch. </param>
        public SearchManager(Graph graph, Vertex start, Vertex end,
            Algorithm currentAlgorithm = Algorithm.DepthFirstSearch)
        {
            Graph = graph;
            CurrentAlgorithm = currentAlgorithm;
            Start = start;
            End = end;
        }

        /// <summary> Starts an SearchAlgorithm and decides which to choose by a strategy pattern. </summary>
        /// <exception cref="NotImplementedException"> Is thrown through an typo mistake or by choosing an unimplemented algorithm. </exception>
        public void StartSearch()
        {
            Debug.Log(CurrentAlgorithm);
            ResetGraph();
            switch (CurrentAlgorithm)
            {
                case Algorithm.BreadthFirstSearch:
                    _currentSearch = new BreadthFirstSearch(Graph, Start, End);
                    break;
                case Algorithm.DepthFirstSearch:
                    _currentSearch = new DepthFirstSearch(Graph, Start, End);
                    break;
                case Algorithm.Dijkstra:
                    _currentSearch = new Dijkstra(Graph, Start, End);
                    break;
                case Algorithm.AStar:
                    _currentSearch = new AStar(Graph, Start, End);
                    break;
                default:
                    throw new NotImplementedException();
            }
            _currentSearch.Search();
            VisitedEdgeList = _currentSearch.VisitedEdgeList;
            PathVertexList = _currentSearch.PathVertexList;
            _visitedVertexList = _currentSearch.VisitedVertexList;
        }

        /// <summary> Validates a graph so you get the Information if an algorithm can go from Start to Finish. </summary>
        /// <returns> True if Start- and EndVertex are reachable through connected edges and vertices. False if not. </returns>
        /// <exception cref="ArgumentException"> Is thrown when the DepthFirstSearch algorithm is not in use. </exception>
        public bool Validate()
        {
            if (CurrentAlgorithm == Algorithm.DepthFirstSearch)
            {
                _currentSearch = new DepthFirstSearch(Graph, Start, End);
                return _currentSearch.Validate();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary> Resets Weight and Heuristic values from a graph to 0. </summary>
        public void ResetGraph()
        {
            foreach (var adjency in Graph.Adj)
            {
                foreach (var edge in adjency)
                {
                    edge.Reset();
                }
            }
        }

        public LinkedList<Edge>[] VisitedEdgeList
        {
            get => _visitedEdgeList;
            private set => _visitedEdgeList = value;
        }

        public LinkedList<Vertex> PathVertexList
        {
            get => _pathVertexList;
            private set => _pathVertexList = value;
        }

        public LinkedList<Vertex> VisistedVertexList
        {
            get => _visitedVertexList;
            private set => _visitedVertexList = value;
        }
    }
}

public enum Algorithm
{
    BreadthFirstSearch,
    DepthFirstSearch,
    Dijkstra,
    AStar,
    None
}