using ScriptableArchitecture.Core;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public struct BasePartData : IDataPoint
    {
        public string PartName;
        [TextArea] public string PartDescription;
        public string PartType;

        public GameObject PartPrefab;
        public Vector3Int BlockSize;
        public bool NeedsAttachment;
        public List<Vector3Int> RelativeAttachmentPoints;   //like (-1, 0, 0) or (0, 0, 1), max 6

    }
}