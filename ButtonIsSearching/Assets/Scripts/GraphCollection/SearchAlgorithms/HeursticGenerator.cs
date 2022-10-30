using System.Collections.Generic;
using System.Linq;
using GraphCollection.GraphComponents;
using StateManagement.PlayState;
using UnityEngine;

namespace GraphCollection.SearchAlgorithm
{
    /// <summary> A class to generate a Heuristic for all vertices in a graph thats put in. </summary>
    /// <author> Johannes Bätz </author>
    public class HeuristicGenerator
    {
        private LinkedList<Edge>[] _adj;
        private bool[] _visitedVertecies;
        private LinkedList<LinkedList<Vertex>> layer;
        private int heuristicLevel;

        /// <summary> Constructor </summary>
        /// <param name="graph"> The graph Heuristic is needed to be processed. </param>
        public HeuristicGenerator(Graph graph)
        {
            layer = new LinkedList<LinkedList<Vertex>>();
            _adj = graph.Adj;
            _visitedVertecies = new bool[graph.VertexCount];
            for (int i = 0; i < _visitedVertecies.Length; i++)
                _visitedVertecies[i] = false;
        }

        /// <summary> Call to start the heuristic calculation. </summary>
        /// <param name="start"></param>
        public void generateHeuristic(Vertex start)
        {
            LinkedList<Vertex> queue = new LinkedList<Vertex>();
            queue.AddLast(start);
            heuristicLevel = 0;
            giveAllHeuristic(queue);
            resetVisitedVerticiesArray();
        }

        /// <summary> Algorithm for Heuristic calculation. Is based on the BFS. </summary>
        /// <param name="queue"> A list for all visitedVertices. </param>
        private void giveAllHeuristic(LinkedList<Vertex> queue)
        {
            if (queue == null || queue.Count == 0) return;
            placeHeuristic(queue);
            if (hasAllHeuristic()) return;
            int index = queue.Count - 1;
            while (index >= 0)
            {
                if (hasAllHeuristic())
                {
                    Debug.Log("all got Heuristic");
                    break;
                }

                Vertex currentVertex = queue.First();
                queue.RemoveFirst();
                if (currentVertex == null) break;
                _visitedVertecies[currentVertex.Position] = true;
                getAllNeighbours(currentVertex, queue);
                index--;
            }
            heuristicLevel++;
            giveAllHeuristic(queue);
        }

        /// <summary> Sets the Heuristic values for each vertex. </summary>
        /// <param name="queue"> the list with the knew visitedVertices. </param>
        private void placeHeuristic(LinkedList<Vertex> queue)
        {
            foreach (var vertex in queue)
            {
                if (!_visitedVertecies[vertex.Position])
                {
                    vertex.Heuristic = heuristicLevel;
                    Debug.Log("Vertex bekam Heuristic: " + vertex.Position + " , Heu: " + vertex.Heuristic);
                }
            }
        }
        
        /// <summary> Checks if all vertices has been visited (if yes they have heuristics). </summary>
        /// <returns> True if all vertices has been visited. False if not. </returns>
        private bool hasAllHeuristic()
        {
            foreach (var condition in _visitedVertecies)
                if (!condition) return false;
            return true;
        }

        /// <summary> Adds all reachable neighbours from the visited Vertices to the list. </summary>
        /// <param name="currentVertex">  </param>
        /// <param name="queue"></param>
        private void getAllNeighbours(Vertex currentVertex, LinkedList<Vertex> queue)
        {
            foreach (var edge in _adj[currentVertex.Position])
            {
                if (!_visitedVertecies[edge.GETOtherVertexPosition(currentVertex.Position)])
                {
                    queue.AddLast(edge.GETOtherVertex(currentVertex));
                }
            }
        }
        
        /// <summary> sets all values of the _visitedVertecies Array to false. </summary>
        private void resetVisitedVerticiesArray()
        {
            for (int i = 0; i < _visitedVertecies.Length; i++)
            {
                _visitedVertecies[i] = false;
            }
        }
    }
}