using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General.SaveLoad;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts._Game.General.Managers{

    public class RespawnManager : Singleton<RespawnManager>, ISaveable
    {
        private GameObject _respawnGO;
        private string _respawnGOID = "empty";

        public GameObject RespawnGO
        {
            get
            {
                if (_respawnGO == null)
                {
                    // try and find respawn gameobject
                    foreach (var saveable in FindObjectsOfType<SaveableEntity>())
                    {
                        if (saveable.Id == _respawnGOID)
                        {
                            _respawnGO = saveable.gameObject;
                            break;
                        }
                    }
                }

                return _respawnGO;
            }
        }
        public string RespawnGOID { get => _respawnGOID; set => _respawnGOID = value; }

        void OnDisable()
        {
            _respawnGOID = string.Empty;
            _respawnGO = null;
        }

        public void RespawnObject(GameObject go)
        {
            if (RespawnGO == null)
            {
                return;
            }
            go.transform.position = RespawnGO.transform.position;
        }

        //ISaveable
        [System.Serializable]
        public struct SaveData
        {
            public string respawnGOID;
        }
        public void LoadState(object state)
        {
            SaveData saveData = (SaveData)state;

            _respawnGOID = saveData.respawnGOID;
            Debug.Log("Loading respawn manager");
        }
        public object SaveState()
        {
            SaveData saveData = new SaveData();

            saveData.respawnGOID = _respawnGOID;
            Debug.Log("RespawnGOID is " + _respawnGOID);
            return saveData;
        }
    }
    
}
