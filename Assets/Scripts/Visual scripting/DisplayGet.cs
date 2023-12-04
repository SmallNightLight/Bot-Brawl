using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DisplayGet : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public BaseGet DefaultGet;
    [HideInInspector] public BaseGet Reference;

    [SerializeField] private GameObject _textPrefab;
    [SerializeField] private GameObject _baseGetPointPrefab;

    public bool IsDefaultNode;
    public bool IsVariable;

    [SerializeField] private Color _baseColor;
    [HideInInspector] public bool IsDragging;

    public Transform _allParent;
    public Transform _overlayParent;

    [HideInInspector] public DisplayDo MainDisplayDo;
    private GetPoint _parentPoint;
    private GetPoint _attachingPoint;

    [SerializeField] private InsertDo _insert;

    [HideInInspector] public bool IsMoving;
    private bool _inPreview;
    private bool _isSnapping;

    private Vector2 _mouseOffset;
    private Vector3 _snapPosition;
    private Vector3 _snapPositionOffset;
    
    private RectTransform _rect;
    private DisplayGet _bufferNode;

    private List<GetPoint> _childrenPoints = new List<GetPoint>();

    private void Start()
    {
        TryGetComponent(out _rect);

        if (IsVariable)
        {
            if (!IsDefaultNode)
                _inPreview = true;

            return;
        }
           

        if (IsDefaultNode)
            SetupAsDefault();
        else
        {
            _inPreview = true;
            _childrenPoints = GetComponentsInChildren<GetPoint>().Where(childComponent => childComponent.transform.parent == transform).ToList();
        }
    }

    public void InitializeAsVariable(string variableName, BaseGet variable)
    {
        IsVariable = true;
        NewText(variableName);

        DefaultGet = variable;

        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    public void SetupAsDefault()
    {
        //Create Do node text
        NewText(DefaultGet.GetNodeText());

        string[] getTexts = DefaultGet.GetBeforeNodeText();
        int i = 0;

        foreach (BaseGet defaultGet in DefaultGet.GetDefaultInput())
        {
            NewText(getTexts[i]);
            CreateGetPoint(defaultGet);
            i++;
        }
    }

    private void CreateGetPoint(BaseGet defaultGet)
    {
        GetPoint child = Instantiate(_baseGetPointPrefab, transform).GetComponent<GetPoint>();
        child.DefaultGet = defaultGet;
        _childrenPoints.Add(child);
    }

    private void Update()
    {
        if (IsDragging && Input.GetMouseButton(0))
        {
            Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            if (Input.GetMouseButtonDown(0))
            {
                _mouseOffset = new Vector2(transform.position.x, transform.position.y) - mousePosition;
                GameObject oldParent = _parentPoint?.gameObject;
                MainDisplayDo = GetDisplayDo();

                if (IsDefaultNode)
                {
                    NewPreviewGet();
                    IsDragging = false;
                    return;
                }
                else
                {
                    MoveToOverlay();
                    _inPreview = true;

                    if (_parentPoint != null)
                        _parentPoint.ChildGet = null;

                    _parentPoint = null;
                    _attachingPoint = null;

                    GetDisplayGet().UpdateGetStack();
                }

                UpdateSizes(oldParent);
                if (MainDisplayDo != null)
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(MainDisplayDo.GetComponent<RectTransform>());
                    MainDisplayDo.UpdateGetStack();
                }
                    
                if (oldParent != null)
                    LayoutRebuilder.ForceRebuildLayoutImmediate(oldParent.GetComponent<RectTransform>());

                MainDisplayDo = null;
            }

            transform.position = mousePosition + _mouseOffset;

            if (Reference == null)
                _rect.anchoredPosition -= new Vector2(_rect.rect.width * 0.5f, 0f);

            EnableMoving();
            SnapToObjects();
        }
        else if (_inPreview)
        {
            _inPreview = false;
            _insert.DisableInsert();

            if (_isSnapping)
            {
                _isSnapping = false;
                
                //Setup other object completly
                _parentPoint = _attachingPoint;
                _parentPoint.ChildGet = this;

                transform.SetParent(_parentPoint.transform);

                _attachingPoint = null;

                _rect.position = _snapPosition;
                _rect.localPosition += _snapPositionOffset;

                GetDisplayGet().UpdateGetStack();
                MainDisplayDo = GetDisplayDo();
                UpdateSizes(_parentPoint.gameObject);

                if (MainDisplayDo != null)
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(MainDisplayDo.GetComponent<RectTransform>());
                    MainDisplayDo.UpdateGetStack();
                }
                
                //Setup references
                //...
            }
            else
            {
                MoveToStandard();
                _isSnapping = false;
            }

            if (Reference == null)
                NewGet();

            DisableMoving();
        }
    }

    private void SnapToObjects()
    {
        foreach (RectTransform snappingTarget in GetPoint.ALLGETPOINTS)
        {
            if (snappingTarget == _rect)
                continue;

            GetPoint otherGetPoint = snappingTarget.gameObject.GetComponent<GetPoint>();

            if (otherGetPoint.ChildGet != null || otherGetPoint.IsMoving)
                continue;

            if (otherGetPoint.MainDo != null && otherGetPoint.MainDo.IsDefaultNode)
                continue;

            if (otherGetPoint.transform.parent.TryGetComponent(out DisplayGet localParent) && localParent.IsDefaultNode)
                continue;

            Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            if (IsPointInsideConstructedRect(snappingTarget, mousePosition))
            {
                //Set preview
                _isSnapping = true;

                if (_attachingPoint == null || _attachingPoint != otherGetPoint)
                {
                    //Set otherDo
                    _attachingPoint = otherGetPoint;

                    //Set preview
                    _insert.SetInsertGet(snappingTarget);
                    _snapPosition = snappingTarget.position;
                    _snapPositionOffset = new Vector3(0, 0, 0);
                }

                return;
            }
        }

        _isSnapping = false;
        _insert.DisableInsert();

        _attachingPoint = null;
    }

    bool IsPointInsideConstructedRect(RectTransform originalRect, Vector2 point)
    {
        float halfHeight = originalRect.rect.height * 0.5f;

        Vector3[] corners = new Vector3[4];
        originalRect.GetWorldCorners(corners);

        float minX = Mathf.Min(corners[0].x, corners[1].x, corners[2].x, corners[3].x);
        float minY = Mathf.Min(corners[0].y, corners[1].y, corners[2].y, corners[3].y + halfHeight); // Adjusted minY to consider half height
        float maxX = Mathf.Max(corners[0].x, corners[1].x, corners[2].x, corners[3].x);
        float maxY = Mathf.Max(corners[0].y, corners[1].y, corners[2].y, corners[3].y + halfHeight); // Adjusted maxY to consider half height

        return (point.x >= minX && point.x <= maxX && point.y >= minY && point.y <= maxY);
    }

    public void UpdateSizes(GameObject parentToCheck)
    {
        foreach (var child in GetComponentsInChildren<GetPoint>())
        {
            bool hasChild = child.gameObject.GetComponentInChildren<DisplayGet>() != null;
            child.gameObject.GetComponent<ContentSizeFitter>().enabled = hasChild;
            RectTransform rect = child.gameObject.GetComponent<RectTransform>();

            if (!hasChild)
                rect.sizeDelta = new Vector2(40, rect.sizeDelta.y);
        }

        if (parentToCheck != null && parentToCheck.TryGetComponent(out GetPoint parentPoint))
        {
            parentPoint.GetComponentInParent<DisplayGet>()?.UpdateSizes(null);
            parentPoint.GetComponentInParent<DisplayDo>()?.UpdateSizes();
        }

        GetComponentInParent<DisplayDo>()?.UpdateSizes();

        if (parentToCheck != null)
        {
            bool hasPadding = !transform.parent.TryGetComponent(out GetPoint point);

            if (point != null && point.transform.parent.TryGetComponent(out DisplayDo displayDo))
                hasPadding = true;

            GetComponent<HorizontalLayoutGroup>().padding.right = hasPadding ? 5 : 0;
        }

        ReloadOnChild();

        if (parentToCheck != null && parentToCheck.TryGetComponent(out GetPoint parentPoint2))
        {
            if (parentPoint2.transform.parent.TryGetComponent(out DisplayGet dG))
                dG.ReloadOnChild();
        }
    }

    public void UpdateGetStack(int height = 19, float darkeningFactor = 1f)
    {
        _rect.sizeDelta = new Vector2(_rect.sizeDelta.x, height);

        if (TryGetComponent(out Image image))
            image.color = DarkenColor(_baseColor, darkeningFactor);

        foreach (GetPoint child in _childrenPoints)
        {
            if (child.TryGetComponent(out Image pointImage))
                pointImage.color = DarkenColor(child.BaseColor, darkeningFactor);

            RectTransform childRect = child.GetComponent<RectTransform>();
            childRect.sizeDelta = new Vector2(childRect.sizeDelta.x, height);

            if (child.ChildGet != null)
                child.ChildGet.UpdateGetStack(height - 2, darkeningFactor - 0.1f);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    private Color DarkenColor(Color color, float factor)
    {
        factor = Mathf.Clamp01(factor);
        return new Color(color.r * factor, color.g * factor,color.b * factor, color.a);
    }

    public void ReloadOnChild()
    {
        var v1 = GetComponentInChildren<GetPoint>();
        if (v1 != null)
        {
            var v2 = v1.GetComponentInChildren<DisplayGet>();
            if (v2 != null)
            {
                v2.ReloadOnChild();
                return;
            }
        }

        ReloadTransform();
    }

    public void ReloadTransform()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());

        if (transform.parent.TryGetComponent(out GetPoint v1))
        {
            if (v1.transform.parent.TryGetComponent(out DisplayGet v2))
            {
                v2.ReloadTransform();
                return;
            }
        }
    }

    private DisplayDo GetDisplayDo()
    {
        return GetComponentInParent<DisplayDo>();
    }

    private DisplayGet GetDisplayGet() //return max parent
    {
        DisplayGet thisParent = transform.parent.GetComponentInParent<DisplayGet>();

        if (thisParent != null)
            return thisParent.GetDisplayGet();

        return this;
    }

    public void EnableMoving()
    {
        IsMoving = true;

        foreach(var childDisplayGet in GetComponentsInChildren<DisplayGet>())
            childDisplayGet.IsMoving = true;

        foreach (var childGetPoint in GetComponentsInChildren<GetPoint>())
            childGetPoint.IsMoving = true;
    }

    public void DisableMoving()
    {
        IsMoving = false;

        foreach (var childDisplayGet in GetComponentsInChildren<DisplayGet>())
            childDisplayGet.IsMoving = false;

        foreach (var childGetPoint in GetComponentsInChildren<GetPoint>())
            childGetPoint.IsMoving = false;
    }

    public void MoveToOverlay()
    {
        transform.SetParent(_overlayParent);
    }

    public void MoveToStandard()
    {
        transform.SetParent(_allParent);
    }

    private void NewPreviewGet()
    {
        _bufferNode = Instantiate(this, _allParent).GetComponent<DisplayGet>();
        _bufferNode.IsDragging = true;
        _bufferNode.IsDefaultNode = false;
    }

    private void NewGet()
    {
        //Only works in editor
        Reference = Instantiate(DefaultGet);
        AssetDatabase.CreateAsset(Reference, "Assets/Data/Get" + Node.ID++ + ".asset"); //NEW PATH
        AssetDatabase.SaveAssets();
    }

    private void NewText(string text)
    {
        if (text == "")
            return;

        GameObject g = Instantiate(_textPrefab, transform);
        g.GetComponent<TMP_Text>().text = text;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        IsDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsDragging = false;

        if (_bufferNode != null)
            _bufferNode.IsDragging = false;
    }
}