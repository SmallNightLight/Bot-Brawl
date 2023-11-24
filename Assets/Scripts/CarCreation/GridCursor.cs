using System.Collections.Generic;
using UnityEngine;
using System;
using ScriptableArchitecture.Data;
using UnityEngine.SceneManagement;

namespace CarCreation
{
    public class GridCursor : MonoBehaviour
    {
        public static event Action<Vector3> CursorMoved;
        // private const int ContainerDim = 6;
        private GameObject _cursorObject;
        private Vector3 _currPosition;
        private Vector3 _prevPosition;
        private Dictionary<KeyCode, Vector3> _keyMappings;

        [SerializeField] private PartDataGameEvent _createNewPart;
        [SerializeField] private List<BasePartDataReference> _baseParts; //change into other way to get base part
        
        //Probably change this to the other existing dectionires, but they are currently not update, so I will use the botData instead
        [SerializeField] private BotDataReference _botData;

        private void Awake()
        {
            _prevPosition = transform.position;
            
            _keyMappings = new Dictionary<KeyCode, Vector3>
            {
                { KeyCode.W, Vector3.forward },
                { KeyCode.S, Vector3.back },
                { KeyCode.D, Vector3.right },
                { KeyCode.A, Vector3.left },
                { KeyCode.E, Vector3.up },
                { KeyCode.Q, Vector3.down }
            };
        }
        
        private void Update()
        {
            _currPosition = transform.position;
            _prevPosition = _currPosition;
            foreach (var kvp in _keyMappings)
            {
                if (Input.GetKeyDown(kvp.Key)) _currPosition += kvp.Value;
            }
            // _currPosition = new Vector3(
            //     Mathf.Clamp(_currPosition.x, 0, ContainerDim - 1),
            //     Mathf.Clamp(_currPosition.y, 0, ContainerDim - 1),
            //     Mathf.Clamp(_currPosition.z, 0, ContainerDim - 1)
            // );
            
            transform.position = _currPosition;
            
            if (!_currPosition.Equals(_prevPosition)) CursorMoved?.Invoke(_currPosition - _prevPosition);


            //Just some code to test the creation part

            Vector3Int partPosition = Vector3Int.RoundToInt(_currPosition);

            if (!_botData.Value.TryGetPartData(partPosition, out PartData existingPart))
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    //1x1x1 block
                    BlockSmall blockSO = ScriptableObject.CreateInstance<BlockSmall>();
                    blockSO.BasePart = _baseParts[0];
                    blockSO.Position = partPosition;

                    //Only works in editor
                    UnityEditor.AssetDatabase.CreateAsset(blockSO, "Assets/Data/Bots/Bot1/Block" + blockSO.Position.ToString() + ".asset");
                    UnityEditor.AssetDatabase.SaveAssets();

                    _createNewPart.Raise(blockSO);
                }

                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    //Wheel
                    WheelPart wheelSO = ScriptableObject.CreateInstance<WheelPart>();
                    wheelSO.BasePart = _baseParts[1];
                    wheelSO.Position = partPosition;

                    //Only works in editor
                    UnityEditor.AssetDatabase.CreateAsset(wheelSO, "Assets/Data/Bots/Bot1/Wheel" + wheelSO.Position.ToString() + ".asset");
                    UnityEditor.AssetDatabase.SaveAssets();

                    _createNewPart.Raise(wheelSO);
                }

                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    //Inverted Wheel, rotated
                    WheelPart wheelSO = ScriptableObject.CreateInstance<WheelPart>();
                    wheelSO.BasePart = _baseParts[1];
                    wheelSO.Position = partPosition;
                    wheelSO.Rotation = new Vector3(0, 180, 0);

                    wheelSO.PartSettings = new WheelPartSettings();
                    wheelSO.PartSettings.Inverted = true;
                    
                    //Only works in editor
                    UnityEditor.AssetDatabase.CreateAsset(wheelSO, "Assets/Data/Bots/Bot1/Wheel" + wheelSO.Position.ToString() + ".asset");
                    UnityEditor.AssetDatabase.SaveAssets();

                    _createNewPart.Raise(wheelSO);
                }
            }
            

            //Test in other scene
            if (Input.GetKeyDown(KeyCode.P))
                SceneManager.LoadScene("MatthiasTesting");
        }
    }
}