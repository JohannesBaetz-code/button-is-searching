using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Button;
using MapDrawCollection;
using SaveAndLoad;
using StateManagement;
using UnityEngine;

namespace SaveAndLoad
{
    /// <summary>
    ///     The ButtonSaveHandler Class handles all methods that require input from the Import, Export and Loading List Buttons
    /// </summary>
    /// <author>Fanny Weidner</author>
    public class ButtonSaveHandler : MonoBehaviour
    {
        /// <summary> The file with the data that needs to be loaded into the scene </summary>
        private string selectedFile;

        /// <summary> SaveManager keeps all necessary Data and Objects necessary for Loading and Saving </summary>
        private GameObject saveManager { get; set; }

        /// <summary> Assigns a number to all Button prefabs and their saves that are being prepared for loading </summary>
        public int buttonMatch { get; set; }
        
        private GameObject _alertMessage;

        /// <param name="Export Function">
        ///     Calls upon all necessary Data and Components to export the currently active Tilemap into
        ///     a file
        /// </param>
        public void exportFunction()
        {
            Debug.Log("We clicked Export.");
            saveManager = GameObject.FindWithTag("SaveManager");

            BuildingCreator bc = BuildingCreator.GetInstance();
            _alertMessage = bc.ShowAlertMessage("Einen Moment Geduld. Die Daten werden gespeichert.");
            
            saveManager = GameObject.FindWithTag("SaveManager");
            saveManager.GetComponent<SaveTilemaps>().SavesTilemaps();
            
            Destroy(_alertMessage);
            bc.ShowAlertMessage("Map erfolgreich gespeichert!");
        }

        /// <param name="start Load">Initiates the newest step to start Importing a chosen file. </param>
        public void startLoad()
        {
            saveManager = GameObject.FindWithTag("SaveManager");

            if (saveManager.GetComponent<SaveTilemaps>().prefabList != null)
            {
                foreach (var button in saveManager.GetComponent<SaveTilemaps>().prefabList) Destroy(button);
                saveManager.GetComponent<SaveTilemaps>().prefabList.Clear();
                saveManager.GetComponent<SaveTilemaps>().filesList.Clear();
            }
            
            saveManager.GetComponent<SaveTilemaps>().SAVE_FOLDER = Application.dataPath + "/Saves/";
            saveManager.GetComponent<SaveTilemaps>().prefabList = new List<GameObject>();
            saveManager.GetComponent<SaveTilemaps>().filesList = new List<string>();
            if (saveManager.GetComponent<SaveTilemaps>().filesList == null) return;
            saveManager.GetComponent<SaveTilemaps>().SaveCanvas.SetActive(true);
            saveManager.GetComponent<SaveTilemaps>().ListFiles();
        }
        
        /// <summary>
        ///     As there are a collection of saves with different datas and names, we
        /// </summary>
        /// <param name="On Button Click"> Determines the clicked loading button and picks up the required file for import</param>
        public void OnButtonClick()
        {
            // gets current SaveManager Data
            saveManager = GameObject.FindWithTag("SaveManager");

            // string for filter of .txt and .meta data from folder
            string s2 = null;

            //Check the selected Button by id 
            for (var i = 0; i < saveManager.GetComponent<SaveTilemaps>().filesList.Count; i++)
                if (buttonMatch == i)
                    selectedFile = saveManager.GetComponent<SaveTilemaps>().filesList[i];

            // Give the new Save Object from the selected File to the SaveManager's SaveObject
            saveManager.GetComponent<SaveTilemaps>().newSaveObject = 
                SaveSystem.LoadObject<SaveTilemaps.SaveObject>(selectedFile);

            //Load Tilemap hopefully successfully
            saveManager.GetComponent<SaveTilemaps>().LoadTileMaps();

            // Go to BuildMode immediately to see Map
            StateManager.GetInstance().ChangeStateToBuildMode();

            //Start Cleanup of lists
            cleanUp();
        }

        /// <param name="Clean Up"> Clears all Lists of the SaveManager and Prefabs to not stack the next Import attempt</param>
        private void cleanUp()
        {
            saveManager.GetComponent<SaveTilemaps>().SaveCanvas.SetActive(false);
            foreach (var button in saveManager.GetComponent<SaveTilemaps>().prefabList) Destroy(button);
            saveManager.GetComponent<SaveTilemaps>().prefabList.Clear();
            saveManager.GetComponent<SaveTilemaps>().filesList.Clear();
        }
        
        
        /// <summary>
        ///     The current LoadingListUI only shows 7 overall saved Maps and therefore requires an easy lil function to delete files.
        /// </summary>
        /// <param name="On Delete Click"> Deletes the file chosen through the correct button.</param>
        public void OnDeleteClick()
        {
            // gets current SaveManager Data
            saveManager = GameObject.FindWithTag("SaveManager");

            //Check the selected Button by id 
            string deleteFile = null;
            
            for (var i = 0; i < saveManager.GetComponent<SaveTilemaps>().filesList.Count; i++)
                if (buttonMatch == i)
                    deleteFile = saveManager.GetComponent<SaveTilemaps>().filesList[i];

            File.Delete(deleteFile);
            //Also delete meta file
            File.Delete(deleteFile + ".meta");

            BuildingCreator bc = BuildingCreator.GetInstance();
            _alertMessage = bc.ShowAlertMessage("Map erfolgreich geloescht.");
            
            //Start Cleanup of lists
            cleanUp();
            startLoad();
        }
        
        
    }
}