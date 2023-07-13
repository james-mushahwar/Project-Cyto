using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace _Scripts._Game.General.SaveLoad{

    public class SaveLoadSystem : MonoBehaviour
    {
        public string m_SavePath => $"{Application.persistentDataPath}/save.txt";

        private static bool _isSaveOrLoadInProgress;
        public static bool IsSaveOrLoadInProgress { get => _isSaveOrLoadInProgress; }

        [ContextMenu("Save")]
        public void Save()
        {
            if (_isSaveOrLoadInProgress)
            {
                return;
            }
            Debug.Log("Saving");
            _isSaveOrLoadInProgress = true;
            var state = LoadFile();
            SaveState(state);
            SaveFile(state);

            _isSaveOrLoadInProgress = false;

        }

        [ContextMenu("Load")]
        void Load()
        {
            if (_isSaveOrLoadInProgress)
            {
                return;
            }
            _isSaveOrLoadInProgress = true;

            var state = LoadFile();
            LoadState(state);

            _isSaveOrLoadInProgress = false;

        }

        [ContextMenu("Delete")]
        void Delete()
        {
            if (File.Exists(m_SavePath))
            {
                try
                {
                    File.Delete(m_SavePath);
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

        public void SaveFile(object state)
        {
            using (var stream = File.Open(m_SavePath, FileMode.Create))
            {
                var formatter = GetBinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }

        Dictionary<string, object> LoadFile()
        {
            if (!File.Exists(m_SavePath))
            {
                Debug.LogWarning("No save file found");
                return new Dictionary<string, object>();
            }

            using (FileStream stream = File.Open(m_SavePath, FileMode.Open))
            {
                var formatter = GetBinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }

        void SaveState(Dictionary<string, object> state)
        {
            foreach (var saveable in FindObjectsOfType<SaveableEntity>())
            {
                state[saveable.Id] = saveable.SaveState();
            }
        }

        void LoadState(Dictionary<string, object> state)
        {
            foreach (var saveable in FindObjectsOfType<SaveableEntity>())
            {
                if (state.TryGetValue(saveable.Id, out object savedSate))
                {
                    saveable.LoadState(savedSate);
                }
            }
        }

    }
}
