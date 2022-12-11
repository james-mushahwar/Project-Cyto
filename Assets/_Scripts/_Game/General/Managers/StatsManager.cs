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
        Possessions,
        Bounces,
        Breaks, 

        COUNT
    }

    public class StatsManager : Singleton<StatsManager>, ISaveable
    {
        private int[] _stats = new int[(int)EStatsType.COUNT]; 

        // Start is called before the first frame update
        void Start()
        {
            
        }
    
        // Update is called once per frame
        void Update()
        {
            
        }

        public void AddStat(EStatsType type, int amount)
        {
            int index = (int)type;
            if (index >= (int)EStatsType.COUNT)
            {
                return;
            }

            _stats[index] += amount;
        }

        public int GetState(EStatsType type)
        {
            int index = (int)type;
            if (index >= (int)EStatsType.COUNT)
            {
                return -1;
            }

            return _stats[index];
        }

        [System.Serializable]
        private struct SaveData
        {
            public int[] _stats;
        }

        public object SaveState()
        {
            SaveData _saveData =  new SaveData()
            {
                _stats = new int[(int)EStatsType.COUNT]
            };

            for (int i = 0; i < (int)EStatsType.COUNT; ++i)
            {
                _stats[i] = this._stats[i];
            }

            return _saveData;
        }

        public void LoadState(object state)
        {
            SaveData saveData = (SaveData)state;

            this._stats = saveData._stats;
        }
    }
    
}
