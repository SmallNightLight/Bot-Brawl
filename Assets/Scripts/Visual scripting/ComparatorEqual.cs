using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultNodeEqual", menuName = "Nodes/Equal")]
public class ComparatorEqual : BaseComparator
{
    public override bool GetComparator(object value1, object value2) => value1 == value2;

    public override string GetBeforeNodeText() => "Equal";
}