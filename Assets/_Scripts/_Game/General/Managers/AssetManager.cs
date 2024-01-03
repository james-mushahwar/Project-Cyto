using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General.SceneLoading;
using Assets._Scripts._Game.General.SceneLoading;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

namespace _Scripts._Game.General.Managers{
    public class AssetManager : Singleton<AssetManager>, IManager
    {
        //Build scenes
        [SerializeField] 
        private SceneField[] _allBuildSceneFields;

        [SerializeField]
        private ZoneScriptableObject[] _zones;

        private Dictionary<string, ZoneScriptableObject> _zoneNameZoneSODict;
        //private Dictionary<int, int[]>[] _zoneAreasDicts;
        //private Dictionary<string, int> _areaNameIndexDict = new Dictionary<string, int>();
        private Dictionary<string, FAreaInfo> _areaNameAreaInfoDict = new Dictionary<string, FAreaInfo>();
        private Dictionary<string, int> _allBuildScenesNameToIndex = new Dictionary<string, int>();
        private Dictionary<int, SceneField> _allBuildIndexToSceneField = new Dictionary<int, SceneField>();
        
        
        //New save zone and area defaults
        [SerializeField] 
        private SceneField _defaultNewSaveZoneScene;
        [SerializeField] 
        private SceneField _defaultNewSaveAreaScene;
        public SceneField DefaultNewSaveZoneScene
        {
            get { return _defaultNewSaveZoneScene; }
        }

        public SceneField DefaultNewSaveAreaScene
        {
            get { return _defaultNewSaveAreaScene; }
        }

        private int _defaultNewSaveAreaIndex;
        public int DefaultNewSaveAreaIndex
        {
            get { return _defaultNewSaveAreaIndex; }
        }



        private bool _initialised = false;

        protected override void Awake()
        {
            if (_initialised)
            {
                return;
            }

            //int sceneCount = SceneManager.sceneCountInBuildSettings;
            //string[] scenes = new string[sceneCount];

            //for (int i = 0; i < sceneCount; i++)
            //{
            //    string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            //    scenes[i] = scenePath;
            //    _allBuildScenesNameToIndex.TryAdd(scenePath, i);
            //}

            for (int i = 0; i < _allBuildSceneFields.Length; i++)
            {
                int sceneIndex = SceneUtility.GetBuildIndexByScenePath(_allBuildSceneFields[i]);
                _allBuildIndexToSceneField.TryAdd(sceneIndex, _allBuildSceneFields[i]);
                _allBuildScenesNameToIndex.TryAdd(_allBuildSceneFields[i], sceneIndex);
            }

            _zoneNameZoneSODict = new Dictionary<string, ZoneScriptableObject>();
            Dictionary<int, int[]> levelIndexDict = new Dictionary<int, int[]>();

            foreach (ZoneScriptableObject zone in _zones)
            {
                FAreaInfo[] areas = zone.AreaInfos;
                foreach (FAreaInfo area in areas)
                {
                    int mainSceneIndex = SceneUtility.GetBuildIndexByScenePath(area.AreaName);
                    _areaNameAreaInfoDict.TryAdd(area.AreaName, area);
                }

                _zoneNameZoneSODict.TryAdd(zone.ZoneScene, zone);
            }
            
            _defaultNewSaveAreaIndex = SceneUtility.GetBuildIndexByScenePath(_defaultNewSaveAreaScene);

            _initialised = true;
        }

        public int SceneNameToBuildIndex(string sceneName)
        {
            int index = -1;
            _allBuildScenesNameToIndex.TryGetValue(sceneName, out index);
            return index;
        }

        public SceneField IndexToSceneField(int index)
        {
            SceneField sceneField = null;
            _allBuildIndexToSceneField.TryGetValue(index, out sceneField);
            return sceneField;
        }

        public IEnumerator LoadZoneAndArea(SceneField zoneScene, SceneField areaScene, bool loadAdditives = true)
        {
            //Debug.Log("Current Zone index is: " + GameStateManager.Instance.CurrentZoneIndex);
            if (!_areaNameAreaInfoDict.ContainsKey(areaScene))
            {
                Debug.LogError("No index in SceneInfo: Index is " + areaScene);
                yield return false;
            }

            PreNewZoneLoad();

            // zones
            SceneField currentZone = GameStateManager.Instance.CurrentZoneScene;

            AsyncOperation zoneSceneLoadSceneAsync = SceneManager.LoadSceneAsync((int)zoneScene, LoadSceneMode.Additive);
            while (!zoneSceneLoadSceneAsync.isDone)
            {
                yield return null;
            }

            if (currentZone != null && currentZone != zoneScene)
            {
                AsyncOperation unloadZoneSceneAsync = SceneManager.UnloadSceneAsync((int)currentZone);
                while (!unloadZoneSceneAsync.isDone)
                {
                    yield return null;
                }
            }

            // areas
            SceneField currentArea = GameStateManager.Instance.CurrentAreaScene;

            AsyncOperation mainAreaLoadScenenAsync = SceneManager.LoadSceneAsync((int)areaScene, LoadSceneMode.Additive);
            while (!mainAreaLoadScenenAsync.isDone)
            {
                yield return null;
            }

            foreach (SceneField additiveScenes in _areaNameAreaInfoDict[areaScene].ConnectedAreas)
            {
                //Debug.Log("Load scene index: " + addIndex);
                AsyncOperation areaSceneLoadSceneAsync = SceneManager.LoadSceneAsync((int)additiveScenes, LoadSceneMode.Additive);
                while (!areaSceneLoadSceneAsync.isDone)
                {
                    yield return null;
                }
            }

            if (currentArea != null && currentArea != areaScene)
            {
                AsyncOperation unloadAreaSceneAsync = SceneManager.UnloadSceneAsync((int)currentArea);
                while (!unloadAreaSceneAsync.isDone)
                {
                    yield return null;
                }

                foreach (SceneField removeAreas in _areaNameAreaInfoDict[currentArea].ConnectedAreas)
                {
                    //Debug.Log("Load scene index: " + addIndex);
                    AsyncOperation removeAreaSceneAsync = SceneManager.UnloadSceneAsync((int)removeAreas);
                    while (!removeAreaSceneAsync.isDone)
                    {
                        yield return null;
                    }
                }
            }

            LightingManager.Instance.FindGlobalLight();
            // post main scene load
            // enable ai path

            // play correct audio track
            // set any post processing for scene
            UpdateStateArea(areaScene);
        }

        private void PreNewZoneLoad()
        {
            // destroy objects before new zone
            List<Object> objectsToBeDestroyed = new List<Object>();

            // destroy old pathfinding singleton
            Object pathfinding = FindObjectOfType(typeof(AstarPath));
            if (pathfinding)
            {
                objectsToBeDestroyed.Add(pathfinding);
            }

            // old global light
            LightingManager.Instance.DisableGlobalLight();

            foreach (Object obj in objectsToBeDestroyed)
            {
                Destroy(obj);
            }
        }

        public void UpdateStateArea(SceneField areaScene)
        {
            // audio
            EAudioTrackTypes musicType = _areaNameAreaInfoDict[areaScene].AreaMusic;
            EAudioTrackTypes ambienceType = _areaNameAreaInfoDict[areaScene].AreaAmbience;
            AudioManager.Instance.StopAllAudioTracks(true);
            AudioManager.Instance.PlayAudio(musicType, true, 2.0f);
        }

        public void PreInGameLoad()
        {
             
        }

        public void PostInGameLoad()
        {
             
        }

        public void PreMainMenuLoad()
        {
             
        }

        public void PostMainMenuLoad()
        {
             
        }
    }
    
}
