using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General.SaveLoad;
using UnityEngine;

namespace _Scripts._Game.General.Interactable.Reactable{

    [RequireComponent(typeof(SaveableEntity))]
    public class DoorEntity : MonoBehaviour, IReactable, ISaveable
    {
        #region General
        private bool _isClosed;
        #endregion

        #region IReactable
        public bool CanReact { get; }
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

            _animator = GetComponent<Animator>();
            _renderer = GetComponent<SpriteRenderer>();
            _blockingCollider = GetComponent<BoxCollider2D>();
        }

        private void OnEnable()
        {
            if (_animator != null)
            {
                _animator.enabled = false;

                
            }

            if (_interactable != null)
            {
                _interactable.OnInteractStart.AddListener(OnStartReact);
            }
        }

        private void OnDisable()
        {
            if (_interactable != null)
            {
                _interactable.OnInteractStart.RemoveListener(OnStartReact);
            }
        }

        public void OnStartReact()
        {
            if (_animator != null)
            {
                int hash = _isClosed ? _closingHash : _openingHash;
                _animator.CrossFade(hash, 0, 0);
            }
        }

        public void OnEndReact()
        {
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
