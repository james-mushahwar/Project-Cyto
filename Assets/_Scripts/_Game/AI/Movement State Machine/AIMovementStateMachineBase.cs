using System;
using System.Collections.Generic;
using _Scripts._Game.General.SaveLoad;
using _Scripts._Game.AI.Bonding;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts._Game.AI.MovementStateMachine{
    
    public class AIMovementStateMachineBase : MonoBehaviour, ISaveable, IBondable
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

        #region Bonding
        private Dictionary<BondInput, Action<InputAction.CallbackContext>> _bondInputsDict = new Dictionary<BondInput, Action<InputAction.CallbackContext>> ();
        public Dictionary<BondInput, Action<InputAction.CallbackContext>> BondInputsDict { get => _bondInputsDict; }
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

        // IBondable
        public virtual void OnMovementInput(InputAction.CallbackContext context)
        {
            
        }
        public virtual void OnDirectionInput(InputAction.CallbackContext context)
        {
            
        }
        public virtual void OnNorthButtonInput(InputAction.CallbackContext context)
        {
            
        }
        public virtual void OnSouthButtonInput(InputAction.CallbackContext context)
        {
            
        }
        public virtual void OnEastButtonInput(InputAction.CallbackContext context)
        {
            
        }
        public virtual void OnWestButtonInput(InputAction.CallbackContext context)
        {
            
        }
        public virtual void OnLeftBumperInput(InputAction.CallbackContext context)
        {
            
        }
        public virtual void OnRightBumperInput(InputAction.CallbackContext context)
        {
            
        }
        public virtual void OnLeftTriggerInput(InputAction.CallbackContext context)
        {
            
        }
        public virtual void OnRightTriggerInput(InputAction.CallbackContext context)
        {
            
        }

        public virtual void OnBonded()
        {
            
        }
        public virtual void OnUnbonded()
        {
            
        }
    }
}
