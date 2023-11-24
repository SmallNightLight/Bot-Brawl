using ScriptableArchitecture.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Bot : MonoBehaviour
{
    [SerializeField] private BotDataReference _botData;
    [SerializeField] private int _yOffset = 0;

    static Vector3Int[] _directions =
    { 
        new Vector3Int(1, 0, 0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(0, 1, 0),
        new Vector3Int(0, -1, 0),
        new Vector3Int(0, 0, 1),
        new Vector3Int(0, 0, -1),
    };

    void Start()
    {
#if UNITY_EDITOR
        CheckBotData(_botData.Value);
#endif
        SetupBot(_botData.Value);
    }

    void Update()
    {
        //Remove
        if (Input.GetKeyDown(KeyCode.P))
            SceneManager.LoadScene("BotAssembleScene");
    }

    public void SetupBot(BotData botData)
    {
        Dictionary<Vector3Int, GameObject> partGameObjects = new Dictionary<Vector3Int, GameObject>();

        foreach (var part in botData.GetParts())
        {
            Vector3Int partPosition = part.Key;
            PartData partData = part.Value;

            GameObject partObject = Instantiate(partData.BasePart.Value.PartPrefab, transform);
            partObject.transform.rotation = Quaternion.Euler(partData.Rotation.x, partData.Rotation.y, partData.Rotation.z);
            partObject.transform.position = partPosition + new Vector3Int(0, _yOffset, 0);

            if (partObject.TryGetComponent(out ObjectPart objectPartScript))
            {
                objectPartScript.SetPartData(partData);
            }

            partGameObjects.Add(partPosition, partObject);
        }

        foreach (var part in partGameObjects)
        {
            Vector3Int partPosition = part.Key;
            GameObject partObject = part.Value;
            ObjectPart objectPartScript = partObject.GetComponent<ObjectPart>();

            foreach (var direction in _directions)
            {
                Vector3Int otherPartPosition = partPosition + direction;
                if (partGameObjects.TryGetValue(otherPartPosition, out GameObject otherObject))
                {
                    ObjectPart otherOjectPartScript = otherObject.GetComponent<ObjectPart>();

                    if (otherOjectPartScript == null || !otherOjectPartScript.PartData.BasePart.Value.NeedsAttachment)
                    {
                        Rigidbody otherRigidbody = otherObject.GetComponent<Rigidbody>();
                        FixedJoint connection = partObject.AddComponent<FixedJoint>();
                        connection.connectedBody = otherRigidbody;
                    }
                }
            }
        }
    }

#if UNITY_EDITOR
    public void CheckBotData(BotData botData)
    {
        foreach (var part in botData.GetParts())
        {
            Vector3Int partPosition = part.Key;
            PartData partData = part.Value;
            if (partData.BasePart.Value.PartPrefab == null)
                Debug.LogError("No BasePart assigned: " + partData);
        }
            
    }
#endif
}