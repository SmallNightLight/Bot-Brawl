using ScriptableArchitecture.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScroll : MonoBehaviour
{
    public RectTransform content;
    public float scrollSpeed = 5f;
    public float zoomSpeed = 2f;
    public float minZoom = 0.5f;
    public float maxZoom = 2f;
    public float zoomLerpSpeed = 5f;

    private bool isDragging = false;
    private Vector2 dragStartPosition;
    private Vector2 contentStartPosition;

    private float targetZoom = 1f;

    [SerializeField] private BoolReference _isOn;

    void Update()
    {
        if (_isOn.Value)
            HandleMouseInput();
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(2)) // Middle mouse button clicked
        {
            isDragging = true;
            dragStartPosition = Input.mousePosition;
            contentStartPosition = content.anchoredPosition;
        }

        if (Input.GetMouseButtonUp(2))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector2 dragDelta = (Vector2)Input.mousePosition - dragStartPosition;
            Vector2 newContentPosition = contentStartPosition + dragDelta / scrollSpeed;
            content.anchoredPosition = newContentPosition;
        }
    }
}