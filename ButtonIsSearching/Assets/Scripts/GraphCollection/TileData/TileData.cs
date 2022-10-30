using MapDrawCollection;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GraphCollection.TileData
{
    public class TileData
    { 
    /// <summary> Provides all possible TileTypes and therefore weights </summary>
    /// <param name="TILETYPE Enum">Enum containing all defined TileTypes</param>
    ///<author>Fanny Weidner, Johannes Bätz</author>
    private float _colPos { get; }
    private float _rowPos { get; }
    // private Sprite _tileSprite { get; }
    private Biome _tileType { get; set; }
    public Vector3Int HexPos { get; set; }
    private Tilemap _tileMap { get; set; }

    /// <summary> Creates easily accessible Data for each Tile placed on the map. </summary>
    /// <param name="TILE DATA">Generates all data concerning a Tile</param>
    public TileData(int x, int y, Vector3Int hexPos, Tilemap tilemap, Biome biome)
    {
        _colPos = x;
        _rowPos = y;
        // _tileSprite = sprite;
        HexPos = hexPos;
        _tileMap = tilemap;
        _tileType = biome;
    }
    
    public float ColPos => _colPos;
    public float RowPos => _rowPos;
    public Biome TileType => _tileType;
    public Tilemap Tilemap => _tileMap;
    
    }
}

