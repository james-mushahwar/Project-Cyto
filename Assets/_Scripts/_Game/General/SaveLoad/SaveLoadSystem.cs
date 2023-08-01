using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using _Scripts._Game.General.Managers;

namespace _Scripts._Game.General.SaveLoad{

    [System.Serializable]
    public struct StringStruct
    {
        [SerializeField]
        private string _string;

        public string String { get => _string; }
    }

    public class SaveLoadSystem : Singleton<SaveLoadSystem>, IPrefsSaveable
    {
        // these are the 3 save slots for each game
        [SerializeField]
        private StringStruct[] _savePaths; 

        public string SavePath
        {
            get { return $"{Application.persistentDataPath}/{_savePaths[GameStateManager.Instance.SaveIndex].String}"; }
        }

        // this is the 1 save to encapsulate things that are saved across the entire game, regardless of which save slot is loaded
        [SerializeField]
        private StringStruct _gamePrefsPath;

        public string GamePrefsPath
        {
            get { return $"{Application.persistentDataPath}/{_gamePrefsPath.String}"; }
        }

        private Dictionary<string, object> _gamePrefsDict = new Dictionary<string, object>();

        private bool _hasFirstTickElapsed = false;
        public bool HasFirstTickElapsed { get => _hasFirstTickElapsed; }

        private static bool _isSaveOrLoadInProgress;
        private ESaveType _saveType;

        public static bool IsSaveOrLoadInProgress { get => _isSaveOrLoadInProgress; }

        #region Game prefs

        private int _lastSaveIndex = 0;
        public int LastSaveIndex { get => _lastSaveIndex; set => _lastSaveIndex = value; }

        private bool[] _saveInSlotIsValid = new bool[3];
        #endregion

        private void Start()
        {
        #if UNITY_EDITOR
            if (DebugManager.Instance.DebugSettings.SaveLoadClear)
            {
                DeleteAllSaves();
                DeleteAllGamePrefs();
            }
        #endif

            LoadGamePrefs();
        }

        private void Update()
        {
            if (!_hasFirstTickElapsed)
            {
                _hasFirstTickElapsed = true;
            }
        }

        [ContextMenu("Save")]
        void Save(ESaveType saveType)
        {
            if (_isSaveOrLoadInProgress)
            {
                return;
            }
            Debug.Log("Saving");
            _isSaveOrLoadInProgress = true;
            _saveType = saveType;
            var state = LoadFile(SavePath);
            SaveState(ESaveTarget.Saveable, state);
            SaveFile(state, SavePath);

            if (_saveType == ESaveType.Manual)
            {
                _gamePrefsDict = LoadFile(GamePrefsPath);
                SaveState(ESaveTarget.GamePrefs, _gamePrefsDict);
                SaveFile(_gamePrefsDict, GamePrefsPath);
            }
            
            _isSaveOrLoadInProgress = false;
            _saveType = ESaveType.NONE;

        }

        public void SaveManual()
        {
            Save(ESaveType.Manual);
        }

        [ContextMenu("Load")]
        void Load()
        {
            if (_isSaveOrLoadInProgress)
            {
                return;
            }
            _isSaveOrLoadInProgress = true;

            var state = LoadFile(SavePath);
            LoadState(ESaveTarget.Saveable, state);

            _gamePrefsDict = LoadFile(GamePrefsPath);
            LoadState(ESaveTarget.GamePrefs, _gamePrefsDict);

            _isSaveOrLoadInProgress = false;

        }

        void LoadGamePrefs()
        {
            _gamePrefsDict = LoadFile(GamePrefsPath);
            LoadState(ESaveTarget.GamePrefs, _gamePrefsDict);
        }

        public object RetrieveLoadObject(ELoadSpecifier loadSpecifier, int saveIndex)
        {
            object savedObject = null;
            if(loadSpecifier == ELoadSpecifier.PlayTime)
            {
                var state = LoadFile($"{Application.persistentDataPath}/{_savePaths[saveIndex].String}");
                GameObject statmanagerGO = StatsManager.Instance.gameObject;
                if (statmanagerGO)
                {
                    SaveableEntity saveableEntity = statmanagerGO.GetComponentInChildren<SaveableEntity>();
                    if (saveableEntity)
                    {
                        if (state.TryGetValue(saveableEntity.Id, out object savedState))
                        {
                            Dictionary<string, object> saveDict = (Dictionary<string, object>)savedState;

                            string typeName = StatsManager.Instance.GetType().ToString();
                            if (saveDict.TryGetValue(typeName, out object save))
                            {
                                return save;
                            }
                        }
                    }
                }
                
            }
            return savedObject;
        }

        [ContextMenu("Delete All Saves")]
        void DeleteAllSaves()
        {
            Delete($"{Application.persistentDataPath}/{_savePaths[0].String}");
            Delete($"{Application.persistentDataPath}/{_savePaths[1].String}");
            Delete($"{Application.persistentDataPath}/{_savePaths[2].String}");
        }
        void Delete(int index)
        {
            if (Delete($"{Application.persistentDataPath}/{_savePaths[index].String}"))
            {
                _saveInSlotIsValid[index] = false;
            }
        }
        [ContextMenu("Delete All GamePrefs")]
        void DeleteAllGamePrefs()
        {
            Delete($"{Application.persistentDataPath}/{GamePrefsPath}");
        }

        bool Delete(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                    Debug.Log("Deleted save file");

                }
                catch
                {
                    Debug.LogWarning("Failed to delete $\"{ Application.persistentDataPath}/ save.txt\"");
                    return false;
                }
            }
        #if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
            return true;
        }

        public BinaryFormatter GetBinaryFormatter()
        {
            BinaryFormatter formatter = new BinaryFormatter();

            SurrogateSelector surrogateSelector = new SurrogateSelector();
            Vector3SerializationSurrogate v3Surrogate = new Vector3SerializationSurrogate();

            surrogateSelector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), v3Surrogate);

            formatter.SurrogateSelector = surrogateSelector;

            return formatter;
        }

        public void SaveFile(object state, string path)
        {
            using (var stream = File.Open(path, FileMode.Create))
            {
                var formatter = GetBinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }

        Dictionary<string, object> LoadFile(string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogWarning("No save file found - creating new SaveFile for path: " + path);
                var newState = new Dictionary<string, object>();
                SaveFile(newState, path);
            }

            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                var formatter = GetBinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }

        void SaveState(ESaveTarget saveTarget, Dictionary<string, object> state)
        {
            foreach (var saveable in FindObjectsOfType<SaveableEntity>())
            {
                if (!saveable.CanSave(saveTarget, _saveType))
                {
                    continue;
                }
                state[saveable.Id] = saveable.SaveState(saveTarget);
            }
        }

        void LoadState(ESaveTarget saveTarget, Dictionary<string, object> state)
        {
            foreach (var saveable in FindObjectsOfType<SaveableEntity>())
            {
                if (state.TryGetValue(saveable.Id, out object savedState))
                {
                    saveable.LoadState(saveTarget, savedState);
                }
            }
        }

        public void OnDisableSaveState(ESaveTarget saveTarget, SaveableEntity saveable)
        {
            var state = LoadFile(SavePath);
            state[saveable.Id] = saveable.SaveState(saveTarget);
            SaveFile(state, SavePath);
        }

        public void OnEnableLoadState(ESaveTarget saveTarget, SaveableEntity saveable)
        {
            var state = LoadFile(SavePath);
            if (state.TryGetValue(saveable.Id, out object savedState))
            {
                saveable.LoadState(saveTarget, savedState);
            }
        }

        //IPrefsSaveable
        [System.Serializable]
        private struct PrefSaveData
        {
            public int lastSaveIndex;
            public bool[] saveInSlotIsValid;
        }
        public object SavePrefs()
        {
            PrefSaveData _prefSaveData = new PrefSaveData()
            {
                lastSaveIndex = _lastSaveIndex,
                saveInSlotIsValid = new bool[_saveInSlotIsValid.Length]
            };

            for (int i = 0; i < _saveInSlotIsValid.Length; i++)
            {
                _prefSaveData.saveInSlotIsValid[0] = _saveInSlotIsValid[0];
                _prefSaveData.saveInSlotIsValid[1] = _saveInSlotIsValid[1];
                _prefSaveData.saveInSlotIsValid[2] = _saveInSlotIsValid[2];
            }

            return _prefSaveData;
        }

        public void LoadPrefs(object state)
        {
            PrefSaveData prefSaveData = (PrefSaveData)state;
            _lastSaveIndex = prefSaveData.lastSaveIndex;

            _saveInSlotIsValid = prefSaveData.saveInSlotIsValid;
            Debug.Log("Loading last save index which is... " + _lastSaveIndex);
        }
    }
}
