
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualBackground : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private List<Sprite> _objectSprites = new List<Sprite>();
    [SerializeField] private Vector2 _speed;

    [SerializeField] private Vector2Int _size = new Vector2Int(100, 100);
    [SerializeField] private Vector2 _bounds;

    private Dictionary<GameObject, float> _currentObjects = new Dictionary<GameObject, float>();

    private int xCount, yCount;

    private void Start()
    {
        xCount = yCount = 0;

        Dictionary<int, float> speeds = new Dictionary<int, float>();

        for (int i = 0; i < _bounds.x; i += _size.x, xCount++)
        {
            for (int j = 0, h = 0; j < _bounds.y; j += _size.y, yCount += i == 0 ? 1 : 0, h++)
            {
                GameObject obj = Instantiate(_prefab, transform);
                obj.GetComponent<Image>().sprite = _objectSprites[Random.Range(0, _objectSprites.Count - 1)];
                obj.transform.localPosition = new Vector3(i, j);
                
                int v = h - xCount;
                float speed;

                if (speeds.ContainsKey(v))
                {
                    speed = speeds[v];
                }
                else
                {
                    speed = Random.Range(_speed.x, _speed.y);
                    speeds[v] = speed;
                }

                _currentObjects[obj] = speeds[v];
            }
        }
    }


    private void Update()
    {
        foreach (var element in _currentObjects)
        {
            GameObject obj = element.Key;
            float speed = element.Value;

            obj.transform.localPosition += new Vector3(speed, speed, 0);

            if (obj.transform.localPosition.y > _bounds.y)
                obj.transform.localPosition -= new Vector3(600, 600, 0);
        }
    }
}