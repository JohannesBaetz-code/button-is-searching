using FlagCollection;
using MothCollection;
using UnityEngine;

namespace MapDrawCollection
{
    /// <summary>
    /// Handles all Methods for erase something on the Tilemaps.
    /// </summary>
    /// <author> Jannick Mitsch </author>
    /// <date>07.01.2022</date>
    public class TilemapEraser : TilemapDrawer
    {
        public void ClearCompleteTilemap(TilemapType tilemapType)
        {
            if(bc.GetTilemap(Window.MAP_WINDOW, tilemapType) != null) bc.GetTilemap(Window.MAP_WINDOW, tilemapType).ClearAllTiles();
            if(bc.GetTilemap(Window.GRAPH_WINDOW, tilemapType) != null) bc.GetTilemap(Window.GRAPH_WINDOW, tilemapType).ClearAllTiles();
        }

        public void Erase(Vector3Int pos)
        {
            bc.GetTilemap(Window.MAP_WINDOW, TilemapType.GROUND).SetTile(pos, null);
            bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.GROUND).SetTile(pos, null);
            bc.GetTilemap(Window.MAP_WINDOW, TilemapType.UNDERGROUND).SetTile(pos, null);
            EraseWayBetween(pos, CalculateNextPosByDirection(pos, WayDirection.RIGHT_TOP));
            EraseWayBetween(pos, CalculateNextPosByDirection(pos, WayDirection.RIGHT));
            EraseWayBetween(pos, CalculateNextPosByDirection(pos, WayDirection.RIGHT_BOTTOM));
            EraseWayBetween(pos, CalculateNextPosByDirection(pos, WayDirection.LEFT_BOTTOM));
            EraseWayBetween(pos, CalculateNextPosByDirection(pos, WayDirection.LEFT));
            EraseWayBetween(pos, CalculateNextPosByDirection(pos, WayDirection.LEFT_TOP));
        }

        public void EraseWayBetween(Vector3Int firstPos, Vector3Int secondPos)
        {
            if (firstPos.Equals(secondPos)) return;
            WayDirection direction = CalculateWayDirection(firstPos, secondPos);
            ClearWayAt(firstPos, direction);
            WayDirection oppositeDirection = GetOppositeDirection(direction);
            ClearWayAt(secondPos, oppositeDirection);
        }

        /// <summary>
        /// Deletes the Way-Tile in given direction and after that deletes Pin/Moth/Flag if no way anymore on this Tile.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="direction"></param>
        private void ClearWayAt(Vector3Int pos, WayDirection direction)
        {
            switch (direction)
            {
                case WayDirection.RIGHT_TOP:
                    bc.GetTilemap(Window.MAP_WINDOW, TilemapType.WAY_RIGHT_TOP).SetTile(pos, null);
                    bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_RIGHT_TOP).SetTile(pos, null);
                    break;
                case WayDirection.RIGHT:
                    bc.GetTilemap(Window.MAP_WINDOW, TilemapType.WAY_RIGHT).SetTile(pos, null);
                    bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_RIGHT).SetTile(pos, null);
                    break;
                case WayDirection.RIGHT_BOTTOM:
                    bc.GetTilemap(Window.MAP_WINDOW, TilemapType.WAY_RIGHT_BOTTOM).SetTile(pos, null);
                    bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_RIGHT_BOTTOM).SetTile(pos, null);
                    break;
                case WayDirection.LEFT_BOTTOM:
                    bc.GetTilemap(Window.MAP_WINDOW, TilemapType.WAY_LEFT_BOTTOM).SetTile(pos, null);
                    bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_LEFT_BOTTOM).SetTile(pos, null);
                    break;
                case WayDirection.LEFT:
                    bc.GetTilemap(Window.MAP_WINDOW, TilemapType.WAY_LEFT).SetTile(pos, null);
                    bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_LEFT).SetTile(pos, null);
                    break;
                case WayDirection.LEFT_TOP:
                    bc.GetTilemap(Window.MAP_WINDOW, TilemapType.WAY_LEFT_TOP).SetTile(pos, null);
                    bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_LEFT_TOP).SetTile(pos, null);
                    break;
            }
            if (!IsWayAt(pos))
            {
                DeletePinAt(pos);
                MothSetter mothSetter = MothSetter.GetInstance();
                FinishSetter finishSetter = FinishSetter.GetInstance();
                if (mothSetter.IsMothSetted() && mothSetter.MothStartGridPos.Equals(pos))
                {
                    mothSetter.UnsetMoth();
                }
                if (finishSetter.IsFinishSetted && finishSetter.GetFinishPos().Equals(pos))
                {
                    finishSetter.UnsetFlag();
                }
            }
        }

        private void DeletePinAt(Vector3Int pos)
        {
            bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.PIN).SetTile(pos, null);
        }
        
    }
}