using System;
using System.Collections.Generic;
using System.Linq;
using GraphCollection.GraphComponents;
using UnityEngine;

namespace GraphCollection.SearchAlgorithm
{
    /// <summary> Class for the BreadthFirstSearch algorithm. </summary>
    /// <author>Johannes Bätz</author>
    public class BreadthFirstSearch : SearchAlgorithmParent
    {
        /// <summary> Constructor of the BreadthFirstSearch class. Don't instantiate your own BreadthFirstSearch, use the GraphManager! </summary>
        /// <param name="graph">The graph you are traversing.</param>
        /// <param name="start">The start vertex.</param>
        /// <param name="end">The vertex to stop the traversing at.</param>
        public BreadthFirstSearch(Graph graph, Vertex start, Vertex end) : base(graph, start, end) { }

        public override void Search()
        {
            Debug.Log("Suche startet");
            LinkedList<Vertex> queue = new LinkedList<Vertex>();
            _visitedVertecies[_start.Position] = true;
            queue.AddLast(_start);
            VisitedVertexList.AddLast(_start);
            startSearch(queue);
            fillVisitedList();
            fillPathList();
        }
        
        public override bool Validate()
        {
            throw new NotImplementedException();
        }

        private void startSearch(LinkedList<Vertex> queue)
        {
            while (queue.Any())
            {
                Vertex currentVertex = queue.First();
                Debug.Log("Momentaner Vertex: " + currentVertex.X + " " + currentVertex.Y);
                queue.RemoveFirst();
                //Debug.Log(currentVertex);
                _visitedVertecies[currentVertex.Position] = true;
                if (!VisitedVertexList.Contains(currentVertex))
                    VisitedVertexList.AddLast(currentVertex);
                if (_end.Equals(currentVertex))
                    return;

                markNeighbours(currentVertex, queue);
            }
        }

        private void markNeighbours(Vertex currentVertex, LinkedList<Vertex> queue)
        {
            foreach (var edge in _adj[currentVertex.Position])
            {
                if (!_visitedVertecies[edge.GETOtherVertexPosition(currentVertex.Position)])
                    queue.AddLast(edge.GETOtherVertex(currentVertex));
            }
        }
    }
}