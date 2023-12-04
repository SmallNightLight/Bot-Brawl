using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ScriptableArchitecture.Data;
using System.IO;

public class DisplayBot : MonoBehaviour
{
    public BotData MyBotData;

    [SerializeField] private TMP_Text _name;
    [SerializeField] private RawImage _image;

    public void Initialize(BotData botData)
    {
        MyBotData = botData;

        _name.text = MyBotData.BotName;
        
        if (LoadImage(out Texture texture))
            _image.texture = texture;
    }

    private bool LoadImage(out Texture image)
    {
        string folderPath = Application.persistentDataPath + "/Images/";
        string imagePath = Path.Combine(folderPath, MyBotData.BotName + ".png");

        if (File.Exists(imagePath))
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);

            Vector2 size = _image.GetComponent<RectTransform>().sizeDelta;

            Texture2D texture = new Texture2D(Mathf.RoundToInt(size.x), Mathf.RoundToInt(size.y));

            if (texture.LoadImage(imageBytes))
            {
                image = texture;
                return true;
            }
            else
            {
                Debug.Log("Failed to load image data into Texture2D.");
            }
        }
        else
        {
            Debug.Log("Image file not found: " + imagePath);
        }

        image = null;
        return false;
    }
}