using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptingBase : MonoBehaviour
{
    public void Compile() => DataManager.Instance.Compile();
}