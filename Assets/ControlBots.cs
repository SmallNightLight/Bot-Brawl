using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class ControlBots : MonoBehaviour
{
    public float smoothTime = 0.5f; // Adjust this value to control the smoothness
    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPosition;

    private void Start()
    {
        targetPosition = transform.localPosition;
    }

    public void Right()
    {
        targetPosition -= new Vector3(450, 0, 0);

    }

    public void Left()
    {
        targetPosition += new Vector3(450, 0, 0);
    }

    private void Update()
    {
        // Smoothly move towards the target position using Vector3.SmoothDamp
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetPosition, ref velocity, smoothTime);
    }
}