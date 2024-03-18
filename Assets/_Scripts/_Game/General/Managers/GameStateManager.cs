using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts._Game.General.SaveLoad;
using _Scripts._Game.Player;
using _Scripts._Game.Sequencer;
using Assets._Scripts._Game.General.SceneLoading;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace _Scripts._Game.General.Managers{
    [RequireComponent(typeof(SaveableEntity))]
    public class GameStateManager : Singleton<GameStateManager>, ISaveable
    {
        private EGameState _gameState;
        private EGameState _pendingGameState;
        public EGameState GameState { get { return _gameState; } }

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
            get
            {
                if (_currentZoneScene == null)
                {
                    _currentZoneScene = ZoneSpawnScene;
                }
                return _currentZoneScene;
            }
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
        private GameObject _alwaysOnManagersGroup;
        [SerializeField]
        private GameObject _inGameManagerGroup;

        [Header("Saveable references")]
        [SerializeField]
        private SaveableEntity[] _startInGameSaveableEntityLoad;

        //Managers
        private IManager[] _managers;
        private IManager[] _alwaysOnManagers;
        private IManager[] _inGameManagers;
        [SerializeField]
        private AIManager _aiManagerPrefab;
        private AIManager _aiManager;
        [SerializeField]
        private AssetManager _assetManagerPrefab;
        private AssetManager _assetManager;
        [SerializeField]
        private AudioManager _audioManagerPrefab;
        private AudioManager _audioManager;
        [SerializeField]
        private CorpseManager _corpseManagerPrefab;
        private CorpseManager _corpseManager;
        [SerializeField]
        private DialogueManager _dialogueManagerPrefab;
        private DialogueManager _dialogueManager;
        [SerializeField]
        private FeedbackManager _feedbackManagerPrefab;
        private FeedbackManager _feedbackManager;
        [SerializeField]
        private InputManager _inputManagerPrefab;
        private InputManager _inputManager;
        [SerializeField]
        private InteractableManager _interactableManagerPrefab;
        private InteractableManager _interactableManager;
        [SerializeField]
        private LightingManager _lightManagerPrefab;
        private LightingManager _lightManager;
        [SerializeField]
        private LogicManager _logicManagerPrefab;
        private LogicManager _logicManager;
        [SerializeField]
        private ParticleManager _particleManagerPrefab;
        private ParticleManager _particleManager;
        [SerializeField]
        private PauseManager _pauseManagerPrefab;
        private PauseManager _pauseManager;
        [SerializeField]
        private ProjectileManager _projectileManagerPrefab;
        private ProjectileManager _projectileManager;
        [SerializeField]
        private RespawnManager _respawnManagerPrefab;
        private RespawnManager _respawnManager;
        [SerializeField]
        private RuntimeIDManager _runtimeIDManagerPrefab;
        private RuntimeIDManager _runtimeIDManager;
        [SerializeField]
        private SpawnManager _spawnManagerPrefab;
        private SpawnManager _spawnManager;
        [SerializeField]
        private StatsManager _statsManagerPrefab;
        private StatsManager _statsManager;
        [SerializeField]
        private TargetManager _targetManagerPrefab;
        private TargetManager _targetManager;
        [SerializeField]
        private TimeManager _timeManagerPrefab;
        private TimeManager _timeManager;
        [SerializeField]
        private UIManager _uiManagerPrefab;
        private UIManager _uiManager;
        [SerializeField]
        private VolumeManager _volumeManagerPrefab;
        private VolumeManager _volumeManager;
        [SerializeField]
        private SequencerManager _sequencerManagerPrefab;
        private SequencerManager _sequencerManager;
        //extra
        [SerializeField]
        private DebugManager _debugManagerPrefab;
        private DebugManager _debugManager;
        [SerializeField]
        private TaskManager _taskManagerPrefab;
        private TaskManager _taskManager;

        private void Log(string log)
        {
            Debug.Log("GameStateManager: " + log);
        }

        protected override void Awake()
        {
            _gameState = EGameState.NONE;
            _pendingGameState = EGameState.NONE;

            bool done = RequestNewGameState(EGameState.InitialiseGameState, false);
            if (!done)
            {
                throw new Exception("Initialise Managers failed");
            }
        }

        protected void GameState_InitaliseGameState()
        {
            //Create Managers
            _taskManager = Instantiate<TaskManager>(_taskManagerPrefab, _alwaysOnManagersGroup.transform);
            _debugManager = Instantiate<DebugManager>(_debugManagerPrefab, _alwaysOnManagersGroup.transform);

            _assetManager       = Instantiate<AssetManager>(_assetManagerPrefab, _alwaysOnManagersGroup.transform);
            _audioManager       = Instantiate<AudioManager>(_audioManagerPrefab, _alwaysOnManagersGroup.transform);
            _corpseManager      = Instantiate<CorpseManager>(_corpseManagerPrefab, _inGameManagerGroup.transform);
            _dialogueManager    = Instantiate<DialogueManager>(_dialogueManagerPrefab, _alwaysOnManagersGroup.transform);
            _feedbackManager    = Instantiate<FeedbackManager>(_feedbackManagerPrefab, _inGameManagerGroup.transform);
            _aiManager          = Instantiate<AIManager>(_aiManagerPrefab, _alwaysOnManagersGroup.transform);
            _inputManager       = Instantiate<InputManager>(_inputManagerPrefab, _alwaysOnManagersGroup.transform);
            _interactableManager= Instantiate<InteractableManager>(_interactableManagerPrefab, _inGameManagerGroup.transform);
            _lightManager       = Instantiate<LightingManager>(_lightManagerPrefab, _alwaysOnManagersGroup.transform);
            _logicManager       = Instantiate<LogicManager>(_logicManagerPrefab, _inGameManagerGroup.transform);
            _particleManager    = Instantiate<ParticleManager>(_particleManagerPrefab, _alwaysOnManagersGroup.transform);
            _pauseManager       = Instantiate<PauseManager>(_pauseManagerPrefab, _alwaysOnManagersGroup.transform);
            _projectileManager  = Instantiate<ProjectileManager>(_projectileManagerPrefab, _inGameManagerGroup.transform);
            _respawnManager     = Instantiate<RespawnManager>(_respawnManagerPrefab, _inGameManagerGroup.transform);
            _runtimeIDManager   = Instantiate<RuntimeIDManager>(_runtimeIDManagerPrefab, _inGameManagerGroup.transform);
            _spawnManager       = Instantiate<SpawnManager>(_spawnManagerPrefab, _inGameManagerGroup.transform);
            _statsManager       = Instantiate<StatsManager>(_statsManagerPrefab, _alwaysOnManagersGroup.transform);
            _targetManager      = Instantiate<TargetManager>(_targetManagerPrefab, _inGameManagerGroup.transform);
            _timeManager        = Instantiate<TimeManager>(_timeManagerPrefab, _inGameManagerGroup.transform);
            _uiManager          = Instantiate<UIManager>(_uiManagerPrefab, _alwaysOnManagersGroup.transform);
            _volumeManager      = Instantiate<VolumeManager>(_volumeManagerPrefab, _alwaysOnManagersGroup.transform);
            _sequencerManager   = Instantiate<SequencerManager>(_sequencerManagerPrefab, _inGameManagerGroup.transform);

            _alwaysOnManagers = new IManager[_alwaysOnManagersGroup.transform.childCount];
            int i = 0;
            foreach (Transform managerTransform in _alwaysOnManagersGroup.transform)
            {
                _alwaysOnManagers[i] = managerTransform.gameObject.GetComponent<IManager>();
                i++;
            }

            _inGameManagers = new IManager[_inGameManagerGroup.transform.childCount];
            i = 0;
            foreach (Transform managerTransform in _inGameManagerGroup.transform)
            {
                _inGameManagers[i] = managerTransform.gameObject.GetComponent<IManager>();
                i++;
            }

            _inGameManagerGroup.SetActive(false);

            _managers = GameObject.FindObjectsOfType<MonoBehaviour>(true).OfType<IManager>().ToArray();

            IsQuitting = false;
            Application.quitting += OnQuit;
            _saveableEntity = GetComponent<SaveableEntity>();

            SceneManager.sceneLoaded += OnSceneLoaded;

            bool done = RequestNewGameState(EGameState.PostInitialiseGameState, false);
            if (!done)
            {
                throw new Exception("request new game state failed");
            }
        }

        //post initialise game state
        void GameState_PostInitialiseGameState()
        {
            GameState_ManagerPostInitialiseGameState();
        }
        private void GameState_ManagerPostInitialiseGameState()
        {
            for (int i = 0; i < _managers.Length; i++)
            {
                _managers[i].ManagedPostInitialiseGameState();
            }
        }

        // load main menu
        void GameState_LoadMainMenu()
        {

        }


        // in main menu
        void GameState_MainMenu()
        {

        }

        // playing game
        void GameState_PlayingGame()
        {

        }

        void Update()
        {
            for (int i = 0; i < _alwaysOnManagers.Length; i++)
            {
                IManager manager = _alwaysOnManagers[i];
                manager.ManagedTick();
            }

            bool done = false;
            switch (GameState)
            {
                case EGameState.InitialiseGameState:
                    done = RequestNewGameState(EGameState.PostInitialiseGameState, false);
                    if (!done)
                    {
                        throw new Exception("request new game state failed");
                    }
                    break;
                case EGameState.PostInitialiseGameState:
                    done = RequestNewGameState(EGameState.LoadMainMenu, false);
                    if (!done)
                    {
                        throw new Exception("request new game state failed");
                    }
                    break;
                case EGameState.LoadMainMenu:
                    done = RequestNewGameState(EGameState.MainMenu, false);
                    if (!done)
                    {
                        throw new Exception("request new game state failed");
                    }
                    break;
                case EGameState.MainMenu:
                    // request new state on button click
                    break;
                case EGameState.LoadGame:
                    break;
                case EGameState.PostLoadGame:
                    break;
                case EGameState.RestoreSave:
                    break;
                case EGameState.PrePlayGame:
                    break;
                case EGameState.PlayingGame:
                    for (int i = 0; i < _inGameManagers.Length; i++)
                    {
                        IManager manager = _inGameManagers[i];
                        manager.ManagedTick();
                    }
                    break;
                case EGameState.PreTeardownGame:
                    break;

            }
            //if (IsGameRunning)
            //{
            //    for (int i = 0; i < _managers.Length; i++)
            //    {
            //        IManager manager = _managers[i];
            //        manager.ManagedTick();
            //    }
            //}
        }

        public bool RequestNewGameState(EGameState newGameState, bool doFade)
        {
            if (newGameState == _gameState || _pendingGameState != EGameState.NONE)
            {
                return false;
            }

            _pendingGameState = newGameState;
            SetNextPlayState();
            return true;
        }

        private void SetNextPlayState()
        {
            _gameState = _pendingGameState;
            _pendingGameState = EGameState.NONE;

            Log("Set play state = " + _gameState);

            switch (_gameState)
            {
                case EGameState.InitialiseGameState:
                    GameState_InitaliseGameState();
                    break;
                case EGameState.PostInitialiseGameState:
                    GameState_PostInitialiseGameState();
                    break;
                case EGameState.LoadMainMenu:
                    GameState_LoadMainMenu();
                    break;
                case EGameState.MainMenu:
                    GameState_MainMenu();
                    break;
                case EGameState.LoadGame:
                    break;
                case EGameState.PostLoadGame:
                    break;
                case EGameState.RestoreSave:
                    break;
                case EGameState.PrePlayGame:
                    break;
                case EGameState.PlayingGame:
                    GameState_PlayingGame();
                    break;
                case EGameState.PreTeardownGame:
                    break;
            }
        }

        void OnQuit()
        {
            IsQuitting = true;
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

            RequestNewGameState(EGameState.PlayingGame, false);
        }

        private IEnumerator LoadInGame()
        {
            RequestNewGameState(EGameState.LoadGame, false);

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
                    // lock player movement
                    PlayerEntity.Instance.FreezeAllMovement(true);
                    // move player to correct spawn location
                    RespawnManager.Instance.RespawnObject(PlayerEntity.Instance.gameObject);
                }
            }

            RequestNewGameState(EGameState.PostLoadGame, false);

            AsyncOperation unloadMainMenuAsync = SceneManager.UnloadSceneAsync("Main_Menu");
            while (!unloadMainMenuAsync.isDone)
            {
                yield return null;
            }

            UIManager.Instance.ShowLoadingScreen(false);

            FollowCamera.Instance.ActivateCamera(true);
            // unlock player movement
            PlayerEntity.Instance.FreezeAllMovement(false);

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
            AssetManager.Instance.UpdateStateArea(areaScene);
        }

        public void QuitToMainMenu()
        {
            RequestNewGameState(EGameState.PreTeardownGame, false);

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
            //unbond if bonded
            if (PlayerEntity.Instance.Possessed != null)
            {
                PlayerEntity.Instance.Possessed.OnDispossess();
                yield return TaskManager.Instance.WaitForSecondsPool.Get(1.0f);
            }

            // lock player movement
            PlayerEntity.Instance.FreezeAllMovement(true);

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

            // unlock player movement
            PlayerEntity.Instance.FreezeAllMovement(false);

            FollowCamera.Instance.ActivateCamera(true);

            UIManager.Instance.ShowLoadingScreen(false);

            _loadNewZoneCoroutine = null;
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
