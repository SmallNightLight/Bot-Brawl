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
        Load();
    }

    //Bot Data
    public List<BasePartDataReference> BasePartDatas = new List<BasePartDataReference>();
    private Dictionary<string, BasePartDataReference> BasePartDataLookup = new Dictionary<string, BasePartDataReference>();

    public BotData CurrentBotData;
    public Dictionary<string, BotData> AllBotData = new Dictionary<string, BotData>();


    //Bot behavior
    public List<BaseFunction> DefaultFunctions = new List<BaseFunction>();
    public List<BaseFunction> Functions = new List<BaseFunction>();
    public List<Node> AllNodes = new List<Node>();

    public List<DisplayDo> FunctionsToCompile = new List<DisplayDo>();

    [SerializedDictionary("Name", "Variable")]
    public SerializedDictionary<string, BaseGet> DefaultVariables = new SerializedDictionary<string, BaseGet>();

    [SerializedDictionary("Name", "Variable")]
    public SerializedDictionary<string, BaseGet> CustomVariables = new SerializedDictionary<string, BaseGet>();

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

    public void AddFunctionNode(DisplayDo functionDo)
    {
        FunctionsToCompile.Add(functionDo);
    }

    public void RemoveFunctionNode()
    {

    }

    public void Compile()
    {

    }


    [ContextMenu("SAVE")]
    public void Save()
    {
        SaveData("/Bots.json", new SerializedBotDataCollection(AllBotData));
    }

    [ContextMenu("LOAD")]
    public void Load()
    {
        if (LoadData("/Bots.json", out SerializedBotDataCollection loadedData))
        {
            foreach (var data in loadedData.BotDatas)
            {
                SerializedDictionary<Vector3Int, PartData> newParts = new SerializedDictionary<Vector3Int, PartData>();

                foreach (var part in data.Parts)
                {
                    PartData newPart = ScriptableObject.CreateInstance<PartData>();

                    SerializedPartData partData = part.Value;
                    BasePartDataReference basePartData = BasePartDataLookup[partData.BasePartName];

                    newPart.Initialize(partData.BasePartName, basePartData, partData.Position, partData.Rotation, partData.Material, partData.Cost, partData.Settings);
                    newParts[partData.Position] = newPart;
                }

                BotData botData = new BotData(data.BotName, newParts);
                AllBotData[botData.BotName] = botData;

                if (CurrentBotData == null || CurrentBotData.BotName == "")
                    CurrentBotData = botData;
            }
        }
    }

    public bool SaveData(string relativePath, SerializedBotDataCollection data)
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

    bool LoadData(string relativePath, out SerializedBotDataCollection collection)
    {
        string path = Application.persistentDataPath + relativePath;

        if (!File.Exists(path))
        {
            collection = new SerializedBotDataCollection();
            return false;
            //Debug.LogError("Did not find file at: " + path);
            //throw new FileNotFoundException("Did not find file at: " + path);
        }

        try
        {
            collection = JsonUtility.FromJson<SerializedBotDataCollection>(File.ReadAllText(path));
            return true;
        }
        catch(Exception e)
        {
            Debug.LogError("Could not load data: " + e.Message + "\n" + e.StackTrace);
            throw e;
        }
    }
}