using System.Collections.Generic;
using System.Linq;
using GraphCollection.GraphComponents;
using UnityEngine;

namespace StateManagement.PlayState
{
    /// <summary> Class to provide all needed information delivered by the lists from the SearchManager. </summary>
    public class ListManager
    {
        private LinkedList<Edge>[] _visitedEdgesArray;
        private LinkedList<Vertex> _pathVertexList;
        public LinkedList<Vertex> PathVertexList => _pathVertexList;
        private LinkedList<Vertex> _visitedVertexList;
        public LinkedList<Vertex> VisitedVertexList => _visitedVertexList;
        private Stack<Vertex> _passedVisitedVerticies;
        public Stack<Vertex> PassedVisitedVerticies => _passedVisitedVerticies;
        private Stack<Vertex> _passedPathVerticies;
        private int _vertexIndex;
        public int Vertexindex => _vertexIndex;
        private Vertex currentVertex;
        private LinkedList<Edge> _currentEdges;
        public LinkedList<Edge> CurrentEdges => _currentEdges;

        /// <summary> Constructor. </summary>
        /// <param name="visitedEdgesArray"> List with all visited edges. </param>
        /// <param name="visitedVertexList"> List with all visited vertices. </param>
        /// <param name="pathVertexList"> List with all vertices chosen by the algorithm as the path. </param>
        public ListManager(LinkedList<Edge>[] visitedEdgesArray, LinkedList<Vertex> visitedVertexList,
            LinkedList<Vertex> pathVertexList)
        {
            _visitedEdgesArray = visitedEdgesArray;
            _visitedVertexList = visitedVertexList;
            _pathVertexList = pathVertexList;
            Debug.Log("ANZAHL VERTEX IN PATHLISTE: " + pathVertexList.Count);
            _passedVisitedVerticies = new Stack<Vertex>();
            _passedPathVerticies = new Stack<Vertex>();
            _currentEdges = new LinkedList<Edge>();
            _vertexIndex = -1;
        }

        /// <summary> Removes the first vertex from visitedVertexList and puts it on the stack. </summary>
        /// <returns> The first Vertex from the visitedVertexList. </returns>
        public Vertex NextStepVisitedVertex()
        {
            if (_visitedVertexList.Equals(null) || _visitedVertexList.Count <= 0) return null;
            Vertex vertex = _visitedVertexList.First.Value;
            _passedVisitedVerticies.Push(vertex);
            _visitedVertexList.RemoveFirst();
            _vertexIndex++;
            return vertex;
        }

        /// <summary> Removes the first vertex from pathVertexList and puts it on the stack. </summary>
        /// <returns> The first Vertex from the pathVertexList. </returns>
        public Vertex NextStepPathVertex()
        {
            if (_pathVertexList.Equals(null) || _pathVertexList.Count <= 0) return null;
            Vertex vertex = _pathVertexList.First.Value;
            _passedPathVerticies.Push(vertex);
            _pathVertexList.RemoveFirst();
            return vertex;
        }

        /// <summary> Removes the first vertex from stack and puts it on the first position at the visitedVertexList. </summary>
        /// <returns> The first Vertex from the stack. </returns>
        public Vertex PreviousStepVisitedVertex()
        {
            if (_passedVisitedVerticies.Equals(null) || _passedVisitedVerticies.Count <= 0) return null;
            Vertex vertex = _passedVisitedVerticies.Pop();
            _visitedVertexList.AddFirst(vertex);
            _vertexIndex--;
            return vertex;
        }

        /// <summary> Removes the first vertex from stack and puts it on the first position at the pathVertexList. </summary>
        /// <returns> The first Vertex from the stack. </returns>
        public Vertex PreviousStepPathVertex()
        {
            if (_passedPathVerticies.Equals(null) || _passedPathVerticies.Count <= 0) return null;
            Vertex vertex = _passedPathVerticies.Pop();
            _pathVertexList.AddFirst(vertex);
            return vertex;
        }
    }
}