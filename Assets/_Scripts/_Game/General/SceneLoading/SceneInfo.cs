using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General.Managers;
using Assets._Scripts._Game.General.SceneLoading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts._Game.General.SceneLoading{

    //[System.Serializable]
    //public struct SceneName
    //{
    //    [SerializeField]
    //    private string name;

    //    public string Name { get => name; }
    //}

    [System.Serializable]
    [CreateAssetMenu(menuName = "Asset Manager/Scene")]
    public class SceneInfo : ScriptableObject
    {
        [SerializeField]
        private SceneField _sceneField;

        [SerializeField]
        private SceneAdditivesDictionary _sceneAdditiveDict = new SceneAdditivesDictionary();

        public SceneField SceneName { get => _sceneField; }
        public SceneAdditivesDictionary SceneAdditiveDict { get => _sceneAdditiveDict; }
    }

    [System.Serializable]
    public class FAreaInfo
    {
        [SerializeField]
        private SceneField _areaName;

        [SerializeField]
        private SceneField[] _connectedAreas;

        [SerializeField]
        private EAudioTrackTypes _areaMusic;
        [SerializeField]
        private EAudioTrackTypes _areaAmbience;

        public SceneField AreaName { get => _areaName; }
        public SceneField[] ConnectedAreas { get => _connectedAreas; }
        public EAudioTrackTypes AreaMusic { get => _areaMusic; }
        public EAudioTrackTypes AreaAmbience { get => _areaAmbience; }
    }
}
