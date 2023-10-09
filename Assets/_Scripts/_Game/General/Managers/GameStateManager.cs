using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts._Game.General.SaveLoad;
using _Scripts._Game.Player;
using Assets._Scripts._Game.General.SceneLoading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts._Game.General.Managers{
    [RequireComponent(typeof(SaveableEntity))]
    public class GameStateManager : Singleton<GameStateManager>, ISaveable
    {
        private int _saveIndex = -1;        //what save file index
        private SceneField _zoneSpawnScene;
        public SceneField ZoneSpawnScene
        {
            get
            {
                if (_zoneSpawnScene == null)
                {
                    _zoneSpawnScene = AssetManager.Instance.DefaultNewSaveZoneScene;
                }
                return _zoneSpawnScene;
            }
            set => _zoneSpawnScene = value;
        }
        private SceneField _currentZoneScene;
        public SceneField CurrentZoneScene
        {
            get { return _currentZoneScene; }
        }

        private SceneField _areaSpawnScene;
        public SceneField AreaSpawnScene
        {
            get
            {
                if (_areaSpawnScene == null)
                {
                    _areaSpawnScene = AssetManager.Instance.DefaultNewSaveAreaScene;
                }
                return _areaSpawnScene;
            }
            set => _areaSpawnScene = value;
        }

        private SceneField _currentAreaScene;
        public SceneField CurrentAreaScene
        {
            get
            {
                if (_currentAreaScene == null)
                {
                    _currentAreaScene = AssetManager.Instance.DefaultNewSaveAreaScene;
                }
                return _currentAreaScene;
            }
            set => _currentAreaScene = value;
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

        private Coroutine _loadGameCoroutine;
        private Coroutine _loadNewZoneCoroutine;

        public bool IsLoadInProgress
        {
            get { return _loadGameCoroutine != null || _loadNewZoneCoroutine != null; }
        }

        private ELoadType _loadType = ELoadType.NONE;
        public ELoadType LoadType { get => _loadType; set => _loadType = value;  }

        private static bool _isQuitting = false;
        public static bool IsQuitting { get => _isQuitting; set => _isQuitting = value; }

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

        //Managers
        private IManager[] _managers;

        protected override void Awake()
        {
            //Managers
            _managers = GameObject.FindObjectsOfType<MonoBehaviour>(true).OfType<IManager>().ToArray();

            IsQuitting = false;
            Application.quitting += OnQuit;
            _saveableEntity = GetComponent<SaveableEntity>();

            _inGameManagers = new GameObject[_inGameManagerGroup.transform.childCount];
            int i = 0;
            foreach (Transform managerTransform in _inGameManagerGroup.transform)
            {
                _inGameManagers[i] = managerTransform.gameObject;
                i++;
            }
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void Start()
        {

            //MainMenuCheck(SceneManager.GetActiveScene());

            //PlayerSceneCheck();
        }

        void Update()
        {
            
        }

        void OnQuit()
        {
            IsQuitting = true;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            MainMenuCheck(scene);

            //PlayerSceneCheck();
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
            if (_loadGameCoroutine != null)
            {
                return;
            }

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

            _loadType = newGame ? ELoadType.NewGame : ELoadType.LoadSave;

            _saveIndex = saveIndex;
            Debug.Log("Save index is: " + _saveIndex);

            _loadGameCoroutine = StartCoroutine(LoadGameEnumerator());
        }

        public void LoadGame(int index = 0)
        {
            if (_loadGameCoroutine != null)
            {
                return;
            }

            _loadGameCoroutine = StartCoroutine(LoadGameEnumerator(index));
        }

        public IEnumerator LoadGameEnumerator(int index = 0)
        {
            _saveIndex = index;

            IEnumerator _loadGameEnumerator = LoadInGame();
            while (_loadGameEnumerator.MoveNext() != false)
            {
                yield return null;
            }

            _loadGameCoroutine = null;
            _loadType = ELoadType.NONE;
        }

        private IEnumerator LoadInGame()
        {
            PreInGameLoad();

            UIManager.Instance.ShowMainMenu(false);
            UIManager.Instance.ShowLoadingScreen(true);
            IsQuitting = false;
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

            AsyncOperation persistantSceneAsync = SceneManager.LoadSceneAsync("PersistantInGameScene", LoadSceneMode.Additive);
            while (!persistantSceneAsync.isDone)
            {
                yield return null;
            }

            SceneField zoneScene = ZoneSpawnScene;
            SceneField areaScene = AreaSpawnScene;
            IEnumerator _loadZoneAreaEnumerator = AssetManager.Instance.LoadZoneAndArea(zoneScene, areaScene);
            while (_loadZoneAreaEnumerator.MoveNext() != false)
            {
                yield return null;
            }

            // load playerscene
            if (_playerSceneLoading == null)
            {
                _playerSceneLoading = SceneManager.LoadSceneAsync("PlayerScene", LoadSceneMode.Additive);

                if (_playerSceneLoading != null)
                {
                    while (!_playerSceneLoading.isDone)
                    {
                        yield return null;
                    }
                    _playerSceneLoading = null;
                    // move player to correct spawn location
                    RespawnManager.Instance.RespawnObject(PlayerEntity.Instance.gameObject);
                }
            }

            AsyncOperation unloadMainMenuAsync = SceneManager.UnloadSceneAsync("Main_Menu");
            while (!unloadMainMenuAsync.isDone)
            {
                yield return null;
            }

            UIManager.Instance.ShowLoadingScreen(false);

            FollowCamera.Instance.ActivateCamera(true);

            PostInGameLoad();
        }

        public void SetAreaSpawnScene(int index)
        {
            _zoneSpawnScene = AssetManager.Instance.IndexToSceneField(CurrentZoneScene);
            _areaSpawnScene = AssetManager.Instance.IndexToSceneField(index);
            Debug.Log("Saved scene index is: " + _areaSpawnScene);
        }

        public void EnterZoneAndArea(SceneField zoneScene, SceneField areaScene)
        {
            if (areaScene == CurrentAreaScene)
            {
                return;
            }

            _currentAreaScene = areaScene;
            AssetManager.Instance.UpdateStateArea();
        }

        public void QuitToMainMenu()
        {
            PreMainMenuLoad();
            AudioManager.Instance.StopAllAudioTracks();

            PauseManager.Instance.TogglePause();

            SceneManager.LoadScene("Main_Menu");
            UIManager.Instance.ShowMainMenu(true);

            PostMainMenuLoad();
            _inGameManagerGroup.SetActive(false);
        }

        public void TryNewZoneAndArea(SceneField zoneScene, SceneField areaScene)
        {
            if (_loadNewZoneCoroutine != null)
            {
                return;
            }

            _loadNewZoneCoroutine = StartCoroutine(LoadNewZoneAndArea(zoneScene, areaScene));
        }

        public IEnumerator LoadNewZoneAndArea(SceneField zoneScene, SceneField areaScene)
        {
            //show camera transition

            // loading screen
            UIManager.Instance.ShowLoadingScreen(true);

            IEnumerator _loadZoneAreaEnumerator = AssetManager.Instance.LoadZoneAndArea(zoneScene, areaScene);
            while (_loadZoneAreaEnumerator.MoveNext() != false)
            {
                yield return null;
            }

            // find correct spawn location
            _currentZoneScene = zoneScene;
            _currentAreaScene = areaScene;

            // enter doorway
            RespawnManager.Instance.EnterDoorway(PlayerEntity.Instance.gameObject);

            UIManager.Instance.ShowLoadingScreen(false);
        }

        // managers
        private void PreInGameLoad()
        {
            for (int i = 0; i < _managers.Length; i++)
            {
                _managers[i].PreInGameLoad();
            }
        }
        private void PostInGameLoad()
        {
            for (int i = 0; i < _managers.Length; i++)
            {
                _managers[i].PostInGameLoad();
            }
        }
        private void PreMainMenuLoad()
        {
            for (int i = 0; i < _managers.Length; i++)
            {
                _managers[i].PreMainMenuLoad();
            }
        }
        private void PostMainMenuLoad()
        {
            for (int i = 0; i < _managers.Length; i++)
            {
                _managers[i].PostMainMenuLoad();
            }
        }

        //ISaveable
        [System.Serializable]
        private struct SaveData
        {
            public int zoneSpawnIndex;  //what zone to load
            public int areaSpawnIndex;  //what area to load in zone

        }

        public object SaveState()
        {
            int zSpawnIndex = AssetManager.Instance.SceneNameToBuildIndex(ZoneSpawnScene);
            int aSpawnIndex = AssetManager.Instance.SceneNameToBuildIndex(AreaSpawnScene);
            return new SaveData()
            {
                zoneSpawnIndex = zSpawnIndex,
                areaSpawnIndex = aSpawnIndex,
            };
        }

        public void LoadState(object state)
        {
            SaveData saveData = (SaveData)state;

            SceneField zSpawnField = AssetManager.Instance.IndexToSceneField(saveData.zoneSpawnIndex);
            _zoneSpawnScene = zSpawnField == null ? AssetManager.Instance.DefaultNewSaveZoneScene : zSpawnField;
            _currentZoneScene = ZoneSpawnScene;

            SceneField aSpawnField = AssetManager.Instance.IndexToSceneField(saveData.areaSpawnIndex);
            _areaSpawnScene = aSpawnField == null ? AssetManager.Instance.DefaultNewSaveAreaScene : aSpawnField;
            _currentAreaScene = AreaSpawnScene;
        }

    }
    
}
