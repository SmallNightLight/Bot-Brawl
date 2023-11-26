using ScriptableArchitecture.Data;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    [SerializeField] private TransformReference _target;

    [Header("Selection")]
    [SerializeField] private BoolReference _isPlacing;
    [SerializeField] private LayerMask _unitLayer;

    [Header("Events")]
    [SerializeField] private PlacingInfoGameEvent _placingPartEvent;
    [SerializeField] private PlacingInfoGameEvent _previewPartEvent;
    [SerializeField] private PartDataGameEvent _selectedPartData;

    [Header("Rotation")]
    [SerializeField] private float _distance = 6;
    [SerializeField] private float _minYAngle = -30f;
    [SerializeField] private float _maxYAngle = 90f;

    [Header("Scrool wheel")]
    [SerializeField] private float _scroolSensitivity = 7;
    [SerializeField] private float _scrollSmoothness = 5;
    [SerializeField] private float _minDistance = 1;
    [SerializeField] private float _maxDistance = 30;

    
    private Vector3 _offset;
    private float _currentDistance;
    private Camera _camera;
    private Vector3 _previousPosition;
    private float y = 0;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        _currentDistance = _distance;
    }

    private void Update()
    {
        //Selecting objects

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _unitLayer))
        {
            if (_isPlacing.Value)
            {
                PlacingInfo placingInfo = new PlacingInfo();
                placingInfo.AttachPoint = Vector3Int.RoundToInt(hit.normal + hit.collider.transform.position);
                placingInfo.Normal = Vector3Int.RoundToInt(hit.normal);
        
                try
                {
                    PartData otherPartData = hit.collider.gameObject.GetComponent<Unit>().UnitPartData;
                    if (otherPartData == null)
                        Debug.LogError("No PartData selected for unit: " + hit.collider.gameObject.name);
        
                    placingInfo.OtherPart = otherPartData;
                }
                catch
                {
                    Debug.LogError("Didn't add a unit component to unit: " + hit.collider.gameObject.name);
                }
        
                if (Input.GetMouseButtonDown(0))
                    _placingPartEvent.Raise(placingInfo);
                else
                    _previewPartEvent.Raise(placingInfo);
            }
            else
            {
                if (Input.GetMouseButtonDown(0) && hit.collider.gameObject.TryGetComponent(out Unit selectedUnit))
                    _selectedPartData.Raise(selectedUnit.UnitPartData);
            }
        }
            

        //Rotating
        if (Input.GetMouseButtonDown(1))
        {
            _previousPosition = _camera.ScreenToViewportPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(1))
        {
            Vector3 newPosition = _camera.ScreenToViewportPoint(Input.mousePosition);
            Vector3 direction = _previousPosition - newPosition;

            float rotationAroundYAxis = -direction.x * 180;
            float rotationAroundXAxis = direction.y * 180;

            rotationAroundXAxis = Mathf.Clamp(rotationAroundXAxis, _minYAngle - (y + rotationAroundXAxis), _maxYAngle - (y + rotationAroundXAxis));

            _camera.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
            _camera.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World);

            _previousPosition = newPosition;
            y += rotationAroundXAxis;
        }

        //Pan
        if (Input.GetMouseButton(2))
        {
            float horizontal = Input.GetAxis("Mouse X");
            float vertical = Input.GetAxis("Mouse Y");

            float relativeSpeed = 2.36276f * _distance - 0.88276f;
            Vector3 movement = (transform.right * -horizontal + transform.up * -vertical) * relativeSpeed * Time.deltaTime;

            transform.Translate(movement);
            _offset += movement;
        }

        //Zooming
        float scroolWheel = Input.GetAxis("Mouse ScrollWheel") * _scroolSensitivity;

        _distance -= scroolWheel;
        _distance = Mathf.Min(Mathf.Max(_distance, _minDistance), _maxDistance);

        _currentDistance = Mathf.Lerp(_currentDistance, _distance, _scrollSmoothness * Time.deltaTime);

        _camera.transform.position = _target.Value.position + _offset;
        _camera.transform.Translate(new Vector3(0, 0, -_currentDistance));
    }
}