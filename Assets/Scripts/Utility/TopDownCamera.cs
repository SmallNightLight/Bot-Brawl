using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    [SerializeField] private float _minZoomDistace = 5f;
    [SerializeField] private float _maxDistance = 30f;
    [SerializeField] private float _smoothing = 0.5f;
    [SerializeField] private float _minY;
    [SerializeField] private float _maxY;

    [SerializeField] private List<Transform> _targets = new List<Transform>();

    private Vector3 _velocity;

    public static TopDownCamera Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
            Destroy(gameObject);
    }

    public void AddTarget(Transform target)
    {
        _targets.Add(target);


    }

    private void LateUpdate()
    {
        if (_targets.Count == 0)
            return;

        Move();
        Zoom();
    }

    private void Move()
    {
        Vector3 centerPoint = GetCenterPoint();
        centerPoint.y = transform.position.y;

        transform.position = Vector3.SmoothDamp(transform.position, centerPoint, ref _velocity, _smoothing);
    }

    private void Zoom()
    {
        float greatestDístance = GetGreatestDistance();

        if (greatestDístance < _minZoomDistace)
            greatestDístance = 0f;

        float newY = Mathf.Lerp(_minY, _maxY, greatestDístance / _maxDistance);

        transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, newY, Time.deltaTime), transform.position.z);
    }

    private float GetGreatestDistance()
    {
        Bounds bounds = EncapsulateTargets();

        return bounds.size.x > bounds.size.z ? bounds.size.x : bounds.size.z;
    }

    private Vector3 GetCenterPoint()
    {
        if (_targets.Count == 1)
        {
            return _targets[0].position;
        }

        Bounds bounds = EncapsulateTargets();

        return bounds.center;
    }

    private Bounds EncapsulateTargets()
    {
        Bounds bounds = new Bounds(_targets[0].position, Vector3.zero);

        foreach(Transform target in _targets)
        {
            bounds.Encapsulate(target.position);
        }


        return bounds;
    }
}