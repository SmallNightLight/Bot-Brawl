using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class ControlBots : MonoBehaviour
{
    public void Right()
    {
        transform.localPosition -= new Vector3(450, 0, 0);
    }

    public void Left()
    {
        transform.localPosition += new Vector3(450, 0, 0);
    }
}