using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InsertDo : MonoBehaviour
{
    [HideInInspector] public RectTransform Rect;
    private Image _image;

    private void Start()
    {
        Rect = GetComponent<RectTransform>();
        _image = GetComponent<Image>();

        DisableInsert();
    }

    public void SetInsert(RectTransform otherRect, bool under)
    {
        Rect.sizeDelta = new Vector2(otherRect.rect.width, Rect.sizeDelta.y);
        Rect.position = otherRect.position;

        if(under)
            Rect.localPosition += new Vector3(0, otherRect.rect.height / 2, 0);
        else
            Rect.localPosition -= new Vector3(0, otherRect.rect.height / 2, 0);

        _image.enabled = true;
    }

    public void SetInsertGet(RectTransform otherRect, Vector3 offset)
    {
        Rect.sizeDelta = new Vector2(otherRect.rect.width, Rect.sizeDelta.y);
        Rect.position = otherRect.position;
        Rect.localPosition += offset;

        _image.enabled = true;
    }

    public void EnableInsert()
    {
        _image.enabled = true;
    }

    public void DisableInsert()
    {
        _image.enabled = false;
    }
}