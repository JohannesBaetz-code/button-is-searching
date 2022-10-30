using UnityEngine;

namespace StateManagement
{
    
    /// <summary>
    /// Stores all Sprites and Button-GameObjects that are necessary for ButtonImageChanges depending on states.
    /// </summary>
    /// <author> Jannick Mitsch </author>
    /// <date>07.01.2022</date>
    public class ImageReferenceManager : Singleton<ImageReferenceManager>
    {
        public Sprite tileDrawerTreeUnchecked;
        public Sprite tileDrawerTreeChecked;
        public Sprite tileDrawerSnowUnchecked;
        public Sprite tileDrawerSnowChecked;
        public Sprite tileDrawerIceUnchecked;
        public Sprite tileDrawerIceChecked;
        
        public Sprite FlagUnchecked;
        public Sprite FlagChecked;
        public Sprite FlagSilhouetteUnchecked;
        public Sprite FlagSilhouetteChecked;
        
        public Sprite WayUnchecked;
        public Sprite WayChecked;
        
        public Sprite EraserUnchecked;
        public Sprite EraserChecked;
        
        public Sprite WayEraserUnchecked;
        public Sprite WayEraserChecked;
        
        public Sprite MothButtonUnchecked;
        public Sprite MothButtonChecked;
        public Sprite MothButtonSilhouetteUnchecked;
        public Sprite MothButtonSilhouetteChecked;
        
        public Sprite MothCottonUnchecked;
        public Sprite MothCottonChecked;
        public Sprite MothCottonSilhouetteUnchecked;
        public Sprite MothCottonSilhouetteChecked;
        
        public Sprite MothNeedleUnchecked;
        public Sprite MothNeedleChecked;
        public Sprite MothNeedleSilhouetteUnchecked;
        public Sprite MothNeedleSilhouetteChecked;
        
        public Sprite MothPatchUnchecked;
        public Sprite MothPatchChecked;
        public Sprite MothPatchSilhouetteUnchecked;
        public Sprite MothPatchSilhouetteChecked;

        public Sprite PlayButton;
        public Sprite PauseButton;
        
        public Sprite SpeedSlowButton;
        public Sprite SpeedNormalButton;
        public Sprite SpeedFastButton;
        
        public Sprite FragezeichenButton;
        public Sprite AusrufezeichenButton;

        public Sprite Karteikarte;
        public Sprite KarteikarteGolden;

        public Sprite TextFieldShowSwitcherButtonUnshowSprite;
        public Sprite TextFieldShowSwitcherButtonShowSprite;


        public GameObject TreeGroundButton;
        public GameObject SnowGroundButton;
        public GameObject IceGroundButton;
        
        public GameObject WayDrawerButton;
        
        public GameObject EraserButton;
        public GameObject WayEraserButton;

        public GameObject FlagButton;
        
        public GameObject MothButtonButton;
        public GameObject MothButtonCottn;
        public GameObject MothButtonNeedle;
        public GameObject MothButtonPatch;
        
        public GameObject PlayPauseButton;
        public GameObject SpeedButton;
        public GameObject FrageButton;

        public GameObject TextFieldShowSwitcherButton;

    }
}