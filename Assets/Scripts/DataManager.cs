using AYellowpaper.SerializedCollections;
using ScriptableArchitecture.Data;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    //Singleton
    private static DataManager _instance;

    public static DataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DataManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("DataManager");
                    _instance = singletonObject.AddComponent<DataManager>();
                }

                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        SetupBaseData();

        //Save(testData.Value);
        Load();
    }

    //SO
    public List<BasePartDataReference> BasePartDatas = new List<BasePartDataReference>();
    private Dictionary<string, BasePartDataReference> BasePartDataLookup = new Dictionary<string, BasePartDataReference>();

    public BotDataReference _currentBotData;
    public List<BotData> BotData = new List<BotData>();
    public List<PartData> PartData = new List<PartData>();
    public List<Node> AllNodes = new List<Node>();

    private void SetupBaseData()
    {
        BasePartDataLookup.Clear();
        foreach (var data in BasePartDatas)
        {
            if (BasePartDataLookup.ContainsKey(data.Value.PartName))
                Debug.LogError("Same name for multiple BasePartData: " + data.Value.PartName);

            BasePartDataLookup[data.Value.PartName] = data;
        }
    }

    public void AddBotData()
    {

    }

    public void Save(BotData data)
    {
        SaveData("/Save1.json", new SerializedBotData(data));
    }

    public void Load()
    {
        SerializedBotData loadedData = LoadData("/Save1.json");

        //Creating new part
        SerializedDictionary<Vector3Int, PartData> newParts = new SerializedDictionary<Vector3Int, PartData>();

        foreach (var part in loadedData.Parts)
        {
            PartData newPart = ScriptableObject.CreateInstance<PartData>();

            SerializedPartData partData = part.Value;
            BasePartDataReference basePartData = BasePartDataLookup[partData.BasePartName];

            newPart.Initialize(partData.BasePartName, basePartData, partData.Position, partData.Rotation, partData.Material, partData.Cost, partData.Settings);
            PartData.Add(newPart);

            newParts[partData.Position] = newPart;
        }
       
        
        BotData botData = new BotData(loadedData.BotName, newParts);
        BotData.Add(botData);
    }

    public bool SaveData(string relativePath, SerializedBotData data)
    {
        string path = Application.persistentDataPath + relativePath;

        try
        {
            if (File.Exists(path))
            {
                Debug.Log("Overwriting saveData at: " + path);
                File.Delete(path);
            }
            else
            {
                Debug.Log("Creating new file at: " + path);
            }

            using FileStream stream = File.Create(path);
            stream.Close();
            File.WriteAllText(path, JsonUtility.ToJson(data));
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("Could not save data: " + e.Message + "\n" + e.StackTrace);
            return false;
        }
    }

    SerializedBotData LoadData(string relativePath)
    {
        string path = Application.persistentDataPath + relativePath;

        if (!File.Exists(path))
        {
            Debug.LogError("Did not find file at: " + path);
            throw new FileNotFoundException("Did not find file at: " + path);
        }

        try
        {
            return JsonUtility.FromJson<SerializedBotData>(File.ReadAllText(path));
        }
        catch(Exception e)
        {
            Debug.LogError("Could not load data: " + e.Message + "\n" + e.StackTrace);
            throw e;
        }
    }
}