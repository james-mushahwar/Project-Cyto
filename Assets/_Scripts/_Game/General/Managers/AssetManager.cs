using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General.SceneLoading;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

namespace _Scripts._Game.General.Managers{
    public class AssetManager : Singleton<AssetManager>
    {
        [SerializeField]
        private SceneAdditives _levelSceneAdditives;
        private Dictionary<int, int[]> _levelIndexDict = new Dictionary<int, int[]>();

        [SerializeField] 
        private string _defaultNewSaveSceneName;

        private int _defaultNewSaveSceneIndex;
        public int DefaultNewSaveSceneIndex
        {
            get { return _defaultNewSaveSceneIndex; }
        }

        private bool _initialised = false;

        public void Awake()
        {
            Debug.Log("LOADING ASSETMANAGER");
            if (_initialised)
            {
                return;
            }
            Debug.Log("STILL LOADING ASSETMANAGER");

            foreach (KeyValuePair<string, SceneName[]> entry in _levelSceneAdditives.SceneAdditiveDict)
            {
                int mainSceneIndex = SceneUtility.GetBuildIndexByScenePath("Assets/_Scenes/Levels/" + entry.Key + ".unity");
                Debug.Log("Main scene index: " + mainSceneIndex);
                int[] additiveIndices = new int[entry.Value.Length];

                if (_levelIndexDict.ContainsKey(mainSceneIndex))
                {
                    continue;
                }

                for (int i = 0; i < entry.Value.Length; ++i)
                {
                    additiveIndices[i] = SceneUtility.GetBuildIndexByScenePath("Assets/_Scenes/Levels/" + entry.Value[i].Name + ".unity");
                    Debug.Log("Add scene path: " + additiveIndices[i] + " - Assets/_Scenes/Levels/" + entry.Value[i].Name + ".unity");
                }

                _levelIndexDict.TryAdd(mainSceneIndex, additiveIndices);
            }

            _defaultNewSaveSceneIndex = SceneUtility.GetBuildIndexByScenePath("Assets/_Scenes/Levels/" + _defaultNewSaveSceneName + ".unity");

            _initialised = true;
        }

        public void LoadSceneByIndex(int index, bool loadAdditives = true)
        {
            if (!_levelIndexDict.ContainsKey(index))
            {
                Debug.LogError("No index in sceneAdditives: Index is " + index);
                return;
            }

            SceneManager.LoadScene(index);

            foreach (int addIndex in _levelIndexDict[index])
            {
                Debug.Log("Load scene index: " + addIndex);
                SceneManager.LoadSceneAsync(addIndex, LoadSceneMode.Additive);
            }

            SceneManager.LoadSceneAsync("SceneSwitcher", LoadSceneMode.Additive);
        }
    }
    
}
