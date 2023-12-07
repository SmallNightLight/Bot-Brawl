using ScriptableArchitecture.Data;
using UnityEngine;

public class WheelObjectPart : ObjectPart
{
    [Header("Components")]
    [SerializeField] private WheelCollider _wheelCollider;
    [SerializeField] private Transform _meshTransform;

    private float _turnAngle;

    void FixedUpdate()
    {
        if (GetComponentInParent<Bot>().IsInFight)
        {
            Steer();
            Accelerate();
            UpdateMeshPosition();
        }
    }

    private void Steer()
    {
        _turnAngle = PartData.GetFloat("Angle");
        _wheelCollider.steerAngle = _turnAngle;
    }

    public void Accelerate()
    {
        if (PartData.GetBool("IsPowered"))
        {
            _wheelCollider.motorTorque = 300 * (PartData.GetBool("Inverted") ? -1 : 1);
            _wheelCollider.brakeTorque = 0;
        }
        else
        {
            _wheelCollider.motorTorque = 0;
            _wheelCollider.brakeTorque = Mathf.Infinity;
        }
            
    }

    private void UpdateMeshPosition()
    {
        _wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);
        _meshTransform.position = pos;
        _meshTransform.rotation = rot;
    }
}