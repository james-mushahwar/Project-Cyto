using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General.SaveLoad;
using _Scripts._Game.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts._Game.General.Managers{
    [RequireComponent(typeof(SaveableEntity))]
    public class GameStateManager : Singleton<GameStateManager>, ISaveable
    {
        private int _saveIndex = -1;        //what save file index
        private int _sceneSpawnIndex = -1;  //what scene to load first

        public int SceneSpawnIndex
        {
            get
            {
                if (_sceneSpawnIndex == -1)
                {
                    _sceneSpawnIndex = AssetManager.Instance.DefaultNewSaveSceneIndex;
                }
                return _sceneSpawnIndex;
            }
            set => _sceneSpawnIndex = value;
        }
        private EGameType _gameType;

        public int SaveIndex
        {
            get
            {
                if (_saveIndex == -1)
                {
                    _saveIndex = 0;
                }

                return _saveIndex;
            }
        }
        public EGameType GameType { get => _gameType; }

        public bool IsGameRunning
        {
            get => GameType == EGameType.InGame && PlayerEntity.Instance != null;
        }

        [Header("Loading states")] 
        private AsyncOperation _playerSceneLoading;

        [Header("Component references")]
        private SaveableEntity _saveableEntity;

        [Header("Manager references")]
        [SerializeField]
        private GameObject _inGameManagerGroup;
        private GameObject[] _inGameManagers;

        [Header("Saveable references")]
        [SerializeField]
        private SaveableEntity[] _startInGameSaveableEntityLoad;

        private void Awake()
        {
            Application.quitting += OnQuit;
            _saveableEntity = GetComponent<SaveableEntity>();

            _inGameManagers = new GameObject[_inGameManagerGroup.transform.childCount];
            int i = 0;
            foreach (Transform managerTransform in _inGameManagerGroup.transform)
            {
                _inGameManagers[i] = managerTransform.gameObject;
                i++;
            }
        }

        void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;

            MainMenuCheck(SceneManager.GetActiveScene());

            PlayerSceneCheck();
        }

        void Update()
        {
            if (_playerSceneLoading != null)
            {
                if (_playerSceneLoading.isDone)
                {
                    _playerSceneLoading = null;
                }
            }
        }

        void OnQuit()
        {
            SaveableEntity.IsQuitting = true;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            MainMenuCheck(scene);

            PlayerSceneCheck();
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

        private void PlayerSceneCheck()
        {
            Debug.Log("Player scene check");
            bool shouldPlayerSceneExist = _gameType == EGameType.InGame;

            bool doesPlayerSceneExist = PlayerEntity.Instance != null;

            if (doesPlayerSceneExist && !shouldPlayerSceneExist)
            {
                Debug.Log("Unloading player scene");
                SceneManager.UnloadSceneAsync("PlayerScene");
            }
            else if (!doesPlayerSceneExist && shouldPlayerSceneExist)
            {
                Debug.Log("Loading player scene");
                if (_playerSceneLoading == null)
                {
                    //_playerSceneLoading = SceneManager.LoadSceneAsync("PlayerScene", LoadSceneMode.Additive);
                }
            }
        }

        public void StartGame(bool newGame = false)
        {
            int saveIndex = -1;
            // new game
            if (newGame == true)
            {
                saveIndex = 0;
            }
            else
            {
                //continue last save
                saveIndex = SaveLoadSystem.Instance.LastSaveIndex;
            }
            _saveIndex = saveIndex;
            Debug.Log("Save index is: " + _saveIndex);

            LoadInGame();
        }

        public void LoadGame(int index = 0)
        {
            _saveIndex = index;

            LoadInGame();
        }

        private void LoadInGame()
        {
            //save last opened save file index
            SaveLoadSystem.Instance.LastSaveIndex = _saveIndex;
            SaveLoadSystem.Instance.SaveGamePrefs();

            // in game managers enable
            _inGameManagerGroup.SetActive(true);

            // run load for managers that load on start
            foreach (SaveableEntity saveableEntity in _startInGameSaveableEntityLoad)
            {
                SaveLoadSystem.Instance.OnEnableLoadState(ESaveTarget.Saveable, saveableEntity);
            }

            // load scene index from save
            SaveLoadSystem.Instance.OnEnableLoadState(ESaveTarget.Saveable, _saveableEntity);

            AssetManager.Instance.LoadSceneByIndex(SceneSpawnIndex);

            // load playerscene
            if (_playerSceneLoading == null)
            {
                _playerSceneLoading = SceneManager.LoadSceneAsync("PlayerScene", LoadSceneMode.Additive);
            }

            UIManager.Instance.ShowMainMenu(false);
        }

        public void SetSpawnIndex(int index)
        {
            SceneSpawnIndex = index;
            Debug.Log("Saved scene index is: " + SceneSpawnIndex);
        }

        public void QuitToMainMenu()
        {
            PauseManager.Instance.TogglePause();

            SceneManager.LoadScene(0);
            UIManager.Instance.ShowMainMenu(true);

            _inGameManagerGroup.SetActive(false);
        }

        //ISaveable
        [System.Serializable]
        private struct SaveData
        {
            public int sceneSpawnIndex;  //what scene to load first
        }

        public object SaveState()
        {
            return new SaveData()
            {
                sceneSpawnIndex = SceneSpawnIndex
            };
        }

        public void LoadState(object state)
        {
            SaveData saveData = (SaveData)state;

            SceneSpawnIndex = saveData.sceneSpawnIndex <= 0 ? AssetManager.Instance.DefaultNewSaveSceneIndex : saveData.sceneSpawnIndex;
        }

    }
    
}
