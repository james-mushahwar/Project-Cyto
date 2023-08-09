using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using _Scripts._Game.General.Managers;

namespace _Scripts._Game.General.SaveLoad{

    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField]
        private string id;
        public string Id { get => id; }

        [Header("Editor Save settings")]
        [SerializeField]
        private bool _editorIgnoreAllSaves;
        [SerializeField]
        private bool _editorIgnoreAllLoads;

        [Header("Save and load settings")]
        [SerializeField]
        private ESaveType[] _excludedSaveTypes;
        [SerializeField]
        private bool _saveOnDisable = false;
        [SerializeField]
        private bool _loadOnEnable = false;
        [SerializeField]
        private bool _skipLoadOnFirstPlay = false;

        bool hasFinishedStartup = false;
        //public ESaveType[] ExcludedSaveTypes { get => _excludedSaveTypes; }

        [ContextMenu("Generate id")]
        private void GenerateId()
        {
            id = Guid.NewGuid().ToString();
        }

        public void Start()
        {
            hasFinishedStartup = true;
            Load();

        }

        public void OnEnable()
        {
            if (!hasFinishedStartup)
            {
                return;
            }

            Load();
        }

        private void Load()
        {
            #if UNITY_EDITOR
            if (_editorIgnoreAllLoads)
            {
                return;
            }
            // on first play, ignore load
            if (_skipLoadOnFirstPlay && !SaveLoadSystem.Instance.HasFirstTickElapsed)
            {
                return;
            }
            #endif

            // get loaded state from SaveLoad
            if (!_loadOnEnable)
            {
                return;
            }
            SaveLoadSystem.Instance?.OnEnableLoadState(ESaveTarget.Saveable, this);
        }

        public void OnDestroy()
        {
        #if UNITY_EDITOR
            if (_editorIgnoreAllSaves)
            {
                return;
            }
        #endif
            // save state to SaveLoad
            if (!_saveOnDisable)
            {
                return;
            }

            // if application quitting ignore
            if (GameStateManager.IsQuitting)
            {
                return;
            }

            SaveLoadSystem.Instance?.OnDisableSaveState(ESaveTarget.Saveable, this);
        }

        public object SaveState(ESaveTarget saveTarget)
        {
            var state = new Dictionary<string, object>();

            if (saveTarget == ESaveTarget.COUNT)
            {
                return state;
            }

            if (saveTarget == ESaveTarget.GamePrefs)
            {
                foreach (var saveable in GetComponents<IPrefsSaveable>())
                {
                    state[saveable.GetType().ToString()] = saveable.SavePrefs();
                }
            }
            else
            {
                foreach (var saveable in GetComponents<ISaveable>())
                {
                    state[saveable.GetType().ToString()] = saveable.SaveState();
                }
            }

            return state;
        }

        public void LoadState(ESaveTarget saveTarget, object state)
        {
            var stateDictionary = (Dictionary<string, object>)state;

            if (saveTarget == ESaveTarget.COUNT)
            {
                return;
            }

            if (saveTarget == ESaveTarget.GamePrefs)
            {
                foreach (var saveable in GetComponents<IPrefsSaveable>())
                {
                    string typeName = saveable.GetType().ToString();
                    if (stateDictionary.TryGetValue(typeName, out object savedState))
                    {
                        saveable.LoadPrefs(savedState);
                    }
                }
            }
            else
            {
                foreach (var saveable in GetComponents<ISaveable>())
                {
                    string typeName = saveable.GetType().ToString();
                    if (stateDictionary.TryGetValue(typeName, out object savedState))
                    {
                        saveable.LoadState(savedState);
                    }
                }
            }

            
        }

        void Reset()
        {
            if (string.IsNullOrEmpty(id))
            {
                GenerateId();
            }
        }

        public bool CanSave(ESaveTarget saveTarget, ESaveType saveType)
        {
            if (saveTarget == ESaveTarget.Saveable)
            {
                foreach (ESaveType type in _excludedSaveTypes)
                {
                    if (saveType == type)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
