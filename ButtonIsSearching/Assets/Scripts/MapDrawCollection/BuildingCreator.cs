using System;
using GraphCollection;
using GraphCollection.GraphGeneration;
using GraphCollection.SearchAlgorithm;
using TMPro;
using UIHandlerCollection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace MapDrawCollection
{
    
    /// <summary>
    /// Stores all References of Tilemaps, TileBases and Prefabs.
    /// Provides a Variation of Getters.
    /// </summary>
    /// <author> Jannick Mitsch </author>
    /// <date>07.01.2022</date>
    public class BuildingCreator : Singleton<BuildingCreator>
     {
         [SerializeField] private Tilemap previewMapLeft, mapGroundLeft, mapUndergroundLeft, mapFlagLeft;
         [SerializeField] private Tilemap previewMapRight, mapGroundRight, mapPinsRight, mapFlagRight;
         [SerializeField] private Tilemap[] mapWaysLeft, mapWaysRight;

         [SerializeField] private TileBase[] tileBasesTree, tileBasesSnow, tileBasesIce, tileBasesGraphWays;             //tiles for 0-5 obstacles(map) and 6-11 ways(map or Graph)
         [SerializeField] private TileBase tileBaseTreeGround, tileBaseSnowGround, tileBaseIceGround;                    //tiles for Ground in MapWindow
         [SerializeField] private TileBase tileBaseTreeUnderground, tileBaseSnowUnderground, tileBaseIceUnderground;     //tiles for Underground in MapWindow
         [SerializeField] private TileBase tileBasePinNormal, tileBasePinFinish, tileBasePinVisited, tileBasePinMoth;    //pin-tiles for graph window
         [SerializeField] private TileBase tileFinish, tileEraser, tileBasePinFinishMoth;                                //special tiles
         [SerializeField] private TileBase tileTreeGraph, tileSnowGraph, tileIceGraph;                                   //tiles for Ground in GraphWindow
         [SerializeField] private TileBase tileDesaturation;                                                             //half transparent tile for desaturation Overlay

         //Prefabs for Player
         [SerializeField] private GameObject mothDepth;
         [SerializeField] private GameObject mothBreadth;
         [SerializeField] private GameObject mothDijkstra;
         [SerializeField] private GameObject mothAStar;

         [SerializeField] private GameObject flagPrefab;

         [SerializeField] private GameObject unvalidMapMessagePrefab;

         [SerializeField] private GameObject weightSign;
         [SerializeField] private GameObject heuristicSign;

         [SerializeField] private GameObject textFieldCanvas;
         [SerializeField] private GameObject alertMessageCanvas;

         [SerializeField] private Color pathWayColor;
         [SerializeField] private Color desaturationColor;

         public DrawMode SelectedDrawMode { get; set; }
         public Biome SelectedBiome { get; set; }
         public Algorithm SelectedAlgorithm { get; set; }
         public BuildingButtonHandler BuildingButtonHandler { get; set; }

         protected override void Awake()
         {
             base.Awake();
         }

         public Tilemap[] GetWayMapsLeftMap()
         {
             return mapWaysLeft;
         }
         
         public Tilemap[] GetWayMapsRightMap()
         {
             return mapWaysRight;
         }

         public Color GetDesaturationColor()
         {
             return desaturationColor;
         }

         public Color GetPathWayColor()
         {
             return pathWayColor;
         }

         public GameObject GetWeightSignPrefab()
         {
             return weightSign;
         }

         public GameObject GetHeuristicSignPrefab()
         {
             return heuristicSign;
         }

         public GameObject GetTextFieldCanvasObject()
         {
             return textFieldCanvas;
         }

         public GameObject GetFlagPrefab()
         {
             return flagPrefab;
         }

         public GameObject ShowAlertMessage(string text)
         {
             GameObject messageObject = Instantiate(unvalidMapMessagePrefab, alertMessageCanvas.transform);
             messageObject.GetComponentInChildren<TextMeshProUGUI>().text = text;
             return messageObject;
         }
         
         /// <summary>
         /// Returns the Reference to the MothPrefab as GameObject depending on parameter Algorithm.
         /// </summary>
         /// <param name="algorithm">Enum from Searchmanager for wanted Prefab</param>
         /// <returns>Created GameObject</returns>
         public GameObject GetMothPrefab(Algorithm algorithm)
         {
             switch (algorithm)
             {
                 case Algorithm.DepthFirstSearch:
                     return mothDepth;
                 case Algorithm.BreadthFirstSearch:
                     return mothBreadth;
                 case Algorithm.Dijkstra:
                     return mothDijkstra;
                 case Algorithm.AStar:
                     return mothAStar;
                 default:
                     Debug.Log("No Algorithm selected for Getting MothPrefab");
                     return null;
             }
         }

         public TileBase GetMapUndergroundTileBaseByBiome(Biome biome)
         {
             switch (biome)
             {
                 case Biome.TREES:
                     return tileBaseTreeUnderground;
                 case Biome.SNOW:
                     return tileBaseSnowUnderground;
                 case Biome.ICE:
                     return tileBaseIceUnderground;
                 default:
                     return null;
             }
         }
         
         public TileBase GetMapGroundTileBaseByBiome(Biome biome)
         {
             switch (biome)
             {
                 case Biome.TREES:
                     return tileBaseTreeGround;
                 case Biome.SNOW:
                     return tileBaseSnowGround;
                 case Biome.ICE:
                     return tileBaseIceGround;
                 default:
                     return null;
             }
         }
        
         public TileBase GetGraphGroundTileBaseByBiome(Biome biome)
         {
             switch (biome)
             {
                 case Biome.TREES:
                     return tileTreeGraph;
                 case Biome.SNOW:
                     return tileSnowGraph;
                 case Biome.ICE:
                     return tileIceGraph;
                 default:
                     return null;
             }
         }
         
         /// <summary>
         /// Returns a Tilemap of Left/Map-Window or Right/Graph-Window.
         /// </summary>
         /// <param name="window">left or right window</param>
         /// <param name="type">which Tilemap do you want</param>
         /// <returns>null if type not exist for param window</returns>
         public Tilemap GetTilemap(Window window, TilemapType type)
         {
             if (window == Window.MAP_WINDOW)
             {
                 switch (type)
                 {
                     case TilemapType.PREVIEW:
                         return previewMapLeft;
                     case TilemapType.GROUND:
                         return mapGroundLeft;
                     case TilemapType.UNDERGROUND:
                         return mapUndergroundLeft;
                     case TilemapType.FLAG:
                         return mapFlagLeft;
                     case TilemapType.WAY_RIGHT_TOP:
                         return mapWaysLeft[0];
                     case TilemapType.WAY_RIGHT:
                         return mapWaysLeft[1];
                     case TilemapType.WAY_RIGHT_BOTTOM:
                         return mapWaysLeft[2];
                     case TilemapType.WAY_LEFT_BOTTOM:
                         return mapWaysLeft[3];
                     case TilemapType.WAY_LEFT:
                         return mapWaysLeft[4];
                     case TilemapType.WAY_LEFT_TOP:
                         return mapWaysLeft[5];
                 }
             }else if (window == Window.GRAPH_WINDOW)
             {
                 switch (type)
                 {
                     case TilemapType.PREVIEW:
                         return previewMapRight;
                     case TilemapType.PIN:
                         return mapPinsRight;
                     case TilemapType.GROUND:
                         return mapGroundRight;
                     case TilemapType.FLAG:
                         return mapFlagRight;
                     case TilemapType.WAY_RIGHT_TOP:
                         return mapWaysRight[0];
                     case TilemapType.WAY_RIGHT:
                         return mapWaysRight[1];
                     case TilemapType.WAY_RIGHT_BOTTOM:
                         return mapWaysRight[2];
                     case TilemapType.WAY_LEFT_BOTTOM:
                         return mapWaysRight[3];
                     case TilemapType.WAY_LEFT:
                         return mapWaysRight[4];
                     case TilemapType.WAY_LEFT_TOP:
                         return mapWaysRight[5];
                 }
             }
             Debug.Log("Fehler im Getter der Tilemaps!");
             return null;
         }
         
         /// <summary>
         /// Get a Tilemap for Map or Graph Window depending on a WayDirection.
         /// </summary>
         /// <param name="window"></param>
         /// <param name="direction"></param>
         /// <returns></returns>
         public Tilemap GetWayTilemap(Window window, WayDirection direction)
         {
             if (window == Window.MAP_WINDOW)
             {
                 switch (direction)
                 {
                     case WayDirection.RIGHT_TOP:
                         return mapWaysLeft[0];
                     case WayDirection.RIGHT:
                         return mapWaysLeft[1];
                     case WayDirection.RIGHT_BOTTOM:
                         return mapWaysLeft[2];
                     case WayDirection.LEFT_BOTTOM:
                         return mapWaysLeft[3];
                     case WayDirection.LEFT:
                         return mapWaysLeft[4];
                     case WayDirection.LEFT_TOP:
                         return mapWaysLeft[5];
                 }
             }else if (window == Window.GRAPH_WINDOW)
             {
                 switch (direction)
                 {
                     case WayDirection.RIGHT_TOP:
                         return mapWaysRight[0];
                     case WayDirection.RIGHT:
                         return mapWaysRight[1];
                     case WayDirection.RIGHT_BOTTOM:
                         return mapWaysRight[2];
                     case WayDirection.LEFT_BOTTOM:
                         return mapWaysRight[3];
                     case WayDirection.LEFT:
                         return mapWaysRight[4];
                     case WayDirection.LEFT_TOP:
                         return mapWaysRight[5];
                 }
             }
             Debug.Log("Fehler im Getter der Tilemaps!");
             return null;
         }

         /// <summary>
         /// Get tileBases that are not Ways/Edges or Obstacles.
         /// Ground(biome), Finish or pin.
         /// </summary>
         /// <param name="window"></param>
         /// <param name="type"></param>
         /// <returns></returns>
         public TileBase GetTileBase(Window window, TileBaseType type)
         {
             if (window == Window.MAP_WINDOW)
             {
                 switch (type)
                 {
                     case TileBaseType.GROUND_TREE:
                         return tileBaseTreeGround;
                     case TileBaseType.GROUND_SNOW:
                         return tileBaseSnowGround;
                     case TileBaseType.GROUND_ICE:
                         return tileBaseIceGround;
                     case TileBaseType.FINISH:
                         return tileFinish;
                     default:
                         Debug.Log("Fehler im Getter der TileBases!");
                         return null;
                 }
             }else if (window == Window.GRAPH_WINDOW)
             {
                 switch (type)
                 {
                     case TileBaseType.GROUND_TREE:
                         return tileTreeGraph;
                     case TileBaseType.GROUND_SNOW:
                         return tileSnowGraph;
                     case TileBaseType.GROUND_ICE:
                         return tileIceGraph;
                     case TileBaseType.PIN_NORMAL:
                         return tileBasePinNormal;
                     case TileBaseType.PIN_FINISH:
                         return tileBasePinFinish;
                     case TileBaseType.PIN_VISITED:
                         return tileBasePinVisited;
                     case TileBaseType.PIN_MOTH:
                         return tileBasePinMoth;
                     case TileBaseType.PIN_FINISH_MOTH:
                         return tileBasePinFinishMoth;
                     default:
                         Debug.Log("Fehler im Getter der TileBases!");
                         return null;
                 }
             }
             return null;
         }

         /// <summary>
         /// Get the Tile for eraser overlay while drawing.
         /// </summary>
         /// <returns></returns>
         public TileBase GetEraserTile()
         {
             return tileEraser;
         }
         
         /// <summary>
         /// Get the Tile for desaturation overlay in Playmode.
         /// </summary>
         /// <returns></returns>
         public TileBase GetDesaturationTile()
         {
             return tileDesaturation;
         }
         
         /// <summary>
         /// Select one of the TileBaseArrays for WaysAndObstacles on Map depending on biome.
         /// </summary>
         /// <param name="biome"></param>
         /// <returns></returns>
         private TileBase[] SelectTileBaseArrayForMapByBiome(Biome biome)
         {
             switch (biome)
             {
                 case Biome.TREES:
                     return tileBasesTree;
                 case Biome.SNOW:
                     return tileBasesSnow;
                 case Biome.ICE:
                     return tileBasesIce;
             }
             return null;
         }
         
         /// <summary>
         /// Get TileBase of Way for MapWindow depending on biome and direction.
         /// </summary>
         /// <param name="biome"></param>
         /// <param name="direction"></param>
         /// <returns></returns>
         public TileBase GetTileBaseMapWay(Biome biome, WayDirection direction)
         {
             switch (direction)
             {
                 case WayDirection.RIGHT_TOP:
                     return SelectTileBaseArrayForMapByBiome(biome)[6];
                 case WayDirection.RIGHT:
                     return SelectTileBaseArrayForMapByBiome(biome)[7];
                 case WayDirection.RIGHT_BOTTOM:
                     return SelectTileBaseArrayForMapByBiome(biome)[8];
                 case WayDirection.LEFT_BOTTOM:
                     return SelectTileBaseArrayForMapByBiome(biome)[9];
                 case WayDirection.LEFT:
                     return SelectTileBaseArrayForMapByBiome(biome)[10];
                 case WayDirection.LEFT_TOP:
                     return SelectTileBaseArrayForMapByBiome(biome)[11];
             }
             Debug.Log("Fehler TileBaseGetter!");
             return null;
         }
         
         /// <summary>
         /// Get TileBase of Way for GraphWindow depending on direction.
         /// </summary>
         /// <param name="direction"></param>
         /// <returns></returns>
         public TileBase GetTileBaseGraphWay(WayDirection direction)
         {
             switch (direction)
             {
                 case WayDirection.RIGHT_TOP:
                     return tileBasesGraphWays[0];
                 case WayDirection.RIGHT:
                     return tileBasesGraphWays[1];
                 case WayDirection.RIGHT_BOTTOM:
                     return tileBasesGraphWays[2];
                 case WayDirection.LEFT_BOTTOM:
                     return tileBasesGraphWays[3];
                 case WayDirection.LEFT:
                     return tileBasesGraphWays[4];
                 case WayDirection.LEFT_TOP:
                     return tileBasesGraphWays[5];
             }
             Debug.Log("Fehler TileBaseGetter!");
             return null;
         }

         /// <summary>
         /// Get TileBase of Obstacle for MapWindow.
         /// </summary>
         /// <param name="biome"></param>
         /// <param name="direction"></param>
         /// <returns></returns>
         public TileBase GetTileBaseMapObstacle(Biome biome, WayDirection direction)
         {
             switch (direction)
             {
                 case WayDirection.RIGHT_TOP:
                     return SelectTileBaseArrayForMapByBiome(biome)[0];
                 case WayDirection.RIGHT:
                     return SelectTileBaseArrayForMapByBiome(biome)[1];
                 case WayDirection.RIGHT_BOTTOM:
                     return SelectTileBaseArrayForMapByBiome(biome)[2];
                 case WayDirection.LEFT_BOTTOM:
                     return SelectTileBaseArrayForMapByBiome(biome)[3];
                 case WayDirection.LEFT:
                     return SelectTileBaseArrayForMapByBiome(biome)[4];
                 case WayDirection.LEFT_TOP:
                     return SelectTileBaseArrayForMapByBiome(biome)[5];
             }
             Debug.Log("Fehler TileBaseGetter!");
             return null;
         }
         
     }

}
