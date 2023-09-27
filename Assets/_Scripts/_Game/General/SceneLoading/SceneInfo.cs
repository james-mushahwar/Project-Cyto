﻿using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts._Game.General.SceneLoading{

    [System.Serializable]
    public struct SceneName
    {
        [SerializeField]
        private string name;

        public string Name { get => name; }
    }

    [System.Serializable]
    [CreateAssetMenu(menuName = "Asset Manager/Scene")]
    public class SceneInfo : ScriptableObject
    {
        [SerializeField]
        private SceneName _sceneName;

        [SerializeField]
        private SceneAdditivesDictionary _sceneAdditiveDict = new SceneAdditivesDictionary();

        public SceneName SceneName { get => _sceneName; }
        public SceneAdditivesDictionary SceneAdditiveDict { get => _sceneAdditiveDict; }
    }

    [System.Serializable]
    public class FAreaInfo
    {
        [SerializeField]
        private SceneName _areaName;

        [SerializeField]
        private SceneName[] _connectedAreas;

        [SerializeField]
        private EAudioTrackTypes _areaMusic;
        [SerializeField]
        private EAudioTrackTypes _areaAmbience;

        public SceneName AreaName { get => _areaName; }
        public SceneName[] ConnectedAreas { get => _connectedAreas; }
        public EAudioTrackTypes AreaMusic { get => _areaMusic; }
        public EAudioTrackTypes AreaAmbience { get => _areaAmbience; }
    }
}
