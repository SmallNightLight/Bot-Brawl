using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonToggle : MonoBehaviour
{
    [SerializeField] private Sprite[] buttonSprites;
    [SerializeField] private Image targetButton;
    public bool IsActive;

    private static List<ButtonToggle> activeImages = new List<ButtonToggle>();

    private void Start()
    {
        activeImages.Add(this);
    }

    private void OnDestroy()
    {
        activeImages.Remove(this);
    }

    public void ChangeSprite()
    {
        if (IsActive)
        {
            foreach (var v in activeImages)
            {
                if (!v.IsActive)
                    continue;

                if (v == this)
                {
                    if (targetButton.sprite == buttonSprites[1])
                        Disable();
                    else
                        Enable();
                }
                else
                {
                    v.Disable();
                }
            }


        }
        else
        {
            if (targetButton.sprite == buttonSprites[0])
            {
                activeImages.Add(this);
                targetButton.sprite = buttonSprites[1];
                return;
            }

            Disable();
        }
    }

    public void Disable()
    {
        targetButton.sprite = buttonSprites[0];
    }

    public void Enable()
    {
        targetButton.sprite = buttonSprites[1];
    }
}
