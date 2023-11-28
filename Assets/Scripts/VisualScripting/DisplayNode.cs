using ScriptableArchitecture.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class DisplayNode : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Node _node;
    [SerializeField] private bool IsDefaultNode;

    private bool _isDragging;
    private Vector2 _offset;
    private float _snappingValue = 10;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (_isDragging)
        {
            if (Input.GetMouseButton(0))
            {
                Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

                if (Input.GetMouseButtonDown(0))
                {
                    _offset = new Vector2(transform.position.x, transform.position.y) - mousePosition;

                    if (IsDefaultNode)
                    {
                        NewNode();
                        _isDragging = false;
                        return;
                    }
                }

                transform.position = SnapPosition(mousePosition + _offset);
            }
        }
    }

    private void NewNode()
    {
        Node newPartData = Instantiate(_node);

        //Only works in editor
        AssetDatabase.CreateAsset(newPartData, "Assets/Data/Bots/Bot1/Behavior/Node" + Node.ID++ + ".asset");
        AssetDatabase.SaveAssets();

        Instantiate(this, transform.parent.transform).GetComponent<DisplayNode>().EnableDragging();
    }

    private Vector2 SnapPosition(Vector2 position)
    {
        float snappedX = Mathf.Round(position.x / _snappingValue) * _snappingValue;
        float snappedY = Mathf.Round(position.y / _snappingValue) * _snappingValue;

        return new Vector2(snappedX, snappedY);
    }

    public void EnableDragging()
    {
        _isDragging = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isDragging = false;
    }
}