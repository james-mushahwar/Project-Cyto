using _Scripts._Game.General;
using UnityEngine;
using _Scripts._Game.AI.MovementStateMachine;

namespace _Scripts._Game.AI.MovementStateMachine.Environment.GeneralTurret{
    
    public class GeneralTurretAIMovementStateMachine : EnvironmentAIMovementStateMachine
    {
        #region Stats
    
        #endregion
        
        #region Bonded Stats
    
        #endregion
    
        protected override void Awake()
        {
            base.Awake();
        }
    
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