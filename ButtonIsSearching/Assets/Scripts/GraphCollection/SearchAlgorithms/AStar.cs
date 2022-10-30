using System;
using System.Collections.Generic;
using System.Linq;
using GraphCollection.GraphComponents;
using UnityEngine;

namespace GraphCollection.SearchAlgorithm
{
    /// <summary> Class for the A* search-algorithm. </summary>
    /// <author> Johannes Bätz </author>
    public class AStar : SearchAlgorithmParent
    {
        private HeuristicGenerator _heuristicGenerator;
        /// <summary> Constructor of the A* class. Don't instantiate your own A*, use the GraphManager! </summary>
        /// <param name="graph">The graph you are traversing.</param>
        /// <param name="start">The start vertex.</param>
        /// <param name="end">The vertex to stop the traversing at.</param>
        public AStar(Graph graph, Vertex start, Vertex end) : base(graph, start, end)
        {
            _heuristicGenerator = new HeuristicGenerator(graph);
        }

        /// <summary> Call to start the algorithm and fill all needed lists. </summary>
        public override void Search()
        {
            _heuristicGenerator.generateHeuristic(End);
            Debug.Log("genHeuristics finished and aStarSearch starts.");
            VisitedVertexList.AddLast(Start);
            aStarSearch(Start);
            fillVisitedList();
            fillPathList();
        }
        
        public override bool Validate()
        {
            throw new NotImplementedException();
        }

        /// <summary> Is the starting Method for the actual A*. </summary>
        /// <param name="currentVertex"> The currentVertex to start or continue the algorithm with. </param>
        private void aStarSearch(Vertex currentVertex)
        {
            if (VisitedVertexList.Last.Value.Equals(End)) return;
            //Debug.Log("Momentaner Vertex: " + currentVertex.X + " " + currentVertex.Y + " Heuristic: " + currentVertex.Heuristic);
            if (!VisitedVertexList.Any(listItem => listItem.Equals(currentVertex)))
            {
                VisitedVertexList.AddLast(currentVertex);
            }
            _visitedVertecies[currentVertex.Position] = true;
            currentVertex = findBestVertex();
            aStarSearch(currentVertex);
        }

        /// <summary> Searches for the best Vertex, which he can access with all accessible Edges. </summary>
        /// <returns> the Vertex with lowest costs. </returns>
        private Vertex findBestVertex()
        {
            int weightHolder = Int32.MaxValue;
            Vertex bestVertex = null;
            foreach (var vertex in VisitedVertexList)
            {
                foreach (var edge in _adj[vertex.Position])
                {
                    if (!_visitedVertecies[edge.GETOtherVertexPosition(vertex.Position)])
                    {
                        if (edge.GETEdgeAStarWeight(edge.GETOtherVertex(vertex)) < weightHolder)
                        {
                            bestVertex = edge.GETOtherVertex(vertex);
                            weightHolder = edge.GETEdgeAStarWeight(bestVertex);
                        }
                    }
                }
            }
            return bestVertex;
        }
    }
}