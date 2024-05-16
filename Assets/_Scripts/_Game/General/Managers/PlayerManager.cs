using _Scripts._Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Managers{
    
    public class PlayerManager : Singleton<PlayerManager>, IManager, ISaveable
    {
        [SerializeField]
        private PlayerEntity _playerEntityPrefab;
        private PlayerEntity _runtimePlayerEntity;

        public void LoadState(object state)
        {
            throw new System.NotImplementedException();
        }
        public object SaveState()
        {
            throw new System.NotImplementedException();
        }

        public void ManagedPostInGameLoad()
        {
            throw new System.NotImplementedException();
        }

        public void ManagedPostMainMenuLoad()
        {
            throw new System.NotImplementedException();
        }

        public void ManagedPreInGameLoad()
        {
            throw new System.NotImplementedException();
        }

        public void ManagedPreMainMenuLoad()
        {
            throw new System.NotImplementedException();
        }

        public void ManagedPostInitialiseGameState()
        {

        }
    }
    
}
