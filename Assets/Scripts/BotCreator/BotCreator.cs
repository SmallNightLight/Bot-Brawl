using ScriptableArchitecture.Data;
using System.Collections.Generic;
using System.Data;
using UnityEditor;
using UnityEngine;

public class BotCreator : MonoBehaviour
{
    [SerializeField] private BotDataReference _botData;
    [SerializeField] private TransformReference _cameraTarget;

    [SerializeField] private BasePartDataReference _mainPartData;
    [SerializeField] private BoolReference _isPlacing;

    [SerializeField] private Material _previewMaterial;
    [SerializeField] private Material _unplacableMaterial;

    private Dictionary<Vector3Int, GameObject> _parts = new Dictionary<Vector3Int, GameObject>();
    private BasePartDataReference _selectedBasePart;
    private Vector3Int _mainPartPosition = new Vector3Int(0, 5, 0);

    private GameObject _previewObject;

    private void Start()
    {
        SetupBot();
    }

    private void Update()
    {
        //In early script execution order

        if (_previewObject != null)
            Destroy(_previewObject);
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
                if (part.Value != null)
                    AddUnit(part.Value, part.Value.BasePart.Value.Equals(_mainPartData.Value));
        }

        _isPlacing.Value = false;
    }

    public void PlacePart(PlacingInfo placingInfo)
    {
        if (_isPlacing.Value && CanPlaceUnit(placingInfo))
            PlacePart(placingInfo, false);
    }

    private bool CanPlaceUnit(PlacingInfo placingInfo)
    {
        if (_selectedBasePart == null)
            return false;

        List<Vector3Int> otherAttachmentPoints = placingInfo.OtherPart.BasePart.Value.RelativeAttachmentPoints;
        Vector3 localNormal = Quaternion.Euler(placingInfo.OtherPart.Rotation) * placingInfo.Normal;

        for (int i = 0; i < otherAttachmentPoints.Count; i++)
            if (localNormal == -otherAttachmentPoints[i])
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

        Unit unitComponent = newUnit.AddComponent<Unit>();
        unitComponent.UnitPartData = partData;

        _parts[partData.Position] = newUnit;

        if (asCameraTarget)
            _cameraTarget.Value = newUnit.transform;
    }

    public void PreviewPartInfo(PlacingInfo placingInfo)
    {
        PartData newPartData = Instantiate(_selectedBasePart.Value.DefaultData);
        newPartData.BasePart = _selectedBasePart;
        newPartData.Position = placingInfo.AttachPoint;

        newPartData.Rotation = Quaternion.FromToRotation(newPartData.BasePart.Value.DefaultAttachmentDirection, placingInfo.Normal).eulerAngles;

        _previewObject = Instantiate(newPartData.BasePart.Value.CreatorPrefab, newPartData.Position, Quaternion.Euler(newPartData.Rotation.x, newPartData.Rotation.y, newPartData.Rotation.z));
        _previewObject.layer = LayerMask.NameToLayer("Default");

        bool canBePlaced = CanPlaceUnit(placingInfo);

        if (_previewObject.TryGetComponent(out MeshRenderer renderer))
            SetTransparent(renderer, canBePlaced);

        foreach(MeshRenderer childRenderer in _previewObject.GetComponentsInChildren<MeshRenderer>())
            SetTransparent(childRenderer, canBePlaced);
    }

    private void SetTransparent(MeshRenderer renderer, bool canBePlaced)
    {
        if (canBePlaced)
            renderer.material = _previewMaterial;
        else
            renderer.material = _unplacableMaterial;
    }

    public void SetSelectionBasePartData(BasePartDataReference data)
    {
        _selectedBasePart = data;
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