using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

        //public ESaveType[] ExcludedSaveTypes { get => _excludedSaveTypes; }

        [ContextMenu("Generate id")]
        private void GenerateId()
        {
            id = Guid.NewGuid().ToString();
        }

        public void Start()
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
            SaveLoadSystem.Instance?.OnEnableLoadState(this);
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
            SaveLoadSystem.Instance?.OnDisableSaveState(this);
        }

        public object SaveState()
        {
            var state = new Dictionary<string, object>();
            foreach (var saveable in GetComponents<ISaveable>())
            {
                state[saveable.GetType().ToString()] = saveable.SaveState();
            }

            return state;
        }

        public void LoadState(object state)
        {
            var stateDictionary = (Dictionary<string, object>)state;

            foreach (var saveable in GetComponents<ISaveable>())
            {
                string typeName = saveable.GetType().ToString();
                if (stateDictionary.TryGetValue(typeName, out object savedState))
                {
                    saveable.LoadState(savedState);
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

        public bool CanSave(ESaveType saveType)
        {
            foreach (ESaveType type in _excludedSaveTypes)
            {
                if (saveType == type)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
