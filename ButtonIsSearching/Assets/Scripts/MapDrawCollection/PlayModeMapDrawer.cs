using FlagCollection;
using MothCollection;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapDrawCollection
{
    /// <summary>
    /// Manages all Methods for PinChanges, ColorChanges and Desaturation.
    /// </summary>
    /// <author> Jannick Mitsch </author>
    /// <date>07.01.2022</date>
    public class PlayModeMapDrawer : TilemapDrawer
    {
        private Color pathWay;
        private TileBase tileNormalPin, tilePinMoth, tilePinFinish, tilePinVisited, tilePinFinishAndMoth;

        public PlayModeMapDrawer()
        {
            tileNormalPin = bc.GetTileBase(Window.GRAPH_WINDOW, TileBaseType.PIN_NORMAL);
            tilePinMoth = bc.GetTileBase(Window.GRAPH_WINDOW, TileBaseType.PIN_MOTH);
            tilePinFinish = bc.GetTileBase(Window.GRAPH_WINDOW, TileBaseType.PIN_FINISH);
            tilePinVisited = bc.GetTileBase(Window.GRAPH_WINDOW, TileBaseType.PIN_VISITED);
            tilePinFinishAndMoth = bc.GetTileBase(Window.GRAPH_WINDOW, TileBaseType.PIN_FINISH_MOTH);
            pathWay = bc.GetPathWayColor();
        }

        /// <summary>
        /// Sets a ColorOverlay for a Way in MapWindow, where the Moth has to walk on.
        /// Positions have to be NeighbourTiles.
        /// </summary>
        /// <param name="pos1"></param>
        /// <param name="pos2"></param>
        public void SetPathWayBetween(Vector3Int pos1, Vector3Int pos2)
        {
            WayDirection direction = CalculateWayDirection(pos1, pos2);
            WayDirection oppositeDirection = GetOppositeDirection(direction);
            SetWayColorOverlayAt(pos1, direction, pathWay);
            SetWayColorOverlayAt(pos2, oppositeDirection, pathWay);
        }
        
        //------------------------------
        //Pin Set Methods:

        /// <summary>
        /// Used to set the MothPin to another Position.
        /// </summary>
        public void MoveMothPinTo(Vector3Int pos)
        {
            ChangeMothPinToNormalPin();
            ChangeFinishPinDependingOnMothPinPosition(pos);
            bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.PIN).SetTile(pos, tilePinMoth);
        }

        private void ChangeFinishPinDependingOnMothPinPosition(Vector3Int currentMothPinPos)
        {
            if (FinishSetter.GetInstance().IsFinishSetted && MothSetter.GetInstance().IsMothSetted())
            {
                Vector3Int finishPos = FinishSetter.GetInstance().GetFinishPos();
                if (currentMothPinPos == finishPos)
                {
                    bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.FLAG).SetTile(finishPos, tilePinFinishAndMoth);
                }
                else
                {
                    bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.FLAG).SetTile(finishPos, tilePinFinish);
                }
            }
        }

        /// <summary>
        /// only BuildMode
        /// </summary>
        /// <param name="pos"></param>
        public void SetFinishPinTo(Vector3Int pos)
        {
            ResetPinAt(FinishSetter.GetInstance().GetFinishPos());
            bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.FLAG).SetTile(pos, tilePinFinish);
        }

        /// <summary>
        /// Changes the Pin at pos to a visitedPin.
        /// </summary>
        /// <param name="pos"></param>
        public void SetPinToVisited(Vector3Int pos)
        {
            if (IsWayAt(pos))
            {
                bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.PIN).SetTile(pos, tilePinVisited);
            }
        }

        //--------------------------------
        //Pin Reset Methods:

        private void ChangeMothPinToNormalPin()
        {
            foreach (Vector3Int x in GetAllPosInDrawArea())
            {
                if (bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.PIN).GetTile(x) == tilePinMoth)
                {
                    ResetPinAt(x);
                }
            }
        }

        /// <summary>
        /// Changes the Pin at pos to a normalPin.
        /// </summary>
        /// <param name="pos"></param>
        public void ResetPinAt(Vector3Int pos)
        {
            if (bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.PIN).GetTile(pos) != null)
            {
                bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.PIN).SetTile(pos, tileNormalPin);
            }
        }

        /// <summary>
        /// Changes the Pins to a normalPin.
        /// </summary>
        public void ResetAllPins()
        {
            foreach (Vector3Int pos in GetAllPosInDrawArea())
            {
                ResetPinAt(pos);
            }
        }

        //--------------------------------
        //Color Overlay Methods:


        private void SetWayColorOverlayAt(Vector3Int pos, WayDirection direction, Color color)
        {
            switch (direction)
            {
                case WayDirection.RIGHT_TOP:
                    SetColorOverlayAt(bc.GetTilemap(Window.MAP_WINDOW, TilemapType.WAY_RIGHT_TOP), pos, color);
                    break;
                case WayDirection.RIGHT:
                    SetColorOverlayAt(bc.GetTilemap(Window.MAP_WINDOW, TilemapType.WAY_RIGHT), pos, color);
                    break;
                case WayDirection.RIGHT_BOTTOM:
                    SetColorOverlayAt(bc.GetTilemap(Window.MAP_WINDOW, TilemapType.WAY_RIGHT_BOTTOM), pos, color);
                    break;
                case WayDirection.LEFT_BOTTOM:
                    SetColorOverlayAt(bc.GetTilemap(Window.MAP_WINDOW, TilemapType.WAY_LEFT_BOTTOM), pos, color);
                    break;
                case WayDirection.LEFT:
                    SetColorOverlayAt(bc.GetTilemap(Window.MAP_WINDOW, TilemapType.WAY_LEFT), pos, color);
                    break;
                case WayDirection.LEFT_TOP:
                    SetColorOverlayAt(bc.GetTilemap(Window.MAP_WINDOW, TilemapType.WAY_LEFT_TOP), pos, color);
                    break;
            }
        }
        
        /// <summary>
        /// Sets a grey ColorOverlay for the Tile at Position.
        /// </summary>
        /// <param name="pos"></param>
        public void DesaturateAt(Vector3Int pos)
        {
            SetColorOverlayAt(bc.GetTilemap(Window.MAP_WINDOW, TilemapType.GROUND), pos, bc.GetDesaturationColor());
            SetColorOverlayAt(bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.GROUND), pos, bc.GetDesaturationColor());
            SetColorOverlayAt(bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.PIN), pos, bc.GetDesaturationColor());
            foreach (Tilemap tilemap in bc.GetWayMapsLeftMap())
            {
                SetColorOverlayAt(tilemap, pos, bc.GetDesaturationColor());
            }

            foreach (Tilemap tilemap in bc.GetWayMapsRightMap())
            {
                SetColorOverlayAt(tilemap, pos, bc.GetDesaturationColor());
            }

            SetColorOverlayAt(bc.GetTilemap(Window.MAP_WINDOW, TilemapType.UNDERGROUND), pos,
                bc.GetDesaturationColor());
        }

        /// <summary>
        /// Sets a color Overlay for the Sprite of a specific Tile on a Tilemap.
        /// </summary>
        /// <param name="tilemap"></param>
        /// <param name="pos"></param>
        /// <param name="color"></param>
        private void SetColorOverlayAt(Tilemap tilemap, Vector3Int pos, Color color)
        {
            // Flag the tile, inidicating that it can change colour.
            // By default it's set to "Lock Color".
            tilemap.SetTileFlags(pos, TileFlags.None);
            tilemap.SetColor(pos, color);
        }

        /// <summary>
        /// Clears Desaturation.
        /// </summary>
        /// <param name="pos"></param>
        public void SaturateAt(Vector3Int pos)
        {
            ClearColorChangeAt(bc.GetTilemap(Window.MAP_WINDOW, TilemapType.GROUND), pos);
            ClearColorChangeAt(bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.GROUND), pos);
            ClearColorChangeAt(bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.PIN), pos);
            UnsetWayColorChangesAt(pos);
            ClearColorChangeAt(bc.GetTilemap(Window.MAP_WINDOW, TilemapType.UNDERGROUND), pos);
        }

        /// <summary>
        /// Removes the ColorOverlay for a specifid Position on a Tilemap.
        /// </summary>
        /// <param name="tilemap"></param>
        /// <param name="pos"></param>
        private void ClearColorChangeAt(Tilemap tilemap, Vector3Int pos)
        {
            tilemap.SetColor(pos, Color.white);
        }

        /// <summary>
        /// for Map and GraphWindow: removes ColorOverlays of the WayTilemaps.
        /// </summary>
        public void RemoveAllWayColorChanges()
        {
            foreach (Vector3Int pos in GetAllPosInDrawArea())
            {
                UnsetWayColorChangesAt(pos);
            }
        }

        /// <summary>
        /// Clears all ColorOverlays of WayTilemaps.
        /// </summary>
        /// <param name="pos"></param>
        private void UnsetWayColorChangesAt(Vector3Int pos)
        {
            foreach (Tilemap tilemap in bc.GetWayMapsLeftMap())
            {
                ClearColorChangeAt(tilemap, pos);
            }

            foreach (Tilemap tilemap in bc.GetWayMapsRightMap())
            {
                ClearColorChangeAt(tilemap, pos);
            }
        }

        /// <summary>
        /// Saturates all Tiles.
        /// </summary>
        public void ClearDesaturation()
        {
            foreach (Vector3Int pos in GetAllPosInDrawArea())
            {
                SaturateAt(pos);
            }
        }
    }
}