using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General.SaveLoad;
using UnityEngine;

namespace _Scripts._Game.General.Interactable.Reactable{

    [RequireComponent(typeof(SaveableEntity))]
    public class DoorEntity : MonoBehaviour, IReactable, ISaveable
    {
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
        }

        public void OnEndReact()
        {
        }

        //ISaveable
        [System.Serializable]
        private struct SaveData
        {
            public int[] _completionStats;
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
