using System;
using System.Collections.Generic;
using System.Linq;
using GraphCollection.GraphComponents;
using UnityEngine;

namespace GraphCollection.SearchAlgorithm
{
    /// <summary> The Class for the dijkstra algorithm. You should not implement this class by yourself!
    /// For using, you should use my GraphManager class. </summary>
    ///<author>Johannes Bätz</author>
    public class Dijkstra : SearchAlgorithmParent
    {
        private bool stopSearch;
        LinkedList<Vertex> queue = new LinkedList<Vertex>();
        /// <summary> Constructor of the Dijkstra class. Don't instantiate your own dijkstra, use the GraphManager! </summary>
        /// <param name="graph">The graph you are traversing.</param>
        /// <param name="start">The start vertex.</param>
        /// <param name="end">The vertex to stop the traversing at.</param>
        public Dijkstra(Graph graph, Vertex start, Vertex end) : base(graph, start, end) { }

        /// <summary> It is the starting method for the dijkstra search. </summary>
        public override void Search()
        {
            stopSearch = false;
            queue.Clear();
            dijkstraTileSearch(_start, queue);
            checkVisitedVertexList();
            fillVisitedList();
            fillPathList(queue);
        }

        public override bool Validate()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tilesearch: goes only to end vertex and stops.
        /// after its finished it continues with graphSearch.
        /// </summary>
        /// <param name="currentVertex"> the vertex you start / continue the search. </param>
        /// <param name="chosenVertexList"> all visitedVerticies on the way of the algorithm. </param>
        private void dijkstraTileSearch(Vertex currentVertex, LinkedList<Vertex> chosenVertexList)
        {
            if (stopSearch) return;
            if (!VisitedVertexList.Any(listItem => listItem.Equals(currentVertex)))
            {
                chosenVertexList.AddLast(currentVertex);
                VisitedVertexList.AddLast(currentVertex);
            }

            if (chosenVertexList.Last.Value.Equals(End))
            {
                dijkstraGraphSearch(currentVertex);
                return;
            }
            _visitedVertecies[currentVertex.Position] = true;
            currentVertex = findBestVertex(chosenVertexList);
            dijkstraTileSearch(currentVertex, chosenVertexList);
        }

        /// <summary>
        /// Graph: is the standard algorithm which connects all vertices.
        /// Continued after TileSearch and is only necessary for the GraphView.
        /// </summary>
        /// <param name="currentVertex"> the vertex you are starting / continuing the algorithm. </param>
        private void dijkstraGraphSearch(Vertex currentVertex)
        {
            if (currentVertex != null)
            {
                if (!VisitedVertexList.Any(listItem => listItem.Equals(currentVertex)))
                    VisitedVertexList.AddLast(currentVertex);
                _visitedVertecies[currentVertex.Position] = true;
                stopSearch = stopGraphSearch();
                if (stopSearch) return;
                Debug.Log("Vertex: " + currentVertex.X + " " + currentVertex.Y + " " + currentVertex.TileType);
                currentVertex = findBestVertex(VisitedVertexList);
                dijkstraGraphSearch(currentVertex);
            }
        }

        /// <summary> Searches the best vertex which has the lowest costs and is reachable from the known verticies. </summary>
        /// <param name="chosenVertexList"> List with all visited verticies. </param>
        /// <returns> the best Vertex for the algorithm. </returns>
        private Vertex findBestVertex(LinkedList<Vertex> chosenVertexList)
        {
            int weightHolder = Int32.MaxValue;
            Vertex bestVertex = null;
            foreach (var vertex in chosenVertexList)
            {
                foreach (var edge in _adj[vertex.Position])
                {
                    if (!_visitedVertecies[edge.GETOtherVertexPosition(vertex.Position)])
                    {
                        if (edge.GETEdgeDijkstraWeight(edge.GETOtherVertex(vertex)) < weightHolder)
                        {
                            bestVertex = edge.GETOtherVertex(vertex);
                            weightHolder = edge.GETEdgeDijkstraWeight(bestVertex);
                        }
                    }
                }
            }
            return bestVertex;
        }

        /// <summary> Stops the Algorithm, when all verticies have been finished. </summary>
        /// <returns> True if all are visited and false if not. </returns>
        private bool stopGraphSearch()
        {
            int visitAll = 0;
            foreach (var visit in _visitedVertecies)
            {
                if (visit) visitAll++;
            }

            return (visitAll >= _visitedVertecies.Length-1);
        }
        
        /// <summary> Fills the PathVertexList. It is the way, the algorithm found from start to end. </summary>
        /// <param name="queue"> A list from TileSearch with all visited vertices. </param>
        private void fillPathList(LinkedList<Vertex> queue)
        {
            if (queue.Last.Value.Equals(End))
            {
                //rückwärts visited listen durchlaufen, um edges und verticies zu finden
                PathVertexList.AddFirst(End);
                Vertex currentVertex;
                for (int j = VisitedEdgeList.Length - 1; j >= 0; j--)
                {
                    currentVertex = PathVertexList.First.Value;
                    foreach (var edge in VisitedEdgeList[j])
                    {
                        if (PathVertexList.First.Value.Equals(_start)) return;
                        if (edge.HasVertex(currentVertex))
                        {
                            if (!PathVertexList.Any(listItem => listItem.Equals(edge.GETOtherVertex(currentVertex))))
                                PathVertexList.AddFirst(edge.GETOtherVertex(currentVertex));
                            break;
                        }
                    }
                }
            }
        }

        private void checkVisitedVertexList()
        {
            foreach (var edglist in _adj)
            {
                if (!_visitedVertexList.Any(listitem => listitem.Equals(edglist.First.Value.HasVertex(listitem))))
                {
                    if (_visitedVertexList.Any(listitem => listitem.Equals(edglist.First.Value.V)))
                    {
                        _visitedVertexList.AddLast(edglist.First.Value.W);
                    }
                    else
                    {
                        _visitedVertexList.AddLast(edglist.First.Value.V);
                    }
                    return;
                }
            }
        }
    }
}