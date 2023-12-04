using ScriptableArchitecture.Data;
using System.Collections.Generic;

[System.Serializable]
public struct SerializedBotDataCollection
{
    public List<SerializedBotData> BotDatas;

    public SerializedBotDataCollection(Dictionary<string, BotData> datas)
    {
        BotDatas = new List<SerializedBotData>();

        foreach (var botData in datas)
            BotDatas.Add(new SerializedBotData(botData.Value));
    }
}