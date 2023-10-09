using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General.Identification;
using _Scripts._Game.General.SaveLoad;
using _Scripts._Game.General.SceneLoading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts._Game.General.Managers{

    public class RespawnManager : Singleton<RespawnManager>, ISaveable, IManager
    {
        #region Respawn
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
        #endregion

        #region Doorways And Corridors
        private GameObject _doorwayGO;
        private string _doorwayGOID = "empty";

        public GameObject DoorWayGO
        {
            get
            {
                if (_doorwayGO == null)
                {
                    // try and find respawn gameobject
                    foreach (var doorway in FindObjectsOfType<Doorway>())
                    {
                        RuntimeID runtimeID = doorway.GetComponentInChildren<RuntimeID>();
                        if (runtimeID == null)
                        {
                            continue;
                        }
                        if (runtimeID.Id == _doorwayGOID)
                        {
                            _doorwayGO = runtimeID.gameObject;
                            break;
                        }
                    }
                }

                return _doorwayGO;
            }
        }
        public string DoorwayGOID { get => _doorwayGOID; set => _doorwayGOID = value; }
        #endregion

        #region Safe respawn
        #endregion

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

        public void EnterDoorway(GameObject go)
        {
            if (DoorWayGO == null)
            {
                return;
            }
            go.transform.position = DoorWayGO.transform.position;
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
