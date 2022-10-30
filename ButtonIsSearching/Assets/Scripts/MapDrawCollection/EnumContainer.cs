namespace MapDrawCollection
{
    public enum DrawMode
    {
        TILE_DRAWER,
        WAY_DRAWER,
        WAY_ERASER,
        TILE_ERASER,
        FINISH_SETTER,
        MOTH_SETTER,
        NONE
    }

    public enum WayDirection
    {
        RIGHT_TOP,
        RIGHT,
        RIGHT_BOTTOM,
        LEFT_BOTTOM,
        LEFT,
        LEFT_TOP,
        NONE
    }

    public enum Window
    {
        MAP_WINDOW,
        GRAPH_WINDOW
    }
     
    public enum TilemapType{
        PREVIEW,
        GROUND,
        UNDERGROUND,
        WAY_RIGHT_TOP,
        WAY_RIGHT,
        WAY_RIGHT_BOTTOM,
        WAY_LEFT_BOTTOM,
        WAY_LEFT,
        WAY_LEFT_TOP,
        PIN,
        FLAG,
        NONE
    }

    public enum TileBaseType
    {
        GROUND_TREE,
        GROUND_SNOW,
        GROUND_ICE,
        UNDERGROUND,
        PIN_NORMAL,
        PIN_FINISH,
        PIN_FINISH_MOTH,
        PIN_VISITED,
        PIN_MOTH,
        FINISH,
        PREVIEW,
        WAY,
        OBSTACLE,
        NONE
    }
}