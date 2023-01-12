using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using _Scripts._Game.General;
using _Scripts._Game.General.SaveLoad;

namespace _Scripts._Game.Player.AttackingStateMachine{
    
    public class PlayerAttackingStateMachine : Singleton<PlayerAttackingStateMachine>, ISaveable
    {
        #region State Machine
        private BaseAttackingState _currentState;
        private PlayerAttackingStateMachineFactory _states;

        public BaseAttackingState CurrentState { get => _currentState; set => _currentState = value; }
        #endregion

        #region External References
        private PlayerEntity _playerEntity;

        public PlayerEntity PlayerEntity { get => _playerEntity; }
        #endregion

        protected override void Awake()
        {
            base.Awake();

            _playerEntity = GetComponent<PlayerEntity>();
            if (_playerEntity)
            {
                _playerEntity.AttackingSM = this;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }
    
        // Update is called once per frame
        void Update()
        {
            
        }

        public void LoadState(object state)
        {
            throw new System.NotImplementedException();
        }

        public object SaveState()
        {
            throw new System.NotImplementedException();
        }
    }
    
}
