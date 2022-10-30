using System;
using GraphCollection;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace MapDrawCollection
{
    /// <summary>
    /// Superclass for other MapDrawClasses.
    /// Provides generally Methods for checks and Calculation in relation to Tilemaps.
    /// </summary>
    /// <author> Jannick Mitsch </author>
    /// <date>07.01.2022</date>
    public class TilemapDrawer
    {
        private protected BuildingCreator bc;
        private readonly int maxMapRadius = 5;

        public TilemapDrawer()
        {
            bc = BuildingCreator.GetInstance();
        }

        public void DrawGround(Vector3Int pos, Biome biome)
        {
            if (!IsPosInDrawArea(pos)) return;
            new TilemapEraser().Erase(pos);
            bc.GetTilemap(Window.MAP_WINDOW, TilemapType.GROUND).SetTile(pos, bc.GetMapGroundTileBaseByBiome(biome));
            bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.GROUND).SetTile(pos, bc.GetGraphGroundTileBaseByBiome(biome));
            bc.GetTilemap(Window.MAP_WINDOW, TilemapType.UNDERGROUND).SetTile(pos, bc.GetMapUndergroundTileBaseByBiome(biome));
            new WayDrawer().SetRandomObstaclesAt(pos);
        }

        public Vector3Int[] GetAllPosInDrawArea()
        {
            Vector3Int[] array = new Vector3Int[maxMapRadius * 2 * maxMapRadius * 2];
            int index = 0;
            for (int x = -maxMapRadius; x <= maxMapRadius; x++)
            {
                for (int y = -maxMapRadius; y <= maxMapRadius; y++)
                {
                    Vector3Int v = new Vector3Int(x, y, 0);
                    if (IsPosInDrawArea(v))
                    {
                        array[index] = v;
                        index++;
                    }
                }
            }
            Vector3Int[] output = new Vector3Int[index];
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = array[i];
            }
            return output;
        }

        public Biome GetBiomeOfGroundAt(Vector3Int pos)
        {
            TileBase tile = bc.GetTilemap(Window.MAP_WINDOW, TilemapType.GROUND).GetTile(pos);
            if (tile == bc.GetTileBase(Window.MAP_WINDOW, TileBaseType.GROUND_TREE)) return Biome.TREES;
            if (tile == bc.GetTileBase(Window.MAP_WINDOW, TileBaseType.GROUND_SNOW)) return Biome.SNOW;
            if (tile == bc.GetTileBase(Window.MAP_WINDOW, TileBaseType.GROUND_ICE)) return Biome.ICE;
            return Biome.NONE;
        }

        public WayDirection CalculateWayDirection(Vector3Int posOrigin, Vector3Int posDestination)
        {
            WayDirection direction = WayDirection.NONE;
            int x = posOrigin.x;
            int y = posOrigin.y;
            if (posDestination.x == x + (Math.Abs(y) % 2) && posDestination.y == y + 1)
                direction = WayDirection.RIGHT_TOP;
            else if (posDestination.x == x + 1 && posDestination.y == y) direction = WayDirection.RIGHT;
            else if (posDestination.x == x + (Math.Abs(y) % 2) && posDestination.y == y - 1)
                direction = WayDirection.RIGHT_BOTTOM;
            else if (posDestination.x == x - ((Math.Abs(y) + 1) % 2) && posDestination.y == y - 1)
                direction = WayDirection.LEFT_BOTTOM;
            else if (posDestination.x == x - 1 && posDestination.y == y) direction = WayDirection.LEFT;
            else if (posDestination.x == x - ((Math.Abs(y) + 1) % 2) && posDestination.y == y + 1)
                direction = WayDirection.LEFT_TOP;
            return direction;
        }

        public Vector3Int CalculateNextPosByDirection(Vector3Int pos, WayDirection direction)
        {
            switch (direction)
            {
                case WayDirection.RIGHT_TOP:
                    return new Vector3Int(pos.x + (Math.Abs(pos.y) % 2), pos.y + 1, 0);
                case WayDirection.RIGHT:
                    return new Vector3Int(pos.x + 1, pos.y, 0);
                case WayDirection.RIGHT_BOTTOM:
                    return new Vector3Int(pos.x + (Math.Abs(pos.y) % 2), pos.y - 1, 0);
                case WayDirection.LEFT_BOTTOM:
                    return new Vector3Int(pos.x - ((Math.Abs(pos.y) + 1) % 2), pos.y - 1, 0);
                case WayDirection.LEFT:
                    return new Vector3Int(pos.x - 1, pos.y, 0);
                case WayDirection.LEFT_TOP:
                    return new Vector3Int(pos.x - ((Math.Abs(pos.y) + 1) % 2), pos.y + 1, 0);
                default:
                    return pos;
            }
        }

        public WayDirection GetOppositeDirection(WayDirection direction)
        {
            switch (direction)
            {
                case WayDirection.RIGHT_TOP:
                    return WayDirection.LEFT_BOTTOM;
                case WayDirection.RIGHT:
                    return WayDirection.LEFT;
                case WayDirection.RIGHT_BOTTOM:
                    return WayDirection.LEFT_TOP;
                case WayDirection.LEFT_BOTTOM:
                    return WayDirection.RIGHT_TOP;
                case WayDirection.LEFT:
                    return WayDirection.RIGHT;
                case WayDirection.LEFT_TOP:
                    return WayDirection.RIGHT_BOTTOM;
                default:
                    return WayDirection.NONE;
            }
        }

        public bool IsGroundAt(Vector3Int pos)
        {
            if (bc.GetTilemap(Window.MAP_WINDOW, TilemapType.GROUND).GetTile(pos) != null) return true;
            return false;
        }

        public bool IsWayAt(Vector3Int pos)
        {
            if (bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_RIGHT_TOP).GetTile(pos) ==
                bc.GetTileBaseGraphWay(WayDirection.RIGHT_TOP)) return true;
            if (bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_RIGHT).GetTile(pos) ==
                bc.GetTileBaseGraphWay(WayDirection.RIGHT)) return true;
            if (bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_RIGHT_BOTTOM).GetTile(pos) ==
                bc.GetTileBaseGraphWay(WayDirection.RIGHT_BOTTOM)) return true;
            if (bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_LEFT_BOTTOM).GetTile(pos) ==
                bc.GetTileBaseGraphWay(WayDirection.LEFT_BOTTOM)) return true;
            if (bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_LEFT).GetTile(pos) ==
                bc.GetTileBaseGraphWay(WayDirection.LEFT)) return true;
            if (bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.WAY_LEFT_TOP).GetTile(pos) ==
                bc.GetTileBaseGraphWay(WayDirection.LEFT_TOP)) return true;
            return false;
        }

        /// <summary>
        /// Is at this Position a way in direction?
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public bool IsWayInDirection(Vector3Int pos, WayDirection direction)
        {
            if (direction == WayDirection.NONE || bc.GetWayTilemap(Window.GRAPH_WINDOW, direction).GetTile(pos) == null)
            {
                return false;
            }

            return true;
        }

        public bool IsPosInDrawArea(Vector3Int pos)
        {
            if ((Math.Abs(pos.y) <= 4 && (pos.x - (Math.Abs(pos.y) / 2) >= -4) &&
                 (pos.x + (Math.Abs(pos.y) / 2 + Math.Abs(pos.y % 2)) <= 4))) return true;
            return false;
        }
    }
}