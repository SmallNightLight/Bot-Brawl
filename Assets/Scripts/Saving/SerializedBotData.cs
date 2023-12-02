using AYellowpaper.SerializedCollections;
using ScriptableArchitecture.Data;
using UnityEngine;

[System.Serializable]
public struct SerializedBotData
{
    public string BotName;
    public SerializedDictionary<Vector3Int, SerializedPartData> Parts;
    //public Node BaseNode;

    public SerializedBotData(BotData baseBotData)
    {
        BotName = baseBotData.BotName;
        Parts = new SerializedDictionary<Vector3Int, SerializedPartData>();

        foreach (var part in baseBotData.GetPartsDictionary())
        {
            PartData mainData = part.Value;
            SerializedPartData partData = new SerializedPartData(mainData.name, mainData.BasePart, mainData.Position, mainData.Rotation, mainData.Material, mainData.Cost, mainData.Settings);
            Parts[part.Key] = partData;
        }
    }
}