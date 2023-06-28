using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General.LogicController;
using _Scripts._Game.General.Managers;
using _Scripts._Game.General.SaveLoad;
using UnityEngine;

namespace _Scripts._Game.General.Interactable.Reactable{

    [RequireComponent(typeof(SaveableEntity)), RequireComponent(typeof(LogicEntity))]
    public class DoorEntity : MonoBehaviour, ISaveable
    {
        #region General
        private bool _isClosed;

        private ILogicEntity _logicEntity;
        #endregion

        #region Interactable
        private GameObject _interactableGO;
        private IInteractable _interactable;
        #endregion

        #region Door
        private Animator _animator;
        private SpriteRenderer _renderer;
        private BoxCollider2D _blockingCollider;
        #endregion

        #region Animation
        private readonly int _openingHash = Animator.StringToHash("Door_Opening");
        private readonly int _closingHash = Animator.StringToHash("Door_Closing");
        #endregion

        private void Awake()
        {
            if (_interactable == null)
            {
                if (_interactableGO != null)
                {
                    _interactable = _interactableGO.GetComponent<IInteractable>();


                }

                if (_interactable == null)
                {
                    _interactable = GetComponent<IInteractable>();
                }
            }

            _logicEntity = GetComponent<LogicEntity>();
            _logicEntity.IsInputLogicValid = LogicManager.Instance.AreAllInputsValid(_logicEntity);

            _animator = GetComponentInChildren<Animator>();

            _renderer = GetComponent<SpriteRenderer>();

            _blockingCollider = GetComponent<BoxCollider2D>();
            _blockingCollider.enabled = _logicEntity.IsInputLogicValid;
        }

        private void OnEnable()
        {
            if (_animator != null)
            {
                _animator.enabled = false;
            }

            if (_logicEntity == null)
            {
                _logicEntity = GetComponent<LogicEntity>();
            }
            _logicEntity.OnInputChanged.AddListener(OnInputChanged);

            _blockingCollider.enabled = !_logicEntity.IsInputLogicValid;
        }

        private void OnDisable()
        {
            if (_logicEntity == null)
            {
                _logicEntity = GetComponent<LogicEntity>();
            }
            _logicEntity.OnInputChanged.RemoveListener(OnInputChanged);
        }

        private void OnInputChanged()
        {
            if (_logicEntity.IsInputLogicValid)
            {
                OnPower();
            }
            else
            {
                OnLosePower();
            }
        }

        public void OnPower()
        {
            _logicEntity.IsOutputLogicValid = true;
            if (_animator != null)
            {
                _animator.enabled = true;
                int hash = _openingHash;
                _animator.CrossFade(hash, 0, 0);
            }

            _blockingCollider.enabled = !_logicEntity.IsInputLogicValid;
            LogicManager.Instance.OnOutputChanged(_logicEntity);
        }

        public void OnLosePower()
        {
            _logicEntity.IsOutputLogicValid = false;
            if (_animator != null)
            {
                _animator.enabled = true;
                int hash = _closingHash;
                _animator.CrossFade(hash, 0, 0);
            }

            _blockingCollider.enabled = !_logicEntity.IsInputLogicValid;
            LogicManager.Instance.OnOutputChanged(_logicEntity);
        }

        //ISaveable
        [System.Serializable]
        private struct SaveData
        {
            public bool _isClosed;
        }

        public object SaveState()
        {
            return new SaveData()
            {
                 
            };
        }

        public void LoadState(object state)
        {
            
        }
    }
    
}
