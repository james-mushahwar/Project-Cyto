﻿using _Scripts._Game.General;
using UnityEngine;
using _Scripts._Game.AI.MovementStateMachine;

namespace _Scripts._Game.AI.MovementStateMachine.Ground.DaggerMushroom{
    
    public class DaggerMushroomAIMovementStateMachine : GroundAIMovementStateMachine
    {
        #region #TBD# Stats
    
        #endregion
        
        #region Bonded #TBD# Stats
    
        #endregion
    
        // ISaveable
        [System.Serializable]
        private struct SaveData
        {
    
        }
    
        public override object SaveState()
        {
            return new SaveData();
        }
    
        public override void LoadState(object state)
        {
            SaveData saveData = (SaveData)state;
        }
    }
}