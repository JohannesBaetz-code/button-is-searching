using GraphCollection;
using StateManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapDrawCollection
{
    /// <summary>
    /// This Singleton manages the Preview while BuildMode Drawing with Mouse.
    /// </summary>
    /// <author> Jannick Mitsch </author>
    /// <date>07.01.2022</date>
    public class PreviewUpdater
    {
        private static PreviewUpdater _previewUpdater;
        private BuildingCreator bc;
        private Vector3Int lastGridPos = new Vector3Int();

        private PreviewUpdater()
        {
            bc = BuildingCreator.GetInstance();
        }
        
        public static PreviewUpdater GetInstance()
        {
            if (_previewUpdater == null)
            {
                _previewUpdater = new PreviewUpdater();
            }
            return _previewUpdater;
        }


        public void DesaturatePositionsWithoutWays()
        {
            Vector3Int[] allPositions = new TilemapDrawer().GetAllPosInDrawArea();
            PlayModeMapDrawer pmDraw = new PlayModeMapDrawer();
            for (int i = 0; i < allPositions.Length; i++)
            {
                if (!new TilemapDrawer().IsWayAt(allPositions[i]))
                {
                    pmDraw.DesaturateAt(allPositions[i]);
                }
            }
        }

        /// <summary>
        /// Depending on current selected DrawMode in BuildStateManager, some PreviewTiles are Drawn.
        /// </summary>
        /// <param name="mouseGridPos"></param>
        /// <param name="mode"></param>
        /// <param name="biome"></param>
        /// <param name="algorithm"></param>
        public void UpdatePreviewWhileMouseDrawing(Vector3Int mouseGridPos, DrawMode mode, Biome biome, Algorithm algorithm)
        {
            // Debug.Log(mouseGridPos);
            Tilemap graphMap = bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.PREVIEW);
            Tilemap mapMap = bc.GetTilemap(Window.MAP_WINDOW, TilemapType.PREVIEW);
            Tilemap undergroundMap = bc.GetTilemap(Window.MAP_WINDOW, TilemapType.UNDERGROUND);
            TilemapEraser eraser = new TilemapEraser();
            
            eraser.ClearCompleteTilemap(TilemapType.PREVIEW);
            new PlayModeMapDrawer().ClearDesaturation();
            
            bc.GetTilemap(Window.MAP_WINDOW, TilemapType.UNDERGROUND).SetTile(lastGridPos, bc.GetMapUndergroundTileBaseByBiome(new TilemapDrawer().GetBiomeOfGroundAt(lastGridPos)));
            if (lastGridPos != mouseGridPos)
            {
                lastGridPos = mouseGridPos;
            }
            
            if (eraser.IsPosInDrawArea(mouseGridPos))
            {
                switch (mode)
                {
                    case DrawMode.TILE_DRAWER:
                        mapMap.SetTile(mouseGridPos, bc.GetMapGroundTileBaseByBiome(biome));
                        graphMap.SetTile(mouseGridPos, bc.GetGraphGroundTileBaseByBiome(biome));
                        undergroundMap.SetTile(mouseGridPos, bc.GetMapUndergroundTileBaseByBiome(biome));
                        break;
                    case DrawMode.WAY_DRAWER:
                        break;
                    case DrawMode.FINISH_SETTER:
                        DesaturatePositionsWithoutWays();
                        break;
                    case DrawMode.TILE_ERASER:
                        mapMap.SetTile(mouseGridPos, bc.GetEraserTile());
                        graphMap.SetTile(mouseGridPos, bc.GetEraserTile());
                        break;
                    case DrawMode.WAY_ERASER:
                        mapMap.SetTile(mouseGridPos, bc.GetEraserTile());
                        graphMap.SetTile(mouseGridPos, bc.GetEraserTile());
                        break;
                    case DrawMode.MOTH_SETTER:
                        DesaturatePositionsWithoutWays();
                        break;
                }
            }
        }


    }
}