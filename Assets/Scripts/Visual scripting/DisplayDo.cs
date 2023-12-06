using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class DisplayDo : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public BaseDo DefaultDo;

    public GameObject Base;
    [SerializeField] private GameObject _textPrefab;
    [SerializeField] List<GameObject> _baseGetPointPrefab = new List<GameObject>();
    [SerializeField] private GameObject _scopePrefab;

    [SerializeField] private InsertDo _insert;
    private Vector2 _insertScale;

    [SerializeField] private Vector2 _snappingOffset;
    [SerializeField] private Vector2 _snappingOffsetScope;

    [HideInInspector] public DisplayDo PreviousDo;
    [HideInInspector] public DisplayDo NextDo;
    [HideInInspector] public DisplayDo PreviousScopedDo;
    [HideInInspector] public DisplayDo NextScopedDo;

    public bool IsDefaultNode;
    public bool IsFunctionNode;

    [SerializeField] private Color _baseColor;

    [HideInInspector] public bool IsDragging;

    public Transform _allParent;
    public Transform _overlayParent;

    private Vector2 _mouseOffset;
    private DisplayDo _bufferNode;
    private RectTransform _rect;

    private static List<RectTransform> ALLNODES = new List<RectTransform>();

    [HideInInspector] public bool IsMoving;
    private bool _inPreview;
    private bool _isSnapping;
    private bool _isFirst;

    private Vector3 _snapPosition;
    private Vector3 _snapPositionOffset;

    private DisplayDo _attachmentPrevious;
    private DisplayDo _attachmentNext;

    private DisplayDo _attachmentPreviousScope;
    private DisplayDo _attachmentNextScope;

    private SnapType _snapType;

    public List<GetPoint> ChildrenPoints = new List<GetPoint>();

    [HideInInspector] public bool HasScope;
    private GameObject _scopeObject;

    private enum SnapType
    {
        Top,
        Insert,
        Under,
        UnderAsScope,
        InsertScope
    }

    private void Start()
    {
        HasScope = DefaultDo.HasScope();

        if (!IsDefaultNode && TryGetComponent(out _rect))
            ALLNODES.Add(_rect);

        //Change when dragging
        if (IsDefaultNode)
            SetupAsDefault();
        else
        {
            _inPreview = true;
            _isFirst = true;

            ChildrenPoints = Base.GetComponentsInChildren<GetPoint>().Where(childComponent => childComponent.transform.parent == Base.transform).ToList();

            if (IsFunctionNode)
                DataManager.Instance.FunctionsToCompile.Add(this);
            else
                DataManager.Instance.DosToCompile.Add(this);

            if (HasScope && !IsFunctionNode)
                _scopeObject = Instantiate(_scopePrefab, transform);

            foreach (Transform child in Base.transform)
                LayoutRebuilder.ForceRebuildLayoutImmediate(child.GetComponent<RectTransform>());

            LayoutRebuilder.ForceRebuildLayoutImmediate(Base.GetComponent<RectTransform>());
        }

        _insertScale = _insert.GetComponent<RectTransform>().sizeDelta;
    }

    private void OnDestroy()
    {
        if (IsFunctionNode)
            DataManager.Instance.FunctionsToCompile.Remove(this);
        else
            DataManager.Instance.DosToCompile.Remove(this);

        ALLNODES.Remove(_rect);
    }

    public void SetupAsDefault()
    {
        //Create Do node text
        NewText(DefaultDo.GetNodeText());

        string[] getTexts = DefaultDo.GetBeforeNodeText();
        int i = 0;

        foreach (BaseGet defaultGet in DefaultDo.GetDefaultInput())
        {
            NewText(getTexts[i]);
            CreateGetPoint(defaultGet, i);

            i++;
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(Base.GetComponent<RectTransform>());
    }   

    private void CreateGetPoint(BaseGet defaultGet, int index)
    {
        GetPoint childGet = Instantiate(_baseGetPointPrefab[index], Base.transform).GetComponent<GetPoint>();
        childGet.DefaultGet = defaultGet;
        ChildrenPoints.Add(childGet);

        LayoutRebuilder.ForceRebuildLayoutImmediate(childGet.GetComponent<RectTransform>());
    }

    private void NewText(string text)
    {
        if (text == "")
            return;

        GameObject newText = Instantiate(_textPrefab, Base.transform);
        newText.GetComponent<TMP_Text>().text = text;

        LayoutRebuilder.ForceRebuildLayoutImmediate(newText.GetComponent<RectTransform>());
    }

    private void Update()
    {
        if (IsDragging && Input.GetMouseButton(0))
        {
            Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            if (Input.GetMouseButtonDown(0))
            {
                _mouseOffset = new Vector2(transform.position.x, transform.position.y) - mousePosition;

                if (IsDefaultNode)
                {
                    NewPreviewDo();
                    IsDragging = false;
                    return;
                }
                else
                {
                    MoveToOverlay();
                    _inPreview = true;
                    _attachmentPrevious = null;
                    _attachmentNext = null;
                    _attachmentPreviousScope = null;
                    _attachmentNextScope = null;

                    if (PreviousScopedDo != null)
                    {
                        PreviousScopedDo.NextScopedDo = null;
                        UpdateScope();
                        PreviousScopedDo = null;
                    }

                    if (PreviousDo != null)
                    {
                        PreviousDo.NextDo = null;
                        PreviousDo.UpdateScope();
                        PreviousDo = null;
                    }

                    UpdateScope();
                }
            }

            transform.position = mousePosition + _mouseOffset;

            if (_isFirst)
                _rect.anchoredPosition -= new Vector2(_rect.rect.width * 0.5f, 0f);

            MoveChild();
            SnapToObjects();
        }
        else if (_inPreview)
        {
            _inPreview = false;
            _insert.DisableInsert();
            MoveToStandard();

            if (_isSnapping)
            {
                _isSnapping = false;

                //Setup other object completly
                PreviousDo = _attachmentPrevious;

                if (_snapType == SnapType.Top)
                {
                    DisplayDo lastDo = GetLastNextDo();

                    if (lastDo == this)
                    {
                        NextDo = _attachmentNext;
                        NextDo.PreviousDo = this;
                    }
                    else
                    {
                        lastDo.NextDo = _attachmentNext;
                        _attachmentNext.PreviousDo = lastDo;
                    }
                }
                else if (_snapType == SnapType.Under)
                {
                    PreviousDo = _attachmentPrevious;
                    PreviousDo.NextDo = this;  
                }
                else if (_snapType == SnapType.Insert)
                {
                    DisplayDo lastDo = GetLastNextDo();

                    if (lastDo == this)
                    {
                        NextDo = _attachmentNext;
                        NextDo.PreviousDo = this;
                    }
                    else
                    {
                        lastDo.NextDo = _attachmentNext;
                        _attachmentNext.PreviousDo = lastDo;
                    }

                    PreviousDo = _attachmentPrevious;
                    PreviousDo.NextDo = this;
                }
                else if (_snapType == SnapType.UnderAsScope)
                {
                    PreviousScopedDo = _attachmentPreviousScope;
                    PreviousScopedDo.NextScopedDo = this;
                }
                else if (_snapType == SnapType.InsertScope)
                {
                    DisplayDo lastDo = GetLastNextDo();

                    if (lastDo == this)
                    {
                        NextDo = _attachmentNext;
                        NextDo.PreviousDo = this;
                        NextDo.PreviousScopedDo = null;
                    }
                    else
                    {
                        lastDo.NextDo = _attachmentNext;
                        lastDo.NextDo.PreviousDo = lastDo;
                        lastDo.NextDo.PreviousScopedDo = null;
                    }

                    PreviousScopedDo = _attachmentPreviousScope;
                    PreviousScopedDo.NextScopedDo = this;
                }

                _attachmentPrevious = null;
                _attachmentNext = null;
                _attachmentNextScope = null;
                _attachmentPreviousScope = null;

                _rect.position = _snapPosition;
                if (_snapType != SnapType.UnderAsScope && _snapType != SnapType.InsertScope)
                    _rect.localPosition += _snapPositionOffset;
                else
                    _rect.localPosition += new Vector3(_snappingOffsetScope.x, _snappingOffsetScope.y, 0);

                UpdateScope();
            }
            else
            {
                _isSnapping = false;
            }

            _isFirst = false;
            DisableMoving();
        }
    }

    public float GetHeightOfBlock(bool includeNextDos)
    {
        float height = 0;

        if (HasScope)
        {
            height += 33;

            if (NextScopedDo != null)
                height += NextScopedDo.GetHeightOfBlock(true);
            else
                height += 20; //Default space if nothing inside
        }
        else
        {
            height += 20;
        }

        if (includeNextDos && NextDo != null)
            height += NextDo.GetHeightOfBlock(true);

        return height;
    }

    public void UpdateScope()
    {
        if (HasScope)
        {
            RectTransform _scopedRect = _scopeObject.GetComponent<RectTransform>();
            _scopedRect.sizeDelta = new Vector2(_scopedRect.sizeDelta.x, GetHeightOfBlock(false) - 30);
        }

        if (PreviousScopedDo != null)
            PreviousScopedDo.UpdateScope();
        
        if (PreviousDo != null)
            PreviousDo.UpdateScope();

        MoveChild(true);
    }

    public GameObject GetScopeObject()
    {
        return _scopeObject;
    }

    private void SnapToObjects()
    {
        foreach (RectTransform snappingTarget in ALLNODES)
        {
            if (snappingTarget == _rect)
                continue;

            DisplayDo otherDo = snappingTarget.gameObject.GetComponent<DisplayDo>();

            if (otherDo.IsMoving)
                continue;

            Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            
            if (DefaultDo.CanSnapAbove() && otherDo.DefaultDo.CanSnapUnder())
            {
                Vector2 pointToTest = mousePosition - new Vector2(0, snappingTarget.sizeDelta.y * 2);

                if (IsPointInsideConstructedRect(snappingTarget, pointToTest, otherDo.Base.GetComponent<RectTransform>().sizeDelta.x))
                {
                    //Set preview
                    _isSnapping = true;

                    if (otherDo.PreviousScopedDo != null)
                    {
                        //Insert scope

                        if (_attachmentPreviousScope == null || _attachmentPreviousScope != otherDo)
                        {
                            _isSnapping = false;
                            continue;
                        }
                    }
                    else if (_attachmentNext == null || _attachmentNext != otherDo)
                    {
                        //Set otherDo
                        _attachmentNext = otherDo;

                        //Set preview
                        _insert.Rect.position = snappingTarget.position;
                        _insert.EnableInsert();

                        _snapPosition = snappingTarget.position;
                        _snapPositionOffset = new Vector3(_snappingOffset.x, _snappingOffset.y, 0);

                        if (HasScope)
                            _snapPositionOffset += new Vector3(0, _scopeObject.GetComponent<RectTransform>().sizeDelta.y + 10, 0);

                        if (otherDo.PreviousDo != null)
                        {
                            //Skip this, the next check will probably catch an insert
                            _isSnapping = false;
                            continue;
                        }
                        else
                        {
                            _snapType = SnapType.Top;
                            _attachmentPrevious = null;
                            _insert.Rect.sizeDelta = _insertScale;
                        }
                    }

                    return;
                }
            }

            if (DefaultDo.CanSnapUnder() && otherDo.DefaultDo.CanSnapAbove())
            {
                Vector2 pointToTest = mousePosition + new Vector2(0, snappingTarget.sizeDelta.y);

                if (otherDo.HasScope)
                    pointToTest -= new Vector2(10, 0);


                if (IsPointInsideConstructedRect(snappingTarget, pointToTest, otherDo.Base.GetComponent<RectTransform>().sizeDelta.x))
                {
                    //Set preview 
                    _isSnapping = true;

                    if (!otherDo.HasScope)
                    {
                        if (_attachmentPrevious == null || _attachmentPrevious != otherDo)
                        {
                            //Set otherDo
                            _attachmentPrevious = otherDo;

                            //Set preview
                            _insert.Rect.position = snappingTarget.position;
                            _insert.EnableInsert();

                            _snapPosition = snappingTarget.position;
                            _snapPositionOffset = -new Vector3(_snappingOffset.x, _snappingOffset.y, 0);

                            if (otherDo.NextDo != null)
                            {
                                _snapType = SnapType.Insert;
                                _attachmentNext = otherDo.NextDo;

                                _insert.Rect.localPosition += _snapPositionOffset / 2;
                                _insert.Rect.sizeDelta = new Vector2(snappingTarget.sizeDelta.x, 5);
                            }
                            else
                            {
                                _snapType = SnapType.Under;
                                _attachmentNext = null;

                                _insert.Rect.localPosition += new Vector3(0, -40, 0);
                                _insert.Rect.sizeDelta = _insertScale;
                            }
                        }
                    }
                    else
                    {
                        //Scope

                        if (_attachmentPreviousScope == null || _attachmentPreviousScope != otherDo)
                        {
                            _attachmentPreviousScope = otherDo;

                            //Set preview
                            _insert.Rect.position = snappingTarget.position;
                            _insert.EnableInsert();

                            _snapPosition = snappingTarget.position;
 
                            if (otherDo.NextScopedDo != null)
                            {
                                _snapType = SnapType.InsertScope;
                                _attachmentNext = otherDo.NextScopedDo;

                                _insert.Rect.localPosition += new Vector3(10, -_snappingOffset.y / 2, 0);
                                _insert.Rect.sizeDelta = new Vector2(_rect.sizeDelta.x, 5);
                            }
                            else
                            {
                                _snapType = SnapType.UnderAsScope;
                                _attachmentNext = null;

                                _insert.Rect.localPosition += new Vector3(4, -40, 0);
                                _insert.Rect.sizeDelta = _insertScale;
                            }
                        }
                    }

                    return;
                }
            }
        }

        foreach (RectTransform snappingTarget in ScopeEnd.ALLSCOPEENDS)
        {
            if (snappingTarget == _rect)
                continue;

            DisplayDo otherDo = snappingTarget.gameObject.GetComponent<ScopeEnd>().ParentDisplayDo;

            if (otherDo.IsMoving)
                continue;

            Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            if (DefaultDo.CanSnapUnder() && otherDo.DefaultDo.CanSnapAbove())
            {
                Vector2 pointToTest = mousePosition + new Vector2(0, snappingTarget.sizeDelta.y);

                if (IsPointInsideConstructedRect(snappingTarget, pointToTest, 50))
                {
                    //Set preview 
                    _isSnapping = true;

                    if (_attachmentPrevious == null || _attachmentPrevious != otherDo)
                    {
                        //Set otherDo
                        _attachmentPrevious = otherDo;

                        //Set preview
                        _insert.Rect.position = snappingTarget.position;
                        _insert.EnableInsert();

                        _snapPosition = snappingTarget.position;
                        _snapPositionOffset = -new Vector3(0, 10, 0);

                        if (otherDo.NextDo != null)
                        {
                            _snapType = SnapType.Insert;
                            _attachmentNext = otherDo.NextDo;

                            _insert.Rect.localPosition += _snapPositionOffset / 2;
                            _insert.Rect.sizeDelta = new Vector2(snappingTarget.sizeDelta.x, 5);
                        }
                        else
                        {
                            _snapType = SnapType.Under;
                            _attachmentNext = null;

                            _insert.Rect.localPosition += new Vector3(0, -33);
                            _insert.Rect.sizeDelta = _insertScale;
                        }
                    }

                    return;
                }
            }
        }

        _isSnapping = false;
        _insert.DisableInsert();

        _attachmentPrevious = null;
        _attachmentNext = null;
        _attachmentPreviousScope = null;
        _attachmentNextScope = null;
    }

    //public DisplayDo GetBasePreviousScope() //Do this to have different colors on DIsplayDO elements
    //{
    //    DisplayDo localPreviousScope = GetPreviousScope();

    //    if (localPreviousScope != null)
    //    {
    //        return localPreviousScope.GetBasePreviousScope();
    //    }
    //    else
    //        return this;
    //}

    //public void ChangeColor()
    //{
    //    if (TryGetComponent(out Image image))
    //        image.color = DarkenColor(_baseColor, darkeningFactor);

    //}

    public void UpdateSizes()
    {
        foreach (var child in GetComponentsInChildren<GetPoint>())
        {
            bool hasChild = child.gameObject.GetComponentInChildren<DisplayGet>() != null;
            child.gameObject.GetComponent<ContentSizeFitter>().enabled = hasChild;
            RectTransform rect = child.gameObject.GetComponent<RectTransform>();

            if (!hasChild)
                rect.sizeDelta = new Vector2(40, rect.sizeDelta.y);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    public void UpdateGetStack()
    {
        foreach (var child in ChildrenPoints)
            child.ChildGet?.UpdateGetStack();

        foreach (Transform child in Base.transform)
            LayoutRebuilder.ForceRebuildLayoutImmediate(child.GetComponent<RectTransform>());

        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    bool IsPointInsideConstructedRect(RectTransform originalRect, Vector2 point, float addedWidth = 0)
    {
        float halfHeight = originalRect.rect.height * 0.5f;

        Vector3[] corners = new Vector3[4];
        originalRect.GetWorldCorners(corners);

        float minX = Mathf.Min(corners[0].x, corners[1].x, corners[2].x, corners[3].x);
        float minY = Mathf.Min(corners[0].y, corners[1].y, corners[2].y, corners[3].y + halfHeight); // Adjusted minY to consider half height
        float maxX = Mathf.Max(corners[0].x, corners[1].x, corners[2].x, corners[3].x + addedWidth);
        float maxY = Mathf.Max(corners[0].y, corners[1].y, corners[2].y, corners[3].y + halfHeight); // Adjusted maxY to consider half height

        return (point.x >= minX && point.x <= maxX && point.y >= minY && point.y <= maxY);
    }

    public void Move(Vector3 position, Vector3 offset, float yOffset, bool disable)
    {
        if (yOffset != 0)
            yOffset += 10;

        transform.position = new Vector2(position.x, position.y);
        transform.localPosition -= new Vector3(offset.x, offset.y + yOffset, 0);

        MoveChild(disable);
    }

    public void MoveChild(bool disable = false)
    {
        IsMoving = true;

        if (NextScopedDo != null)
            NextScopedDo.Move(_rect.position, -_snappingOffsetScope, 0, disable);

        if (NextDo != null)
            NextDo.Move(_rect.position, _snappingOffset, GetYOffset(), disable);

        if (disable)
            IsMoving = false;
    }

    public DisplayDo GetPreviousScope()
    {
        if (PreviousScopedDo != null)
            return PreviousScopedDo;

        if (PreviousDo != null)
            return PreviousDo.GetPreviousScope();

        return null;
    }

    private float GetYOffset()
    {
        if (HasScope)
            return _scopeObject.GetComponent<RectTransform>().sizeDelta.y;

        return 0f;
    }

    private Color DarkenColor(Color color, float factor)
    {
        factor = Mathf.Clamp01(factor);
        return new Color(color.r * factor, color.g * factor, color.b * factor, color.a);
    }

    public void MoveToOverlay()
    {
        transform.SetParent(_overlayParent);

        if (NextScopedDo != null)
            NextScopedDo.MoveToOverlay();

        if (NextDo != null)
            NextDo.MoveToOverlay();
    }

    public void MoveToStandard()
    {
        transform.SetParent(_allParent);

        if (NextScopedDo != null)
            NextScopedDo.MoveToStandard();

        if (NextDo != null)
            NextDo.MoveToStandard();
    }

    public DisplayDo GetLastNextDo()
    {
        if (NextDo == null)
            return this;
        else
            return NextDo.GetLastNextDo();
    }

    public DisplayDo GetLastNextDoScoped()
    {
        if (NextScopedDo == null)
            return this;
        else
            return NextScopedDo.GetLastNextDoScoped();
    }

    public void DisableMoving()
    {
        IsMoving = false;

        if (NextDo != null)
            NextDo.DisableMoving();

        if (NextScopedDo != null)
            NextScopedDo.DisableMoving();
    }

    private void NewPreviewDo()
    {
        _bufferNode = Instantiate(this, transform.parent.transform).GetComponent<DisplayDo>();
        _bufferNode.IsDragging = true;
        _bufferNode.IsDefaultNode = false;
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