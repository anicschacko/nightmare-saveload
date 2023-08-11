using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveGameManager : MonoBehaviour
{

    private string saveFilePath = "";
    
    #if UNITY_EDITOR
    [ContextMenu("Open Persistent Data")]
    public void OpenPersistentPath()
    {
        string path = Application.persistentDataPath;
        System.Diagnostics.Process.Start(path);
    }
    #endif

    public bool TryGetSavedGameState<T>(string fileName, out T savedData)
    {
        savedData = LoadData<T>(fileName);
        
        return savedData != null;
    }

    public void SaveData<T>(T data, string fileName)
   {
        saveFilePath = Path.Combine(Application.persistentDataPath, $"{fileName}.json");
       
        Debug.Log("Saving Game State!");
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(saveFilePath, json);
    }

    private T LoadData<T>(string fileName)
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, $"{fileName}.json");
        
        if (!File.Exists(saveFilePath)) return default;
        
        string json = File.ReadAllText(saveFilePath);
        var fetchedData = JsonUtility.FromJson<T>(json);

        Debug.Log("Game Data Loaded, deleting saved Data!!");
        File.Delete(saveFilePath); //Can be modified based on requirement. 
        return fetchedData;
    }
}


