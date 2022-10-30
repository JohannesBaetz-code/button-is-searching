using System;
using System.Collections.Generic;
using System.IO;
using GraphCollection;
using GraphCollection.GraphComponents;
using GraphCollection.GraphGeneration;
using MapDrawCollection;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using TileData = GraphCollection.TileData.TileData;

namespace SaveAndLoad
{
    /// <summary>
    ///     SaveTilemaps is the newest iteration of the Import and Export mechanic which allows the player to save and
    /// load maps into/from files.
    /// I apologise that this class is so long and difficult. I did not want to deal with the repercussions of parting
    /// Import and Export (a clever decision) days before handing it in.
    /// </summary>
    /// <author>Fanny Weidner</author>

    public class SaveTilemaps : MonoBehaviour
    {
        // SAVE MANAGER ==================================================================================================
        /// <summary> The SaveManager is a GameObject in the scene that gives different other GameObjects access to
        /// methods and references they require for Import and Export. </summary>
        
        /// <summary> Keeps necessary Objects and Component for the Import Mechanic that the Save Manager handles </summary>
        [SerializeField] private GameObject Prefab;
        [SerializeField] private Transform Container;
        [SerializeField] public GameObject SaveCanvas;
        
        // EXPORT/IMPORT ===============================================================================================
        /// <summary> Creates a new SaveObject that holds the Data (converted to string later) </summary>
        public SaveObject newSaveObject { get; set; }
        /// <summary> Gets the newest instance of the BuildingCreator for all references </summary>
        private BuildingCreator _bc;
        
        /// <summary> EXPORT: Saves all currently active Tilemaps </summary>
        private List<TileData> _allGraphTilemaps;
        /// <summary> EXPORT: Calls upon a GraphGenerator to get all instances that have currently been drawn on the map </summary>
        private GraphComponent _graphComponent;
        
        /// <summary> IMPORT: Current SaveFolder </summary>
        public string SAVE_FOLDER { get; set; }
        /// <summary> IMPORT: Holds names of all files in the current SaveFolder </summary>
        public List<string> filesList { get; set; }
        /// <summary> IMPORT: List of all SaveButton Prefabs that get created </summary>
        public List<GameObject> prefabList { get; set; }

        /// <param name="Start">Makes sure there is no goddamn NullReferenceException</param>
        public void Start()
        {
            _allGraphTilemaps = new List<TileData>();
        }

        
        // EXPORT ======================================================================================================
        
        /// <param name="Saves Tilemaps">Gets all Data from the current Tilemaps to save them into lists</param>
        public void SavesTilemaps()
        {
            // First get Instance and Create one for all data and references
            _graphComponent = new GraphComponent();
            if (_bc == null) _bc = BuildingCreator.GetInstance();

            
            // We save all Ground and Path Data
            var SaveObjectListGround = new List<TileData>();
            var SaveObjectListPaths = new List<TileData>();

            // Gets all Ground and PathTile Coordinates, Types, Layers, (etc) to export them
            var _allGroundTiles =
                _graphComponent.GetAllTiles(_bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.GROUND), false);
            var _allPathTiles_LEFT =
                _graphComponent.GetAllTiles(_bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_LEFT), false);
            var _allPathTiles_LEFT_TOP =
                _graphComponent.GetAllTiles(_bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_LEFT_TOP), false);
            var _allPathTiles_LEFT_BOTTOM =
                _graphComponent.GetAllTiles(_bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_LEFT_BOTTOM), false);
            var _allPathTiles_RIGHT =
                _graphComponent.GetAllTiles(_bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_RIGHT), false);
            var _allPathTiles_RIGHT_TOP =
                _graphComponent.GetAllTiles(_bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_RIGHT_TOP), false);
            var _allPathTiles_RIGHT_BOTTOM =
                _graphComponent.GetAllTiles(_bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_RIGHT_BOTTOM), false);

            // Save all of it together for the final SaveObject
            _allGraphTilemaps.AddRange(_allPathTiles_LEFT);
            _allGraphTilemaps.AddRange(_allPathTiles_LEFT_TOP);
            _allGraphTilemaps.AddRange(_allPathTiles_LEFT_BOTTOM);
            _allGraphTilemaps.AddRange(_allPathTiles_RIGHT);
            _allGraphTilemaps.AddRange(_allPathTiles_RIGHT_TOP);
            _allGraphTilemaps.AddRange(_allPathTiles_RIGHT_BOTTOM);


            foreach (var tile in _allGraphTilemaps) SaveObjectListPaths.Add(tile);
            foreach (var tile in _allGroundTiles) SaveObjectListGround.Add(tile);

            // Saving Requires a conversion of the difficult Saving Data into simple int and string
            SaveConvert(SaveObjectListGround, SaveObjectListPaths);
        }

        /// <summary> JSON .txt files can only keep the data of simple data types such as int and string,
        /// therefore the more advanced data types need to be converted to primitive ones</summary>
        /// <param name="Saves Convert">Takes all intricate/advanced data types to convert them to primitive ones</param>
        private void SaveConvert(List<TileData> SaveObjectListGround, List<TileData> SaveObjectListPaths)
        {
            // Path Coordinates
            List<int> PathX = new List<int>();
            List<int> PathY = new List<int>();

            // Ground Coordinates
            List<int> GroundX = new List<int>();
            List<int> GroundY = new List<int>();
            
            // Tilemaps 1 = left, 2 = top left, 3 = bottom left, 4 = right, 5 = top right, 6 = bottom right
            List<int> Tilemaps = new List<int>();
            
            // Biomes/TileType 0 = trees, 1 = snow, 2 = ice/glacier
            List<int> TileBase = new List<int>();

            // convert the TileData to coordinates and TileType in ints (Only TileType as it is the same Tilemap)
            foreach (TileData groundTile in SaveObjectListGround)
            {
                GroundX.Add(groundTile.HexPos.x);
                GroundY.Add(groundTile.HexPos.y);

                if (groundTile.TileType == Biome.TREES) TileBase.Add(0);

                if (groundTile.TileType == Biome.SNOW) TileBase.Add(1);

                if (groundTile.TileType == Biome.ICE) TileBase.Add(2);
            }

            // convert the TileData to coordinates and TileMap in ints (only requires Tilemap for Pathdirection)
            foreach (TileData pathTile in SaveObjectListPaths)
            {
                PathX.Add(pathTile.HexPos.x);
                PathY.Add(pathTile.HexPos.y);

                var newDirection = new WayDirection();
                newDirection = getPathDirection(pathTile.Tilemap);

                if (newDirection == WayDirection.LEFT) Tilemaps.Add(1);
                if (newDirection == WayDirection.LEFT_TOP) Tilemaps.Add(2);
                if (newDirection == WayDirection.LEFT_BOTTOM) Tilemaps.Add(3);
                if (newDirection == WayDirection.RIGHT) Tilemaps.Add(4);
                if (newDirection == WayDirection.RIGHT_TOP) Tilemaps.Add(5);
                if (newDirection == WayDirection.RIGHT_BOTTOM) Tilemaps.Add(6);
            }

            // Create a new SaveObject with all Graphcomponent Data and converted TileData
            SaveObject saveObject = new SaveObject
            {
                tileDataGround = SaveObjectListGround.ToArray(),
                tileDataPaths = SaveObjectListPaths.ToArray(),
                PathX = PathX.ToArray(), PathY = PathY.ToArray(), GroundX = GroundX.ToArray(),
                GroundY = GroundY.ToArray(),
                TilemapsPaths = Tilemaps.ToArray(), TileBaseGround = TileBase.ToArray()
            };

           
            newSaveObject = saveObject;

            // We save the whole saveObject and its data into a new file
            SaveSystem.SaveObject(saveObject);
       
        }

        
        // IMPORT ======================================================================================================
        
        /// <param name="Load Tilemaps">Takes saveObject from given File and projects its kept data onto Tilemaps</param>
        public void LoadTileMaps()
        {
            // new SaveObject and Instance to work with
            var saveObject = new SaveObject();
            saveObject = newSaveObject;
            _bc = BuildingCreator.GetInstance();
            
            // 1. Draw Ground
            var wayDrawer = new WayDrawer();
            Vector3Int pos = default;
            var groundBiome = Biome.NONE;

            // Based on the position of a Tile and its TileType, the Tile is drawn onto the map
            for (var t = 0; t < saveObject.TileBaseGround.Length; t++)
            {
                if (saveObject.TileBaseGround[t] == 0) groundBiome = Biome.TREES;
                if (saveObject.TileBaseGround[t] == 1) groundBiome = Biome.SNOW;
                if (saveObject.TileBaseGround[t] == 2) groundBiome = Biome.ICE;

                pos = new Vector3Int(saveObject.GroundX[t], saveObject.GroundY[t], 0);
                wayDrawer.DrawGround(pos, groundBiome);
            }

            // 2. Draw Paths
            var pathDirection = WayDirection.NONE;
            
            // Based on the position of a Tile and its Tilemap, the Paths and their connections are calculated
            for (var t = 0; t < saveObject.TilemapsPaths.Length; t++)
            {
                // 1 = left, 2 = top left, 3 = bottom left, 4 = right, 5 = top right, 6 = bottom right
                if (saveObject.TilemapsPaths[t] == 1) pathDirection = WayDirection.LEFT;
                if (saveObject.TilemapsPaths[t] == 2) pathDirection = WayDirection.LEFT_TOP;
                if (saveObject.TilemapsPaths[t] == 3) pathDirection = WayDirection.LEFT_BOTTOM;
                if (saveObject.TilemapsPaths[t] == 4) pathDirection = WayDirection.RIGHT;
                if (saveObject.TilemapsPaths[t] == 5) pathDirection = WayDirection.RIGHT_TOP;
                if (saveObject.TilemapsPaths[t] == 6) pathDirection = WayDirection.RIGHT_BOTTOM;


                pos = new Vector3Int(saveObject.PathX[t], saveObject.PathY[t], 0);
                wayDrawer.DrawWayBetween(pos, wayDrawer.CalculateNextPosByDirection(pos, pathDirection));
            }
        }

        /// <param name="get Path Direction">Changes the current WayDirection based on the Tilemap of the current Tile
        /// to generate its neighbour and draw it accordingly</param>
        public WayDirection getPathDirection(Tilemap tilemapOfTile)
        {
            var direction = new WayDirection();


            if (tilemapOfTile == _bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_LEFT))
                direction = WayDirection.LEFT;

            if (tilemapOfTile == _bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_LEFT_TOP))
                direction = WayDirection.LEFT_TOP;

            if (tilemapOfTile == _bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_LEFT_BOTTOM))
                direction = WayDirection.LEFT_BOTTOM;

            if (tilemapOfTile == _bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_RIGHT))
                direction = WayDirection.RIGHT;

            if (tilemapOfTile == _bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_RIGHT_TOP))
                direction = WayDirection.RIGHT_TOP;

            if (tilemapOfTile == _bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_RIGHT_BOTTOM))
                direction = WayDirection.RIGHT_BOTTOM;

            return direction;
        }
        
        
        /// <param name="List Files"> Checks the saves folder for all current save files to load into SaveButtonPrefabs  </param>
        public void ListFiles()
        {
            string[] files = Directory.GetFiles(SAVE_FOLDER);

            if (files.Length == 0)
            {
                Debug.Log("There are no saves.");
                return;
            }
          
            string SAVE_EXTENSION = "txt";

            // Gets all files from folder if they are a .txt doc (only creates buttons for them, not .meta docs)
            foreach (string file in files)
            {
                string s2 = file.Substring(file.Length - 3);
                if (s2 == SAVE_EXTENSION)
                {
                    filesList.Add(file);
                }
            }

            //Creates the Button Prefabs for the saves (Continue with ButtonSaveHandler)
            for (var i = 0; i < filesList.Count; i++)
            {
                var go = Instantiate(Prefab);
                go.transform.SetParent(Container);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
                go.GetComponent<ButtonSaveHandler>().buttonMatch = i;
                go.GetComponentInChildren<TextMeshProUGUI>().text =
                    "save " + go.GetComponent<ButtonSaveHandler>().buttonMatch;
                go.name = files[i];
                prefabList.Add(go);
            }
        }


        /// <param name="SaveObject"> Class that can be used as a Save Object to keep all references and data necessary
        /// for export and import of intricate as well as primitive Tilemap Data </param>
        [Serializable]
        public class SaveObject
        {
            /// <summary> All Coordinates X and Y from Ground and Path Tiles </summary>
            public int[] PathX;
            public int[] PathY;

            public int[] GroundX;
            public int[] GroundY;

            /// <summary> All Tilemaps from Paths </summary>
            //1 = left, 2 = top left, 3 = bottom left, 4 = right, 5 = top right, 6 = bottom right
            public int[] TilemapsPaths;
            
            /// <summary> All TileTypes from GroundTiles </summary>
            // 0 = trees, 1 = snow, 2 = ice/glacier
            public int[] TileBaseGround;
            
            // TileData Saves for extra conversion safety
            public TileData[] tileDataGround;
            public TileData[] tileDataPaths;
            
        }
    }
}