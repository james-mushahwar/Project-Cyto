using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General.Identification;
using _Scripts._Game.General.SaveLoad;
using _Scripts._Game.General.SceneLoading;
using Assets._Scripts._Game.General.SceneLoading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts._Game.General.Managers{

    public class RespawnManager : Singleton<RespawnManager>, ISaveable, IManager
    {
        #region Respawn
        private GameObject _respawnGO;
        private string _respawnGOID = "";

        public GameObject RespawnGO
        {
            get
            {
                GameObject saveStation = null;
                if (_respawnGO == null)
                {
                    // try and find respawn gameobject
                    foreach (var saveable in FindObjectsOfType<SaveableEntity>())
                    {
                        if (saveable.gameObject.tag == "Save Station")
                        {
                            SceneField areaSpawnField = AssetManager.Instance.IndexToSceneField(saveable.gameObject.scene.buildIndex);
                            if (areaSpawnField.SceneName == GameStateManager.Instance.AreaSpawnScene.SceneName)
                            {
                                saveStation = saveable.gameObject;
                            }
                        }
                        if (saveable.Id == _respawnGOID)
                        {
                            _respawnGO = saveable.gameObject;
                            break;
                        }
                    }
                }

                if (_respawnGO == null)
                {
                    if (saveStation != null)
                    {
                        _respawnGO = saveStation;
                    }
                }

                return _respawnGO;
            }
        }
        public string RespawnGOID { get => _respawnGOID; set => _respawnGOID = value; }
        #endregion

        #region Doorways And Corridors
        private GameObject _doorwayGO;
        private string _doorwayGOID = "";
        private int _doorwayIndex;

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
        public int DoorwayIndex { get => _doorwayIndex; set => _doorwayIndex = value; }
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

        public void ManagedPreInGameLoad()
        {
             
        }

        public void ManagedPostInGameLoad()
        {
             
        }

        public void ManagedPreMainMenuLoad()
        {
             
        }

        public void ManagedPostMainMenuLoad()
        {
             
        }
    }
    
}
