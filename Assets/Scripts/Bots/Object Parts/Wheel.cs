using ScriptableArchitecture.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : ObjectPart
{
    [Header("Components")]
    [SerializeField] private WheelCollider _wheelCollider;
    [SerializeField] private Transform _meshTransform;

    [Header("Settings")]
    [SerializeField] private float _power = 150f;
    [SerializeField] private float _maxAngle = 90f;
    [SerializeField] private float _offset = 0f;

    private Vector2 _input;
    private float _turnAngle;

    void Update()
    {
        //Replace this with other movement in the future
        _input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    void FixedUpdate()
    {
        Steer(_input.x);
        Accelerate(_input.y);
        UpdateMeshPosition();
    }

    private void Steer(float horizontalDirection)
    {
        _turnAngle = horizontalDirection * _maxAngle + _offset;
        _wheelCollider.steerAngle = _turnAngle;
    }

    private void Accelerate(float powerInput)
    {
        if (_isPowered.Value) 
            _wheelCollider.motorTorque = powerInput * _power * 100;
        else 
            _wheelCollider.brakeTorque = 0; //Or even go negative
    }

    private void UpdateMeshPosition()
    {
        _wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);
        _meshTransform.position = pos;
        _meshTransform.rotation = rot;
    }
}