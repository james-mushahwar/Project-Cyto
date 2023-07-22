using System.Collections;
using System.Collections.Generic;
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
    public class SceneAdditives : ScriptableObject
    {
        [SerializeField]
        private SceneAdditivesDictionary _sceneAdditiveDict = new SceneAdditivesDictionary();

        public SceneAdditivesDictionary SceneAdditiveDict { get => _sceneAdditiveDict; }
    }

}
