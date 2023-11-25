using ScriptableArchitecture.Data;
using System.Collections.Generic;
using System.Data;
using UnityEditor;
using UnityEngine;

public class BotCreator : MonoBehaviour
{
    [SerializeField] private BotDataReference _botData;
    [SerializeField] private TransformReference _cameraTarget;

    private Dictionary<Vector3Int, GameObject> _parts = new Dictionary<Vector3Int, GameObject>();

    int n = 0;
    [SerializeField] private BasePartDataReference _mainPartData;
    [SerializeField] private List<BasePartDataReference> _basePartDataReferences = new List<BasePartDataReference>();

    [SerializeField] private BoolReference IsPlacing;

    private BasePartDataReference _selectedBasePart;

    private Vector3Int _mainPartPosition = new Vector3Int(0, 5, 0);

    private void Start()
    {
        SetupBot();

        //Remove
        _selectedBasePart = _basePartDataReferences[n];
    }

    private void SetupBot()
    {
        if (_botData.Value.GetPartCount() == 0)
        {
            //Create new main block
            _selectedBasePart = _mainPartData;
            PlacingInfo mainPartInfo = new PlacingInfo();
            mainPartInfo.AttachPoint = _mainPartPosition;

            PlacePart(mainPartInfo, true);
        }
        else
        {
            //Setup bot from previous save
            foreach (var part in _botData.Value.GetParts())
                AddUnit(part.Value, part.Value.BasePart.Value.Equals(_mainPartData.Value));
        }

        _selectedBasePart = null;
    }

    private void Update()
    {
        //Remove
        if (Input.GetKeyDown(KeyCode.Space))
        {
            n++;
            if (n == _basePartDataReferences.Count)
                n = 0;

            _selectedBasePart = _basePartDataReferences[n];
        }
    }

    public void PlacePart(PlacingInfo placingInfo)
    {
        if (CanPlaceUnit(placingInfo))
            PlacePart(placingInfo, false);
    }

    private bool CanPlaceUnit(PlacingInfo placingInfo)
    {
        if (_selectedBasePart == null)
            return false;

        List<Vector3Int> attachmentPoints = _selectedBasePart.Value.DefaultData.BasePart.Value.RelativeAttachmentPoints;
        List<Vector3Int> otherAttachmentPoints = placingInfo.OtherPart.BasePart.Value.RelativeAttachmentPoints;


        Quaternion otherRotation = Quaternion.Euler(placingInfo.OtherPart.Rotation);



        for (int i = 0, j = 0; i < otherAttachmentPoints.Count; i++)
        {
            Debug.Log(placingInfo.Normal == -1 * otherAttachmentPoints[i]);


            //Vector3 rotation = (otherRotation * Quaternion.Euler(attachmentPoints[i])).eulerAngles;

            //rotation.x = ClampAngle(rotation.x);
            //rotation.y = ClampAngle(rotation.y);
            //rotation.z = ClampAngle(rotation.z);

            //Vector3Int r = Vector3Int.RoundToInt(rotation);

            //Debug.Log(r.ToString());
            //Debug.Log(placingInfo.Normal);
        }
        return true;
        return false;
    }

    public float ClampAngle(float angle)
    {
        if (angle == 359f)
            return -1f;

        return angle;
    }

    private void PlacePart(PlacingInfo placingInfo, bool AsCameraTarget)
    {
        PartData newPartData = Instantiate(_selectedBasePart.Value.DefaultData);
        newPartData.BasePart = _selectedBasePart;
        newPartData.Position = placingInfo.AttachPoint;

        newPartData.Rotation = Quaternion.FromToRotation(newPartData.BasePart.Value.DefaultAttachmentDirection, placingInfo.Normal).eulerAngles;

        //Only works in editor
        AssetDatabase.CreateAsset(newPartData, "Assets/Data/Bots/Bot1/Part" + newPartData.Position.ToString() + ".asset");
        AssetDatabase.SaveAssets();

        AddPart(newPartData, AsCameraTarget);
    }

    private void AddPart(PartData partData, bool asCameraTarget = false)
    {
        if (_parts.ContainsKey(partData.Position))
            return;

        _botData.Value.AddPart(partData.Position, partData);
        AddUnit(partData, asCameraTarget);
    }

    private void AddUnit(PartData partData, bool asCameraTarget = false)
    {
        GameObject newUnit = Instantiate(partData.BasePart.Value.CreatorPrefab, partData.Position, Quaternion.Euler(partData.Rotation.x, partData.Rotation.y, partData.Rotation.z));

        Unit unitComponent;

        if (!newUnit.TryGetComponent(out unitComponent))
            unitComponent = newUnit.AddComponent<Unit>();

        unitComponent.UnitPartData = partData;

        _parts[partData.Position] = newUnit;

        if (asCameraTarget)
            _cameraTarget.Value = newUnit.transform;
    }

    public void Change(PartData partData)
    {
        if (!_parts.ContainsKey(partData.Position))
            return;

        _botData.Value.ChangePart(partData.Position, partData);
    }

    [ContextMenu("Clear Bot")]
    public void ClearBotData()
    {
        foreach (var partData in _botData.Value.GetParts())
            Remove(partData.Value.Position);
    }

    public void Remove(Vector3Int partPosition)
    {
        if (partPosition == _mainPartPosition)
            return;

        _botData.Value.RemovePart(partPosition);

        //Remove SO from editor
        string assetPath = "Assets/Data/Bots/Bot1/Part" + partPosition.ToString() + ".asset";
        AssetDatabase.DeleteAsset(assetPath);
        AssetDatabase.Refresh();

        if (!_parts.ContainsKey(partPosition))
            return;

        Destroy(_parts[partPosition]);
        _parts.Remove(partPosition);
    }
}