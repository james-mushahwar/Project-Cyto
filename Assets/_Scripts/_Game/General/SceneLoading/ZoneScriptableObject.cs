using System.Collections;
using System.Collections.Generic;
using Assets._Scripts._Game.General.SceneLoading;
using UnityEngine;

namespace _Scripts._Game.General.SceneLoading{

    [System.Serializable]
    [CreateAssetMenu(menuName = "Asset Manager/Zone")]
    public class ZoneScriptableObject : ScriptableObject
    {
        //[SerializeField]
        //private int _zoneIndex;
        [SerializeField]
        private SceneField _zoneScene;

        [SerializeField]
        private FAreaInfo[] _areaInfos;

        //public int ZoneIndex { get => _zoneIndex; }
        public SceneField ZoneScene { get => _zoneScene; }

        public FAreaInfo[] AreaInfos { get => _areaInfos; }
    }
    
}
