using UnityEngine;

public class Node : ScriptableObject
{
    [HideInInspector] public static int ID;

    [Header("Drawing")]
    public string BaseNodeName;
    public Color BaseNodeColor;
}