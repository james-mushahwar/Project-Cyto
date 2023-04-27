using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts._Game.General.SaveLoad;

namespace _Scripts._Game.General.Managers{
    
    public enum EStatsType
    {
        // General
        TimePlayed,

        //World
        FriendsFound,
        HostilesFound,
        AreasFound,

        // Player actions
        Bonds,
        Possessions,
        Bounces,
        Phases,
        Exposures, 

        COUNT
    }

    public enum EEntity
    {
        Player,
        BombDroid,
        MushroomArcher,
        DaggerMushroom,
        COUNT
    }

    [System.Serializable]
    public struct FEntityStats
    {
        [SerializeField]
        private EEntity _entity;
        [SerializeField]
        private float _maxHealth;

        public float MaxHealth { get => _maxHealth; }
    }

    public class StatsManager : Singleton<StatsManager>, ISaveable
    {
        private int[] _completionStats = new int[(int)EStatsType.COUNT]; 
        [SerializeField]
        private FEntityStats[] _entityStats = new FEntityStats[(int)EEntity.COUNT]; 

        // Start is called before the first frame update
        void Start()
        {
            
        }
    
        // Update is called once per frame
        void Update()
        {
            
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

        public int GetStat(EStatsType type)
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

            return _entityStats[index];
        }

        [System.Serializable]
        private struct SaveData
        {
            public int[] _completionStats;
        }

        public object SaveState()
        {
            SaveData _saveData =  new SaveData()
            {
                _completionStats = new int[(int)EStatsType.COUNT]
            };

            for (int i = 0; i < (int)EStatsType.COUNT; ++i)
            {
                _completionStats[i] = this._completionStats[i];
            }

            return _saveData;
        }

        public void LoadState(object state)
        {
            SaveData saveData = (SaveData)state;

            this._completionStats = saveData._completionStats;
        }
    }
    
}
