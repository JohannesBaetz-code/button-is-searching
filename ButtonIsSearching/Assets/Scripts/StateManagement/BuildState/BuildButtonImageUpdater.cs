using FlagCollection;
using GraphCollection;
using MapDrawCollection;
using MothCollection;
using UnityEngine.UI;

namespace StateManagement.BuildState
{
    /// <summary>
    /// Inherits Methods for Update all BuildModeUI Buttons depending on DrawMode.
    /// </summary>
    /// <author> Jannick Mitsch </author>
    /// <date>07.01.2022</date>
    public class BuildButtonImageUpdater
    {
        private DrawMode _selectedDrawMode;
        private Biome _selectedBiome;
        private Algorithm _selectedAlgorithm;
        ImageReferenceManager irm;

        public BuildButtonImageUpdater()
        {
            irm = ImageReferenceManager.GetInstance();
        }

        public void UpdateBuildButtonImages(DrawMode selectedDrawMode, Biome selectedBiome, Algorithm selectedAlgorithm)
        {
            _selectedDrawMode = selectedDrawMode;
            _selectedBiome = selectedBiome;
            _selectedAlgorithm = selectedAlgorithm;

            UpdateFinishButtonImage();

            UpdateMothButtonButtonImage();
            UpdateMothCottonButtonImage();
            UpdateMothNeedleButtonImage();
            UpdateMothPatchButtonImage();

            UpdateWayDrawButtonImage();
            UpdateTileDrawButtonsImages();
            UpdateEraserButtonsImages();
        }

        private void UpdateWayDrawButtonImage()
        {
            irm.WayDrawerButton.GetComponent<Image>().sprite = irm.WayUnchecked;
            if (_selectedDrawMode == DrawMode.WAY_DRAWER)
            {
                irm.WayDrawerButton.GetComponent<Image>().sprite = irm.WayChecked;
            }
        }

        private void UpdateEraserButtonsImages()
        {
            irm.EraserButton.GetComponent<Image>().sprite = irm.EraserUnchecked;
            irm.WayEraserButton.GetComponent<Image>().sprite = irm.WayEraserUnchecked;
            if (_selectedDrawMode == DrawMode.TILE_ERASER || _selectedDrawMode == DrawMode.WAY_ERASER)
            {
                irm.EraserButton.GetComponent<Image>().sprite = irm.EraserChecked;
                if (_selectedDrawMode == DrawMode.WAY_ERASER)
                {
                    irm.WayEraserButton.GetComponent<Image>().sprite = irm.WayEraserChecked;
                }
            }
        }

        private void UpdateTileDrawButtonsImages()
        {
            irm.TreeGroundButton.GetComponent<Image>().sprite = irm.tileDrawerTreeUnchecked;
            irm.SnowGroundButton.GetComponent<Image>().sprite = irm.tileDrawerSnowUnchecked;
            irm.IceGroundButton.GetComponent<Image>().sprite = irm.tileDrawerIceUnchecked;
            if (_selectedDrawMode == DrawMode.TILE_DRAWER)
            {
                switch (_selectedBiome)
                {
                    case Biome.TREES:
                        irm.TreeGroundButton.GetComponent<Image>().sprite = irm.tileDrawerTreeChecked;
                        break;
                    case Biome.SNOW:
                        irm.SnowGroundButton.GetComponent<Image>().sprite = irm.tileDrawerSnowChecked;
                        break;
                    case Biome.ICE:
                        irm.IceGroundButton.GetComponent<Image>().sprite = irm.tileDrawerIceChecked;
                        break;
                    default:
                        break;
                }
            }
        }

        private void UpdateFinishButtonImage()
        {
            Image image = irm.FlagButton.GetComponent<Image>();
            if (_selectedDrawMode != DrawMode.FINISH_SETTER)
            {
                if (FinishSetter.GetInstance().IsFinishSetted)
                {
                    image.sprite = irm.FlagSilhouetteUnchecked;
                }
                else
                {
                    image.sprite = irm.FlagUnchecked;
                }
            }
            else
            {
                if (FinishSetter.GetInstance().IsFinishSetted)
                {
                    image.sprite = irm.FlagSilhouetteChecked;
                }
                else
                {
                    image.sprite = irm.FlagChecked;
                }
            }
        }

        private bool IsMothSettedAndAlgorithmEquals(Algorithm algorithm)
        {
            if (MothSetter.GetInstance().IsMothSetted() && MothSetter.GetInstance().MothAlgorithm == algorithm)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void UpdateMothButtonButtonImage()
        {
            Image image = irm.MothButtonButton.GetComponent<Image>();

            if (_selectedAlgorithm == Algorithm.DepthFirstSearch)
            {
                if (IsMothSettedAndAlgorithmEquals(Algorithm.DepthFirstSearch))
                {
                    image.sprite = irm.MothButtonSilhouetteChecked;
                }
                else
                {
                    image.sprite = irm.MothButtonChecked;
                }
            }
            else
            {
                if (IsMothSettedAndAlgorithmEquals(Algorithm.DepthFirstSearch))
                {
                    image.sprite = irm.MothButtonSilhouetteUnchecked;
                }
                else
                {
                    image.sprite = irm.MothButtonUnchecked;
                }
            }
        }

        private void UpdateMothCottonButtonImage()
        {
            Image image = irm.MothButtonCottn.GetComponent<Image>();
            if (_selectedAlgorithm == Algorithm.BreadthFirstSearch)
            {
                if (IsMothSettedAndAlgorithmEquals(Algorithm.BreadthFirstSearch))
                {
                    image.sprite = irm.MothCottonSilhouetteChecked;
                }
                else
                {
                    image.sprite = irm.MothCottonChecked;
                }
            }
            else
            {
                if (IsMothSettedAndAlgorithmEquals(Algorithm.BreadthFirstSearch))
                {
                    image.sprite = irm.MothCottonSilhouetteUnchecked;
                }
                else
                {
                    image.sprite = irm.MothCottonUnchecked;
                }
            }
        }

        private void UpdateMothNeedleButtonImage()
        {
            Image image = irm.MothButtonNeedle.GetComponent<Image>();
            if (_selectedAlgorithm == Algorithm.Dijkstra)
            {
                if (IsMothSettedAndAlgorithmEquals(Algorithm.Dijkstra))
                {
                    image.sprite = irm.MothNeedleSilhouetteChecked;
                }
                else
                {
                    image.sprite = irm.MothNeedleChecked;
                }
            }
            else
            {
                if (IsMothSettedAndAlgorithmEquals(Algorithm.Dijkstra))
                {
                    image.sprite = irm.MothNeedleSilhouetteUnchecked;
                }
                else
                {
                    image.sprite = irm.MothNeedleUnchecked;
                }
            }
        }

        private void UpdateMothPatchButtonImage()
        {
            Image image = irm.MothButtonPatch.GetComponent<Image>();
            if (_selectedAlgorithm == Algorithm.AStar)
            {
                if (IsMothSettedAndAlgorithmEquals(Algorithm.AStar))
                {
                    image.sprite = irm.MothPatchSilhouetteChecked;
                }
                else
                {
                    image.sprite = irm.MothPatchChecked;
                }
            }
            else
            {
                if (IsMothSettedAndAlgorithmEquals(Algorithm.AStar))
                {
                    image.sprite = irm.MothPatchSilhouetteUnchecked;
                }
                else
                {
                    image.sprite = irm.MothPatchUnchecked;
                }
            }
        }
    }
}