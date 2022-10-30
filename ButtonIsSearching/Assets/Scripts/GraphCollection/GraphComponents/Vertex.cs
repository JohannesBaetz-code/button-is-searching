using System;
using GraphCollection;
using UnityEngine;

namespace GraphCollection.GraphComponents
{
    /// <summary> The Vertex class, where a custom Vertex can be generated. </summary>
    ///<author>Johannes Bätz, Fanny Weidner</author>
    public class Vertex
    {
        /// <summary> Position is the address in the adjency list and for each vertex individual. </summary>
        public int Position { get; }
        public float X { get; set; }
        public float Y { get; set; }
        public Biome TileType{ get; set; }
        public int TileWeight { get; set; }
        public int Heuristic { get; set; }
        public int PathWeight { get; set; } = 0;
        public TileData.TileData TileData { get; set; }

        /// <summary> Creates an vertex with only its Position and Type. </summary>
        /// <param name="position">the position in the adjency list.</param>
        public Vertex(int position)
        {
            Position = position;
        }

        /// <summary> Checks if a Vector3IntPosition is equal to the position of the vertex. </summary>
        /// <param name="vertexPosition"></param>
        /// <returns> true if the position is equal, false if not. </returns>
        public bool IsVertexCoordinatesEqual(Vector3Int vertexPosition)
        {
            bool equals = false;
            int col = vertexPosition.x;
            int row = vertexPosition.y;

            if ((col == X) && (row == Y))
            {
                equals = true;
            }
            return equals;
        }
        
        /// <summary> Sets the tileweight needed for the edgeweightcalculation. </summary>
        /// <returns> The integer value, of the vertex. </returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetTileWeight()
        {
            switch (TileType)
            {
                case Biome.TREES:
                    TileWeight = 1; break;
                case Biome.SNOW:
                    TileWeight = 3; break;
                case Biome.ICE:
                    TileWeight = 5; break;
                default:
                    throw new NotImplementedException();
            }
            return TileWeight;
        }
    }
}