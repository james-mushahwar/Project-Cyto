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
        private ZoneScriptableObject[] _zones;

        private Dictionary<int, int[]> _levelIndexDict = new Dictionary<int, int[]>();
        private Dictionary<int, int[]>[] _zoneAreasDicts;
        //New save zone and area defaults
        [SerializeField]
        private int _defaultNewSaveZoneIndex = 0;
        public int DefaultNewSaveZoneIndex
        {
            get { return _defaultNewSaveZoneIndex; }
        }

        [SerializeField] 
        private string _defaultNewSaveSceneName;
        private int _defaultNewSaveAreaIndex;
        public int DefaultNewSaveAreaIndex
        {
            get { return _defaultNewSaveAreaIndex; }
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

            _zoneAreasDicts = new Dictionary<int, int[]>[_zones.Length];
            int index = 0;
            foreach (ZoneScriptableObject zone in _zones)
            {
                FAreaInfo[] areas = zone.AreaInfos;
                foreach (FAreaInfo area in areas)
                {
                    int mainSceneIndex = SceneUtility.GetBuildIndexByScenePath("Assets/_Scenes/Areas/" + area.AreaName.Name + ".unity");
                    Debug.Log("Main scene index: " + mainSceneIndex);
                    int[] additiveIndices = new int[area.ConnectedAreas.Length];

                    if (_levelIndexDict.ContainsKey(mainSceneIndex))
                    {
                        continue;
                    }

                    for (int i = 0; i < area.ConnectedAreas.Length; ++i)
                    {
                        additiveIndices[i] = SceneUtility.GetBuildIndexByScenePath("Assets/_Scenes/Areas/" + area.ConnectedAreas[i].Name + ".unity");
                        Debug.Log("Add scene path: " + additiveIndices[i] + " - Assets/_Scenes/Areas/" + area.ConnectedAreas[i].Name + ".unity");
                    }

                    _levelIndexDict.TryAdd(mainSceneIndex, additiveIndices);
                }

                _zoneAreasDicts[index] = _levelIndexDict;
                index++;
            }
            

            _defaultNewSaveAreaIndex = SceneUtility.GetBuildIndexByScenePath("Assets/_Scenes/Areas/" + _defaultNewSaveSceneName + ".unity");

            _initialised = true;
        }

        public void LoadZoneAreaByIndex(int index, bool loadAdditives = true)
        {
            if (!_levelIndexDict.ContainsKey(index))
            {
                Debug.LogError("No index in SceneInfo: Index is " + index);
                return;
            }

            SceneManager.LoadScene(index);

            SceneManager.LoadSceneAsync(_zones[GameStateManager.Instance.ZoneSpawnIndex].ZoneName, LoadSceneMode.Additive);

            foreach (int addIndex in _levelIndexDict[index])
            {
                Debug.Log("Load scene index: " + addIndex);
                SceneManager.LoadSceneAsync(addIndex, LoadSceneMode.Additive);
            }


            // post main scene load
            // enable ai path

            // play correct audio track
            // set any post processing for scene
        }
    }
    
}
