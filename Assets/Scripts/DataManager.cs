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
    public BaseFunction DefaultFunctionSetup;
    public BaseFunction DefaultFunctionUpdate;

    public List<DisplayDo> FunctionsToCompile = new List<DisplayDo>();
    public List<DisplayDo> DosToCompile = new List<DisplayDo>();
    public List<DisplayGet> GetsToCompile = new List<DisplayGet>();

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

    private Dictionary<DisplayDo, BaseFunction> _functions = new Dictionary<DisplayDo, BaseFunction>();
    private Dictionary<DisplayDo, BaseDo> _dos = new Dictionary<DisplayDo, BaseDo>();
    private Dictionary<DisplayGet, BaseGet> _gets = new Dictionary<DisplayGet, BaseGet>();

    public void Compile()
    {
        //Clear all lists
        CurrentBotData.UpdateFunctions.Clear();
        CurrentBotData.SetupFunctions.Clear();

        _functions.Clear();
        _dos.Clear();
        _gets.Clear();

        //First add Default functions
        foreach (DisplayDo displayFunction in FunctionsToCompile)
        {
            if (!displayFunction.IsFunctionNode || !(displayFunction.DefaultDo is BaseFunction))
                FunctionsToCompile.Remove(displayFunction);

            BaseFunction function = displayFunction.DefaultDo as BaseFunction;
            BaseFunction newDo = Instantiate(function);

            _functions[displayFunction] = newDo;
            _dos[displayFunction] = newDo;

            if (function == DefaultFunctionSetup)
                CurrentBotData.SetupFunctions.Add(newDo);
            else if (function == DefaultFunctionUpdate)
                CurrentBotData.UpdateFunctions.Add(newDo);
        }

        //Create other dos
        foreach (DisplayDo displayDo in DosToCompile)
        {
            BaseDo baseDo = displayDo.DefaultDo;
            BaseDo newDo = Instantiate(baseDo);

            _dos[displayDo] = newDo;
        }

        //Create other gets
        foreach (DisplayGet displayGet in GetsToCompile)
        {
            BaseGet baseGet = displayGet.DefaultGet;
            BaseGet newGet = Instantiate(baseGet);

            _gets[displayGet] = newGet;
        }

        //Setup do references
        foreach (var functionPair in _functions)
        {
            BaseFunction function = functionPair.Value;
            DisplayDo functionDo = functionPair.Key;

            if (functionDo.NextDo != null)
            {
                DisplayDo firstDisplayDo = functionDo.NextDo;
                AddNextAndScopes(firstDisplayDo, functionDo);
            }
        }
    }

    private void AddNextAndScopes(DisplayDo currentDo, DisplayDo scope)
    {
        _dos[scope].AddChild(_dos[currentDo]);

        if (currentDo.NextScopedDo != null)
            AddNextAndScopes(currentDo.NextScopedDo, currentDo);

        if (currentDo.NextDo != null)
            AddNextAndScopes(currentDo.NextDo, scope);
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