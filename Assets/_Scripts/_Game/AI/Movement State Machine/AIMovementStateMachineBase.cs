using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts._Game.General;
using _Scripts._Game.General.SaveLoad;
using _Scripts._Game.General.Managers;

namespace _Scripts._Game.AI.MovementStateMachine{
    
    public class AIMovementStateMachineBase : Singleton<AIMovementStateMachineBase>, ISaveable
    {
        #region State Machine
        private BaseAIMovementState _currentState;
        protected AIMovementStateMachineFactory _states;
        public BaseAIMovementState CurrentState { get => _currentState; set => _currentState = value; }
        #endregion

        #region Player attributes
        private Rigidbody2D _rb;
        private CapsuleCollider2D _capsule;
        [SerializeField]
        private LayerMask _groundedLayer;

        public Rigidbody2D Rb { get => _rb; }
        public LayerMask GroundedLayer { get => _groundedLayer; }
        #endregion

        protected override void Awake()
        {
            base.Awake();

            _states = new AIMovementStateMachineFactory(this);
            //_currentState = _states.GetState(MovementState.Grounded);
        }

        // Start is called before the first frame update
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _capsule = GetComponent<CapsuleCollider2D>();
            _currentState.EnterState();
        }
    
        // Update is called once per frame
        void Update()
        {
            
        }

        protected virtual void SetUpStateMachineFactory()
        {

        }

        // ISaveable
        [System.Serializable]
        protected struct SaveData
        {

        }

        public object SaveState()
        {
            return new SaveData();
        }

        public void LoadState(object state)
        {
            SaveData saveData = (SaveData)state;
        }
    }
}
