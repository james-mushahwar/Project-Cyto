using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General.SaveLoad;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts._Game.General.Managers{
    [RequireComponent(typeof(SaveableEntity))]
    public class GameStateManager : Singleton<GameStateManager>, ISaveable
    {
        private int _saveIndex = -1;        //what save file index
        private int _sceneSpawnIndex = -1;  //what scene to load first
        private EGameType _gameType;

        public EGameType GameType { get => _gameType; }

        [Header("Component references")]
        private SaveableEntity _saveableEntity;

        void Awake()
        {
            _saveableEntity = GetComponent<SaveableEntity>();

        }

        void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;

            MainMenuCheck(SceneManager.GetActiveScene());
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            MainMenuCheck(scene);
        }

        private void MainMenuCheck(Scene scene)
        {
            if (scene.name == "Main_Menu")
            {
                UIManager.Instance.ShowMainMenu(true);
                InputManager.Instance.TryEnableActionMap(EInputSystem.Menu);
                _gameType = EGameType.MainMenu;
            }
            else
            {
                _gameType = EGameType.InGame;
            }
        }

        public void StartGame()
        {
            _saveIndex = 0;

            // load scene index from save
            SaveLoadSystem.Instance.OnEnableLoadState(_saveableEntity);

            AssetManager.Instance.LoadSceneByIndex(_sceneSpawnIndex);

            UIManager.Instance.ShowMainMenu(false);

            //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        }

        public void SetSpawnIndex(int index)
        {
            _sceneSpawnIndex = index;
            Debug.Log("Saved scene index is: " + _sceneSpawnIndex);
        }

        //ISaveable
        [System.Serializable]
        private struct SaveData
        {
            public int currentSaveIndex; //what save file index
            public int sceneSpawnIndex;  //what scene to load first
        }

        public object SaveState()
        {
            return new SaveData()
            {
                currentSaveIndex = _saveIndex < 0 ? 0 : _saveIndex,
                sceneSpawnIndex = _sceneSpawnIndex < 0 ? 0 : _sceneSpawnIndex
            };
        }

        public void LoadState(object state)
        {
            SaveData saveData = (SaveData)state;

            _saveIndex = saveData.currentSaveIndex;
            _sceneSpawnIndex = saveData.sceneSpawnIndex;
            
        }

    }
    
}
