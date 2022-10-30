using GraphCollection;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapDrawCollection
{
    /// <summary>
    /// Handles all Methods for Draw Ways on Tilemaps.
    /// </summary>
    /// <author> Jannick Mitsch </author>
    /// <date>07.01.2022</date>
    public class WayDrawer : TilemapDrawer
    {
        /// <summary>
        /// Checks: Position not equal and is Ground at both positions.
        /// if true: draw at both positions way that are connected to each other
        /// </summary>
        /// <param name="firstPos"></param>
        /// <param name="secondPos"></param>
        public void DrawWayBetween(Vector3Int firstPos, Vector3Int secondPos)
        {
            if (firstPos.Equals(secondPos)) return;
            if (!(IsGroundAt(firstPos) && IsGroundAt(secondPos))) return;
            WayDirection direction = CalculateWayDirection(firstPos, secondPos);
            WayDirection oppositeDirection = GetOppositeDirection(direction);
            if (direction == WayDirection.NONE || oppositeDirection == WayDirection.NONE) return;
            SetWayAt(firstPos, direction);
            SetWayAt(secondPos, oppositeDirection);
        }

        public void ChangeBiomeAt(Vector3Int pos, Biome biome)
        {
            if (!IsGroundAt(pos)) return;
            Biome lastBiome = GetBiomeOfGroundAt(pos);
            if (biome == lastBiome) return;
            bc.GetTilemap(Window.MAP_WINDOW, TilemapType.GROUND).SetTile(pos, bc.GetMapGroundTileBaseByBiome(biome));
            bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.GROUND).SetTile(pos, bc.GetGraphGroundTileBaseByBiome(biome));
            bc.GetTilemap(Window.MAP_WINDOW, TilemapType.UNDERGROUND).SetTile(pos, bc.GetMapUndergroundTileBaseByBiome(biome));
            ChangeBiomeOfWaysAt(pos, biome);
        }

        private void ChangeBiomeOfWaysAt(Vector3Int pos, Biome biome)
        {
            WayDirection direction = WayDirection.RIGHT_TOP;
            foreach (Tilemap tilemap in bc.GetWayMapsLeftMap())
            {
                if (IsWayInDirection(pos, direction))
                {
                    SetWayAt(pos, direction);
                }
                else
                {
                    tilemap.SetTile(pos, null);
                }

                direction = NextWayDirectionByCycleThroughWayDirections(direction);
            }

            SetRandomObstaclesAt(pos);
        }

        private WayDirection NextWayDirectionByCycleThroughWayDirections(WayDirection direction)
        {
            switch (direction)
            {
                case WayDirection.RIGHT_TOP:
                    return WayDirection.RIGHT;
                case WayDirection.RIGHT:
                    return WayDirection.RIGHT_BOTTOM;
                case WayDirection.RIGHT_BOTTOM:
                    return WayDirection.LEFT_BOTTOM;
                case WayDirection.LEFT_BOTTOM:
                    return WayDirection.LEFT;
                case WayDirection.LEFT:
                    return WayDirection.LEFT_TOP;
                case WayDirection.LEFT_TOP:
                    return WayDirection.RIGHT_TOP;
                default:
                    return WayDirection.NONE;
            }
        }

        private void SetWayAt(Vector3Int pos, WayDirection direction)
        {
            Biome biome = GetBiomeOfGroundAt(pos);
            if (biome == Biome.NONE || direction == WayDirection.NONE) return;
            SetNormalPinAt(pos);
            bc.GetWayTilemap(Window.MAP_WINDOW, direction).SetTile(pos, bc.GetTileBaseMapWay(biome, direction));
            bc.GetWayTilemap(Window.GRAPH_WINDOW, direction).SetTile(pos, bc.GetTileBaseGraphWay(direction));
        }

        public void SetRandomObstaclesAt(Vector3Int pos)
        {
            for (int i = 0; i < 6; i++)
            {
                WayDirection direction = WayDirection.NONE;
                switch (Random.Range(0, 6))
                {
                    case 0:
                        direction = WayDirection.RIGHT_TOP;
                        break;
                    case 1:
                        direction = WayDirection.RIGHT;
                        break;
                    case 2:
                        direction = WayDirection.RIGHT_BOTTOM;
                        break;
                    case 3:
                        direction = WayDirection.LEFT_BOTTOM;
                        break;
                    case 4:
                        direction = WayDirection.LEFT;
                        break;
                    case 5:
                        direction = WayDirection.LEFT_TOP;
                        break;
                }

                SetObstacleAt(pos, direction);
            }
        }

        /// <summary>
        /// Sets a Decoration/Obstacle at this position in direction.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="direction"></param>
        private void SetObstacleAt(Vector3Int pos, WayDirection direction)
        {
            Biome biome = GetBiomeOfGroundAt(pos);
            if (biome == Biome.NONE) return;

            Tilemap wayMap = bc.GetWayTilemap(Window.MAP_WINDOW, direction);
            if (wayMap.GetTile(pos) == null)
            {
                wayMap.SetTile(pos, bc.GetTileBaseMapObstacle(biome, direction));
            }
        }

        public void SetNormalPinAt(Vector3Int pos)
        {
            Tilemap pinMap = bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.PIN);
            if (IsPosInDrawArea(pos) && pinMap.GetTile(pos) == null)
            {
                pinMap.SetTile(pos, bc.GetTileBase(Window.GRAPH_WINDOW, TileBaseType.PIN_NORMAL));
            }
        }
    }
}