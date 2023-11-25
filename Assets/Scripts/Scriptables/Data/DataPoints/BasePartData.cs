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
        public PartData DefaultData;

        public GameObject PartPrefab;
        public GameObject CreatorPrefab;
        public Vector3Int BlockSize;
        public bool NeedsAttachment; //Remove
        public List<Vector3Int> RelativeAttachmentPoints;   //like (-1, 0, 0) or (0, 0, 1), max 6
        public Vector3Int DefaultAttachmentDirection;
    }
}