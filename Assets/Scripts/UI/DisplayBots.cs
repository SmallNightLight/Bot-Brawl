using ScriptableArchitecture.Data;
using System.Collections;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using System.Collections.Generic;

public class DisplayBots : MonoBehaviour
{
    [SerializeField] private GameObject _displayBotPrefab;
    [SerializeField] private Transform _botParent;

    private List<DisplayBot> _displayBots = new List<DisplayBot>();

    private int _index;
    private int _botCount = 0;

    private void Start()
    {
        foreach(var botData in DataManager.Instance.AllBotData)
        {
            if (botData.Value != null)
            {
                _botCount++;

                DisplayBot bot = Instantiate(_displayBotPrefab, _botParent).GetComponent<DisplayBot>();
                bot.Initialize(botData.Value);
                _displayBots.Add(bot);
            }
        }

        _index = 0;
        UpdateBotUI();
    }

    private void UpdateBotUI()
    {
        // Update UI with the current bot data
        //botImage.sprite = DataManager.Instance.BotData[currentIndex].BotImage;
        //botNameText.text = DataManager.Instance.BotData[currentIndex].BotName;
    }

    public void GoNext()
    {
        _index = (_index + 1) % _botCount;
        UpdateBotUI();
    }

    public void GoPrevious()
    {
        _index = (_index - 1 + _botCount) % _botCount;
        UpdateBotUI();
    }

    public void CreateNewBot(string name)
    {
        if (DataManager.Instance.AllBotData.ContainsKey(name))
            return;

        BotData newBotData = new BotData(name, new SerializedDictionary<Vector3Int, PartData>());
        DataManager.Instance.AllBotData[name] = newBotData;
        DataManager.Instance.CurrentBotData = newBotData;
    }

    public void SetCurrentBot()
    {
        DataManager.Instance.CurrentBotData = _displayBots[_index].MyBotData;
    }
}