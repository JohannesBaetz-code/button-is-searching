using System.IO;
using UnityEngine;

namespace SaveAndLoad
{
    /// <summary>
    ///     SaveAndLoad is an older version of a generalised file loader.
    ///     Currently not in use anymore but I like this class
    /// </summary>
    /// <author>Fanny Weidner</author>
    
    public class SaveSystem
    {
        /// <summary> Type of document that holds data </summary>
        private const string SAVE_EXTENSION = "txt";

        /// <summary> Save Folder </summary>
        private static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";
        
        /// <summary> Check if it has been initialised </summary>
        private static bool isInit;


        /// <param name="Init">
        ///     Initialises the current folder directory to access its contents
        /// </param>
        public static void Init()
        {
            if (!isInit)
            {
                isInit = true;
                if (!Directory.Exists(SAVE_FOLDER)) Directory.CreateDirectory(SAVE_FOLDER);
            }
        }

        /// <param name="Save">
        ///     Saves a given set of data in form of a string into a new/preexisting file
        /// </param>
        public static void Save(string fileName, string saveString, bool overwrite)
        {
            Init();
            var saveFileName = fileName;
            var saveNumber = 1;

            if (!overwrite)
                while (File.Exists(SAVE_FOLDER + saveFileName + "." + SAVE_EXTENSION))
                {
                    saveNumber++;
                    saveFileName = fileName + "_" + saveNumber;
                }

            File.WriteAllText(SAVE_FOLDER + saveFileName + "." + SAVE_EXTENSION, saveString);
        }


        /// <param name="SaveObject">
        ///     Takes a given object that holds string data and converts it to save it as string data
        /// </param>
        public static void SaveObject(object saveObject)
        {
            SaveObjectThis("save", saveObject, false);
        }

        /// <param name="SaveObjectThis">
        ///     Converts SaveObject into string data to save it
        /// </param>
        public static void SaveObjectThis(string fileName, object saveObject, bool overwrite)
        {
            var json = JsonUtility.ToJson(saveObject);
            Save(fileName, json, overwrite);
        }

        /// <param name="LoadObject">
        ///     Loads the string of a file and converts it into a SaveObject
        /// </param>
        public static TSaveObject LoadObject<TSaveObject>(string fileName)
        {
            Init();
            var saveString = Load(fileName);
            if (saveString != null)
            {
                var saveObject = JsonUtility.FromJson<TSaveObject>(saveString);
                return saveObject;
            }

            return default;
        }

        /// <param name="LoadMostRecentFile">
        ///     Loads the File that has been saved to the folder most recently.
        /// </param>
        public static string LoadMostRecentFile()
        {
            Init();
            var directoryInfo = new DirectoryInfo(SAVE_FOLDER);
            var saveFiles = directoryInfo.GetFiles("*." + SAVE_EXTENSION);
            FileInfo mostRecentFile = null;
            foreach (var fileInfo in saveFiles)
                if (mostRecentFile == null)
                {
                    mostRecentFile = fileInfo;
                }
                else
                {
                    if (fileInfo.LastWriteTime > mostRecentFile.LastWriteTime) mostRecentFile = fileInfo;
                }

            return null;
        }

        /// <param name="LoadMostRecentObject">
        ///     Loads the SaveObject of a file that has been saved to the folder most recently.
        /// </param>
        public static TSaveObject LoadMostRecentObject<TSaveObject>()
        {
            Init();
            var saveString = LoadMostRecentFile();

            if (saveString != null)
            {
                var saveObject = JsonUtility.FromJson<TSaveObject>(saveString);
                return saveObject;
            }

            return default;
        }

        /// <param name="Load">
        ///     Loads a string from a file by a given name
        /// </param>
        public static string Load(string fileName)
        {
            Init();
            Debug.Log("CurrentFile Load: " + fileName);
            if (File.Exists(fileName))
            {
                var saveString = File.ReadAllText(fileName);
                return saveString;
            }

            return null;
        }
    }
}