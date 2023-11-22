using ScriptableArchitecture.Data;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bot : MonoBehaviour
{
    [SerializeField] private BotDataReference _botData;

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
        SetupBot(_botData.Value);
    }

    void Update()
    {
        
    }

    public void SetupBot(BotData botData)
    {
        Dictionary<Vector3Int, GameObject> partGameObjects = new Dictionary<Vector3Int, GameObject>();

        foreach (var part in botData.GetParts())
        {
            Vector3Int partPosition = part.Key;
            BotPartDataReference partData = part.Value;

            GameObject partObject = Instantiate(partData.Value.BasePart.Value.PartPrefab);
            partObject.transform.SetParent(transform);
            partObject.transform.position = partPosition;
            partGameObjects.Add(partPosition, partObject);
        }

        foreach (var part in partGameObjects)
        {
            Vector3Int partPosition = part.Key;
            GameObject partObject = part.Value;

            foreach (var direction in _directions)
            {
                Vector3Int otherPartPosition = partPosition + direction;
                if (partGameObjects.TryGetValue(otherPartPosition, out GameObject otherObject))
                {
                    if (true) //Test if can attach
                    {
                        Rigidbody otherRigidbody = otherObject.GetComponent<Rigidbody>();
                        FixedJoint connection = partObject.AddComponent<FixedJoint>();
                        connection.connectedBody = otherRigidbody;
                    }
                }
            }
        }
    }
}