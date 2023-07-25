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
        public int LastSaveIndex { get => _lastSaveIndex; }
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

        [ContextMenu("Delete All Saves")]
        void DeleteAllSaves()
        {
            Delete($"{Application.persistentDataPath}/{_savePaths[0].String}");
            Delete($"{Application.persistentDataPath}/{_savePaths[1].String}");
            Delete($"{Application.persistentDataPath}/{_savePaths[2].String}");
        }
        [ContextMenu("Delete All GamePrefs")]
        void DeleteAllGamePrefs()
        {
            Delete($"{Application.persistentDataPath}/{GamePrefsPath}");
        }

        void Delete(string path)
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
                }
            }
        #if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
        #endif
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
                if (state.TryGetValue(saveable.Id, out object savedSate))
                {
                    saveable.LoadState(saveTarget, savedSate);
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
            if (state.TryGetValue(saveable.Id, out object savedSate))
            {
                saveable.LoadState(saveTarget, savedSate);
            }
        }

        //IPrefsSaveable
        [System.Serializable]
        private struct PrefSaveData
        {
            public int lastSaveIndex;
        }
        public object SavePrefs()
        {
            return new PrefSaveData()
            {
                lastSaveIndex = _lastSaveIndex
            };
        }

        public void LoadPrefs(object state)
        {
            PrefSaveData prefSaveData = (PrefSaveData)state;
            _lastSaveIndex = prefSaveData.lastSaveIndex;
            Debug.Log("Loading last save index which is... " + _lastSaveIndex);
        }
    }
}
