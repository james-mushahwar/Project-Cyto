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
        private Dictionary<string, int> _areaNameIndexDict = new Dictionary<string, int>();
        private Dictionary<int, FAreaInfo> _buildIndexAreaInfoDict = new Dictionary<int, FAreaInfo>();
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
            Dictionary<int, int[]> levelIndexDict = new Dictionary<int, int[]>();
            int index = 0;

            foreach (ZoneScriptableObject zone in _zones)
            {
                FAreaInfo[] areas = zone.AreaInfos;
                foreach (FAreaInfo area in areas)
                {
                    int mainSceneIndex = SceneUtility.GetBuildIndexByScenePath("Assets/_Scenes/Areas/" + area.AreaName.Name + ".unity");
                    Debug.Log("Main scene index: " + mainSceneIndex);
                    int[] additiveIndices = new int[area.ConnectedAreas.Length];

                    _areaNameIndexDict.TryAdd(area.AreaName.Name, mainSceneIndex);
                    _buildIndexAreaInfoDict.TryAdd(mainSceneIndex, area);

                    if (levelIndexDict.ContainsKey(mainSceneIndex))
                    {
                        continue;
                    }

                    for (int i = 0; i < area.ConnectedAreas.Length; ++i)
                    {
                        additiveIndices[i] = SceneUtility.GetBuildIndexByScenePath("Assets/_Scenes/Areas/" + area.ConnectedAreas[i].Name + ".unity");
                        Debug.Log("Add scene path: " + additiveIndices[i] + " - Assets/_Scenes/Areas/" + area.ConnectedAreas[i].Name + ".unity");
                    }

                    levelIndexDict.TryAdd(mainSceneIndex, additiveIndices);
                }

                _zoneAreasDicts[index] = levelIndexDict;
                index++;
            }
            

            _defaultNewSaveAreaIndex = SceneUtility.GetBuildIndexByScenePath("Assets/_Scenes/Areas/" + _defaultNewSaveSceneName + ".unity");

            _initialised = true;
        }

        public int AreaNameToBuildIndex(string areaName)
        {
            int index = -1;
            _areaNameIndexDict.TryGetValue(areaName, out index);
            return index;
        }

        public IEnumerator LoadZoneAreaByIndex(int index, bool loadAdditives = true)
        {
            Debug.Log("Current Zone index is: " + GameStateManager.Instance.CurrentZoneIndex);
            if (!_zoneAreasDicts[GameStateManager.Instance.CurrentZoneIndex].ContainsKey(index))
            {
                Debug.LogError("No index in SceneInfo: Index is " + index);
                yield return false;
            }

            //SceneManager.LoadScene(index);

            AsyncOperation zoneSceneLoadSceneAsync = SceneManager.LoadSceneAsync(_zones[GameStateManager.Instance.ZoneSpawnIndex].ZoneName, LoadSceneMode.Additive);
            while (!zoneSceneLoadSceneAsync.isDone)
            {
                yield return null;
            }

            AsyncOperation mainAreaLoadScenenAsync = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
            while (!mainAreaLoadScenenAsync.isDone)
            {
                yield return null;
            }

            foreach (int addIndex in _zoneAreasDicts[GameStateManager.Instance.CurrentZoneIndex][index])
            {
                Debug.Log("Load scene index: " + addIndex);
                AsyncOperation areaSceneLoadSceneAsync = SceneManager.LoadSceneAsync(addIndex, LoadSceneMode.Additive);
                while (!areaSceneLoadSceneAsync.isDone)
                {
                    yield return null;
                }
            }
            // post main scene load
            // enable ai path

            // play correct audio track
            // set any post processing for scene
            UpdateStateArea();
        }

        private IEnumerator LoadZoneAreaByIndexEnumerator(int index, bool loadAddtives = true)
        {
            yield return null;
        }

        public void UpdateStateArea()
        {
            // audio
            EAudioTrackTypes musicType = _buildIndexAreaInfoDict[GameStateManager.Instance.CurrentAreaIndex].AreaMusic;
            EAudioTrackTypes ambienceType = _buildIndexAreaInfoDict[GameStateManager.Instance.CurrentAreaIndex].AreaAmbience;
            AudioManager.Instance.PlayAudio(musicType, true, 0.5f);

        }
    }
    
}
