using ScriptableArchitecture.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    private BotData _botData;
    [SerializeField] private int _yOffset = 0;

    public bool Active;
    public bool IsInFight = true;
    public bool IsMovingOut;
    [SerializeField] private float _baseMovingSpeed;

    [SerializeField] private bool _setupOnStart = true;


    static Vector3Int[] _directions =
    { 
        new Vector3Int(1, 0, 0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(0, 1, 0),
        new Vector3Int(0, -1, 0),
        new Vector3Int(0, 0, 1),
        new Vector3Int(0, 0, -1),
    };

    private void Start()
    {
        if (_setupOnStart)
            Setup();
    }

    public void Setup()
    {
        Active = true;

        if (_botData == null)
            _botData = DataManager.Instance.CurrentBotData;

#if UNITY_EDITOR
        CheckBotData(_botData);
#endif

        SetupBot(_botData);

        if (IsInFight)
            foreach (BaseFunction function in _botData.SetupFunctions)
                function.Execute();
    }

    public void RemoveBot()
    {
        Active = false;

        foreach (var v in _partGameObjects)
        {
            Destroy(v.Value);
        }
    }

    public void SetBotData(BotData data)
    {
        _botData = data;
    }

    private void Update()
    {
        if (!Active) 
            return;

        if (IsInFight)
        {
            foreach (BaseFunction function in _botData.UpdateFunctions)
                function.Execute();

            //Check health

           

        }
        else
        {
            if (IsMovingOut)
            {
                transform.Translate(Vector3.forward * _baseMovingSpeed * Time.deltaTime);
            }
        }
    }

    private Dictionary<Vector3Int, GameObject> _partGameObjects = new Dictionary<Vector3Int, GameObject>();

    public void SetupBot(BotData botData)
    {
        _partGameObjects.Clear();

        bool firstPiece = false;

        foreach (var part in botData.GetParts())
        {
            Vector3Int partPosition = part.Key;
            PartData partData = part.Value;

            GameObject partObject = Instantiate(partData.BasePart.Value.PartPrefab, transform);
            partObject.transform.rotation = Quaternion.Euler(partData.Rotation.x, partData.Rotation.y, partData.Rotation.z);
            partObject.transform.position =  transform.position + partPosition + new Vector3Int(0, _yOffset, 0);

            if (partObject.TryGetComponent(out ObjectPart objectPartScript))
            {
                objectPartScript.SetPartData(partData);
            }

            _partGameObjects.Add(partPosition, partObject);

            if (!firstPiece)
            {
                firstPiece = true;
                TopDownCamera.Instance?.AddTarget(partObject.transform);
            }
        }

        foreach (var part in _partGameObjects)
        {
            Vector3Int partPosition = part.Key;
            GameObject partObject = part.Value;
            ObjectPart objectPartScript = partObject.GetComponent<ObjectPart>();

            foreach (var direction in _directions)
            {
                Vector3Int otherPartPosition = partPosition + direction;
                if (_partGameObjects.TryGetValue(otherPartPosition, out GameObject otherObject))
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