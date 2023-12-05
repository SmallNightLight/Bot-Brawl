using ScriptableArchitecture.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class BotCreator : MonoBehaviour
{
    private BotData _botData;
    [SerializeField] private TransformReference _cameraTarget;

    [SerializeField] private BasePartDataReference _mainPartData;
    [SerializeField] private BoolReference _isPlacing;

    [SerializeField] private Material _previewMaterial;
    [SerializeField] private Material _unplacableMaterial;
    [SerializeField] private Material _selectedMaterial;

    [SerializeField] private List<Canvas> _canvases = new List<Canvas>();

    private Dictionary<Vector3Int, GameObject> _parts = new Dictionary<Vector3Int, GameObject>();
    private BasePartDataReference _selectedBasePart;
    private Vector3Int _mainPartPosition = new Vector3Int(0, 5, 0);

    private GameObject _previewObject;

    private PartData _selectedPartNew;

    [SerializeField] private PartDataGameEvent _selectedPartData;
    private List<MeshRenderer> _selectedRenderers = new List<MeshRenderer>();

    private void Start()
    {
        _botData = DataManager.Instance.CurrentBotData;

        if (_botData == null)
            return;

        SetupBot();
    }

    private void Update()
    {
        //In early script execution order

        if (_previewObject != null)
            Destroy(_previewObject);
        HandleRotationInput();
    }
    private void HandleRotationInput()
    {
        if (_selectedPartNew == null) return;
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Rotation" + _selectedPartNew.Rotation);
            GameObject selectedUnit = _parts[_selectedPartNew.Position];
            Debug.Log("Rotation GO" + selectedUnit.transform.rotation);
        }
        if (Input.GetMouseButton(1) && Input.GetAxis("Mouse ScrollWheel") > 0f) RotateSelectedPart(90f);
        else if (Input.GetMouseButton(1) && Input.GetAxis("Mouse ScrollWheel") < 0f) RotateSelectedPart(-90f);
    }

    private void RotateSelectedPart(float angle)
    {
        if (_selectedPartNew != null && _parts.ContainsKey(_selectedPartNew.Position))
        {
            GameObject selectedUnit = _parts[_selectedPartNew.Position];
            selectedUnit.transform.Rotate(Vector3.up, angle, Space.Self);
            _selectedPartNew.Rotation = selectedUnit.transform.eulerAngles;
        }
    }
    
    private void SetupBot()
    {
        if (_botData.GetPartCount() == 0)
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
            foreach (var part in _botData.GetParts())
                if (part.Value != null)
                    AddUnit(part.Value, part.Value.BasePart.Value.Equals(_mainPartData.Value));
        }

        _isPlacing.Value = false;
        StartCoroutine(WaitForScreenshot(false));
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

    private void PlacePart(PlacingInfo placingInfo, bool isMainBlock)
    {
        PartData newPartData = Instantiate(_selectedBasePart.Value.DefaultData);
        newPartData.BasePart = _selectedBasePart;
        newPartData.CustomName = _selectedBasePart.Value.PartName + UnityEngine.Random.Range(0, int.MaxValue).ToString();
        newPartData.Position = placingInfo.AttachPoint;

        newPartData.Rotation = Quaternion.FromToRotation(newPartData.BasePart.Value.DefaultAttachmentDirection, placingInfo.Normal).eulerAngles;

        AddPart(newPartData, isMainBlock);
    }

    private void AddPart(PartData partData, bool isMainBlock = false)
    {
        if (_parts.ContainsKey(partData.Position))
            return;

        _botData.AddPart(partData.Position, partData);
        AddUnit(partData, isMainBlock);
    }

    private void AddUnit(PartData partData, bool isMainBlock = false)
    {
        GameObject newUnit = Instantiate(partData.BasePart.Value.CreatorPrefab, partData.Position, Quaternion.Euler(partData.Rotation.x, partData.Rotation.y, partData.Rotation.z));

        Unit unitComponent = newUnit.AddComponent<Unit>();
        unitComponent.UnitPartData = partData;

        _parts[partData.Position] = newUnit;

        if (isMainBlock)
        {
            _selectedPartData.Raise(partData);
            _cameraTarget.Value = newUnit.transform;
        }
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

    public void SelectUnit(PartData partData)
    {
        Deselect();

        GameObject selectedParent = _parts[partData.Position];

        _selectedPartNew = partData;

        if (selectedParent.TryGetComponent(out MeshRenderer renderer))
            SetSelected(renderer);

        foreach (MeshRenderer childRenderer in selectedParent.GetComponentsInChildren<MeshRenderer>())
            SetSelected(childRenderer);
    }

    private void SetTransparent(MeshRenderer renderer, bool canBePlaced)
    {
        if (canBePlaced)
            renderer.material = _previewMaterial;
        else
            renderer.material = _unplacableMaterial;
    }

    private void SetSelected(MeshRenderer renderer)
    {
        _selectedRenderers.Add(renderer);

        Material[] currentMaterials = renderer.materials;
        Material[] newMaterials = new Material[currentMaterials.Length + 1];

        for (int i = 0; i < currentMaterials.Length; i++)
            newMaterials[i] = currentMaterials[i];
        newMaterials[newMaterials.Length - 1] = _selectedMaterial;

        renderer.materials = newMaterials;
    }

    public void Deselect()
    {
        for(int i = _selectedRenderers.Count - 1; i >= 0; i--)
        {
            MeshRenderer renderer = _selectedRenderers[i];

            if (renderer == null)
            {
                _selectedRenderers.Remove(renderer);
                continue;
            }
            RemoveSelectedMaterial(renderer);
        }

        _selectedRenderers.Clear();
    }

    private void RemoveSelectedMaterial(MeshRenderer renderer)
    {
        Material[] currentMaterials = renderer.sharedMaterials;

        if (currentMaterials.Any(mat => ReferenceEquals(mat, _selectedMaterial)))
        {
            Material[] newMaterials = new Material[currentMaterials.Length - 1];
            int newIndex = 0;

            for (int i = 0; i < currentMaterials.Length; i++)
            {
                if (currentMaterials[i] != _selectedMaterial)
                {
                    newMaterials[newIndex] = currentMaterials[i];
                    newIndex++;
                }
            }

            renderer.materials = newMaterials;
        }
    }

    public void SetSelectionBasePartData(BasePartDataReference data)
    {
        _selectedBasePart = data;
    }

    public void Change(PartData partData)
    {
        if (!_parts.ContainsKey(partData.Position))
            return;

        _botData.ChangePart(partData.Position, partData);
    }

    [ContextMenu("Clear Bot")]
    public void ClearBotData()
    {
        foreach (var partData in _botData.GetParts())
            Remove(partData.Value.Position);
    }

    public void RemoveSelected() //Make into one that removes as state
    {
        if (_selectedPartNew != null)
            Remove(_selectedPartNew.Position);
    }

    public void Remove(Vector3Int partPosition)
    {
        if (partPosition == _mainPartPosition)
            return;

        _botData.RemovePart(partPosition);
        if (!_parts.ContainsKey(partPosition))
            return;

        Destroy(_parts[partPosition]);
        _parts.Remove(partPosition);
    }

    [ContextMenu("Screenshot")]
    public void TakeScreenshot()
    {
        StartCoroutine(WaitForScreenshot(true));
    }

    private IEnumerator WaitForScreenshot(bool alwaysCapture)
    {
        foreach (Canvas canvas in _canvases)
            canvas.gameObject.SetActive(false);

        yield return new WaitForEndOfFrame();

        string botName = _botData.BotName;
        int width = Screen.width;
        int height = Screen.height;

        Texture2D screenshotTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(0, 0, width, height);
        screenshotTexture.ReadPixels(rect, 0, 0);
        screenshotTexture.Apply();

        byte[] byteArray = screenshotTexture.EncodeToPNG();

        string folderPath = Application.persistentDataPath + "/Images/";
        
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string filePath = Path.Combine(folderPath, botName + ".png");

        if (alwaysCapture || !File.Exists(filePath))
            File.WriteAllBytes(filePath, byteArray);

        foreach (Canvas canvas in _canvases)
            canvas.gameObject.SetActive(true);
    }
}