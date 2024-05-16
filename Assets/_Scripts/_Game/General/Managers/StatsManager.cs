using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts._Game.General.SaveLoad;

namespace _Scripts._Game.General.Managers{

    [System.Serializable]
    public struct FEntityStats
    {
        [SerializeField]
        private EEntity _entity;
        [SerializeField]
        private float _maxHealth;
        [SerializeField]
        private float _maxBondableHealth;

        public float MaxHealth { get => _maxHealth; }
        public float MaxBondableHealth => _maxBondableHealth;
    }

    public class StatsManager : Singleton<StatsManager>, ISaveable, IManager
    {
        private SaveableEntity _saveableEntity;

        private float[] _completionStats = new float[(int)EStatsType.COUNT]; 
        //[SerializeField]
        //private FEntityStats[] _entityStats = new FEntityStats[(int)EEntity.COUNT];

        [SerializeField] 
        private EEntityFEntityStatsDictionary _entityStatsDict = new EEntityFEntityStatsDictionary();

        // Start is called before the first frame update
        void Start()
        {
            _saveableEntity = GetComponentInChildren<SaveableEntity>();
        }
    
        // Update is called once per frame
        public void ManagedTick()
        {
            if (GameStateManager.Instance.IsGameRunning && !PauseManager.Instance.IsPaused)
            {
                _completionStats[(int)EStatsType.TimePlayed] += Time.unscaledDeltaTime;
            }
        }

        // stats
        public void AddStat(EStatsType type, int amount)
        {
            int index = (int)type;
            if (index >= (int)EStatsType.COUNT)
            {
                return;
            }

            _completionStats[index] += amount;
        }

        public float GetStat(EStatsType type)
        {
            int index = (int)type;
            if (index >= (int)EStatsType.COUNT)
            {
                return -1;
            }

            return _completionStats[index];
        }

        // Entity stats
        public FEntityStats GetEntityStat(EEntity type)
        {
            int index = (int)type;
            if (index >= (int)EStatsType.COUNT)
            {
                return new FEntityStats();
            }

            FEntityStats foundEntityStats;
            _entityStatsDict.TryGetValue(type, out foundEntityStats);
            return foundEntityStats;
        }

        [System.Serializable]
        public struct SaveData
        {
            public float[] _completionStats;
        }

        public object SaveState()
        {
            SaveData _saveData =  new SaveData()
            {
                _completionStats = new float[(int)EStatsType.COUNT]
            };

            for (int i = 0; i < (int)EStatsType.COUNT; ++i)
            {
                _saveData._completionStats[i] = this._completionStats[i];
            }

            return _saveData;
        }

        public void LoadState(object state)
        {
            SaveData saveData = (SaveData)state;

            this._completionStats = saveData._completionStats;
            Debug.Log("Completion time is: " + _completionStats[(int)EStatsType.TimePlayed]);
        }

        public void ManagedPreInGameLoad()
        {
             
        }

        public void ManagedPostInGameLoad()
        {
             
        }

        public void ManagedPreMainMenuLoad()
        {
            SaveLoadSystem.Instance?.OnDisableSaveState(ESaveTarget.Saveable, _saveableEntity);
        }

        public void ManagedPostMainMenuLoad()
        {
             
        }
    }
    
}
