using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.SceneLoading{

    [System.Serializable]
    [CreateAssetMenu(menuName = "Asset Manager/Zone")]
    public class ZoneScriptableObject : ScriptableObject
    {
        //[SerializeField]
        //private int _zoneIndex;
        [SerializeField]
        private string _zoneName;

        [SerializeField]
        private FAreaInfo[] _areaInfos;

        //public int ZoneIndex { get => _zoneIndex; }
        public string ZoneName { get => _zoneName; }

        public FAreaInfo[] AreaInfos { get => _areaInfos; }
    }
    
}
