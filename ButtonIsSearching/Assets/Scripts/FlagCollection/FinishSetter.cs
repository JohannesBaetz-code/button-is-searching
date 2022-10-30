using MapDrawCollection;
using MothCollection;
using UnityEngine;

namespace FlagCollection
{
    /// <summary>
    /// Manages the Finish/Flag (Position, Object, Pin, ...).
    /// </summary>
    /// <author> Jannick Mitsch </author>
    /// <date>07.01.2022</date>
    public class FinishSetter : TilemapDrawer
    {
        private static FinishSetter _finishSetter;
        public GameObject SettedFlag { get; set; }
        public bool IsFinishSetted { get; set; }
        private Vector3Int finishPos;

        private FinishSetter()
        {
        }

        public static FinishSetter GetInstance()
        {
            if (_finishSetter == null)
            {
                _finishSetter = new FinishSetter();
            }
            return _finishSetter;
        }

        public void SetNewFlagTo(Vector3Int pos)
        {
            MothSetter mothSetter = MothSetter.GetInstance();
            if (!IsWayAt(pos) || (mothSetter.IsMothSetted() && mothSetter.MothStartGridPos.Equals(pos))) return;
            UnsetFlag();
            SettedFlag = GameObject.Instantiate(bc.GetFlagPrefab(),
                bc.GetTilemap(Window.MAP_WINDOW, TilemapType.FLAG).CellToWorld(pos),
                new Quaternion());
            new PlayModeMapDrawer().SetFinishPinTo(pos);
            finishPos = pos;
            IsFinishSetted = true;
        }

        public void UnsetFlag()
        {
            GameObject.Destroy(SettedFlag);
            bc.GetTilemap(Window.GRAPH_WINDOW, TilemapType.FLAG).ClearAllTiles();
            IsFinishSetted = false;
        }

        public Vector3Int GetFinishPos()
        {
            return finishPos;
        }
    }
}