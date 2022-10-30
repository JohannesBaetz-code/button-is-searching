using FlagCollection;
using MapDrawCollection;
using UnityEngine;

namespace MothCollection
{
    /// <summary>
    /// Manages the Moth GameObject and moth-specific Data.
    /// </summary>
    /// <author> Jannick Mitsch </author>
    /// <date>07.01.2022</date>
    public class MothSetter
    {
        private static MothSetter _mothSetter;
        public GameObject SettedMoth { get; set; }
        public Algorithm MothAlgorithm { get; set; }
        public Vector3Int MothStartGridPos { get; set; }
        
        private MothSetter()
        {
            SettedMoth = null;
            MothAlgorithm = Algorithm.None;
        }

        public static MothSetter GetInstance()
        {
            if (_mothSetter == null)
            {
                _mothSetter = new MothSetter();
            }
            return _mothSetter;
        }
        
        public void SetNewMothTo(Vector3Int pos, Algorithm algorithm)
        {
            if (!new TilemapDrawer().IsWayAt(pos) || (FinishSetter.GetInstance().IsFinishSetted && FinishSetter.GetInstance().GetFinishPos().Equals(pos))) return;
            UnsetMoth();
            new PlayModeMapDrawer().MoveMothPinTo(pos);
            MothAlgorithm = algorithm;
            MothStartGridPos = pos;
            BuildingCreator bc = BuildingCreator.GetInstance();
            SettedMoth = GameObject.Instantiate(bc.GetMothPrefab(algorithm),
                                                bc.GetTilemap(Window.MAP_WINDOW, TilemapType.PREVIEW).CellToWorld(pos),
                                                new Quaternion());
        }

        public void MoveMoth(Vector3Int posFrom, Vector3Int posTo)
        {
            SettedMoth.GetComponent<AnimationController>().SetMothWalkTargetPos(posFrom, posTo);
        }

        public void UnsetMoth()
        {
            if(IsMothSetted()) new PlayModeMapDrawer().ResetPinAt(MothStartGridPos);
            GameObject.Destroy(SettedMoth);
            MothAlgorithm = Algorithm.None;
            SettedMoth = null;
            MothStartGridPos = new Vector3Int(0, 0, 100);
        }

        public bool IsMothSetted()
        {
            return SettedMoth == null ? false : true;
        }

    }
}