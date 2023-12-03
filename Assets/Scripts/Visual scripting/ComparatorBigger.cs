using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultNodeBigger", menuName = "Nodes/Bigger")]
public class ComparatorBigger : BaseComparator
{
    [Header("Just these needed! (not the other default above)")]
    public BaseGetNumber DefaultNumber1;
    public BaseGetNumber DefaultNumber2;

    public override bool GetComparator(object value1, object value2) => (float)value1 > (float)value2;

    public override List<BaseGet> GetDefaultInput() => new List<BaseGet> { DefaultNumber1, DefaultNumber2 };

    public override string[] GetBeforeNodeText() => new string[] { "", ">" };
}