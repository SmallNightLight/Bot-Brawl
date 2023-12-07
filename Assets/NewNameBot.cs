using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ScriptableArchitecture.Data;
using AYellowpaper.SerializedCollections;
using UnityEngine.SceneManagement;

public class NewNameBot : MonoBehaviour
{
    [SerializeField] private TMP_InputField _nameField;
    [SerializeField] private string _sceneName;

    public void CreateBot()
    {
        CreateNewBot(_nameField.text);
    }

    public void CreateNewBot(string name)
    {
        if (name == "")
            return;

        if (DataManager.Instance.AllBotData.ContainsKey(name))
            return;

        BotData newBotData = new BotData(name, new SerializedDictionary<Vector3Int, PartData>());
        DataManager.Instance.AllBotData[name] = newBotData;
        DataManager.Instance.CurrentBotData = newBotData;

        SceneManager.LoadScene(_sceneName);
    }
}