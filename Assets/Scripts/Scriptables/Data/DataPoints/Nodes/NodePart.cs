using ScriptableArchitecture.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [CreateAssetMenu(fileName = "DefaultNodePart", menuName = "Nodes/Part")]
    public class NodePart : Node
    {
        public PartData PartData;

        public List<PartSetting> NewSettings = new List<PartSetting>();

        //public PartSetting SelectedPartSetting;

        public override void ExecuteNode()
        {
            PartData.Settings = NewSettings;
        }
    }
}