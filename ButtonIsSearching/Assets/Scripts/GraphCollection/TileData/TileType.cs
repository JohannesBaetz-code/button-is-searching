using Unity.Collections;
using UnityEngine;

namespace GraphCollection
{
    namespace GraphCollection.TileData
    {
        
        /// <summary> Each Tile drawn on the Tilemap is of a specific type. When an edge is drawn between two tiles,
        /// the weight of that connecting edge changes depending on the type. This enum defines the types of the tiles. </summary>
        public enum TILETYPE
        {
            FOREST = 0,
            ICEDESERT = 1,
            LAKE = 2

        }
    }
}