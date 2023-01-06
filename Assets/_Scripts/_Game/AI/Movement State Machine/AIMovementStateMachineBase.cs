using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts._Game.General;
using _Scripts._Game.General.SaveLoad;
using _Scripts._Game.General.Managers;

namespace _Scripts._Game.AI.MovementStateMachine{
    
    public class AIMovementStateMachineBase : MonoBehaviour, ISaveable
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

        protected virtual void Awake()
        {
            _states = new AIMovementStateMachineFactory(this);
            //_currentState = _states.GetState(MovementState.Grounded);
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _capsule = GetComponent<CapsuleCollider2D>();
        }
   

        // ISaveable
        [System.Serializable]
        private struct SaveData
        {

        }

        public virtual object SaveState()
        {
            return new SaveData();
        }

        public virtual void LoadState(object state)
        {
            SaveData saveData = (SaveData)state;
        }
    }
}
