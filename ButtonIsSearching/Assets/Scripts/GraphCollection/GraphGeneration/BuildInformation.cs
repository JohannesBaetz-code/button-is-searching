using System;
using System.Collections.Generic;
using GraphCollection.GraphComponents;
using UnityEngine;
using UnityEngine.Tilemaps;
using Button;
using FlagCollection;
using MapDrawCollection;
using MothCollection;
using Debug = UnityEngine.Debug;

namespace GraphCollection.GraphGeneration
{
    /// <summary> Class to get all the needed Informations together for StartVertex, EndVertex, the Chosen Algorithm,
    /// the Graph which has been generated. </summary>
    /// <author> Johannes Bätz, Jannick Mitsch </author>
    public class BuildInformation
    {
        private MothSetter _mothSetter = MothSetter.GetInstance();
        public GraphComponent GraphComponent { get; set; }
        public Graph Graph { get; set; }
        public Vertex StartVertex { get; set; }
        public Vertex EndVertex { get; set; }
        public Algorithm Algorithm { get; set; }

        /// <summary> constructor </summary>
        public BuildInformation()
        {
            Debug.Log("clicked");
            Algorithm = _mothSetter.MothAlgorithm;
            Debug.Log(Algorithm);
            GraphComponent = new GraphComponent();
            Graph = GraphComponent._Graph;
            setStartVertex();
            setEndVertex();
        }

        private void setStartVertex()
        {
            StartVertex = VertexByPosition(_mothSetter.MothStartGridPos);
            // Debug.Log("Position starttile: " + StartVertex.X + " " + StartVertex.Y);
        }

        private void setEndVertex()
        {
            EndVertex = VertexByPosition(FinishSetter.GetInstance().GetFinishPos());
            // Debug.Log("Position endtile: " + EndVertex.X + " " + EndVertex.Y);
        }

        /// <summary> Searches the vertex of the painted Graph. </summary>
        /// <param name="position"> Vector3Int as given Position where a vertex should be. </param>
        /// <returns> The vertex with the equal position. </returns>
        private Vertex VertexByPosition(Vector3Int position)
        {
            Vertex currentV, currentW;
            foreach (LinkedList<Edge> edgeList in Graph.Adj)
            {
                foreach (Edge edge in edgeList)
                {
                    currentV = edge.V;
                    currentW = edge.W;
                    if (currentV != null && currentV.TileData.HexPos.Equals(position))
                    {
                        return currentV;
                    }

                    if (currentW != null && currentW.TileData.HexPos.Equals(position))
                    {
                        return currentW;
                    }
                }
            }
            return null;
        }
    }
}