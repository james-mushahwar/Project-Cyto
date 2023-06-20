using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using _Scripts._Game.Dialogue;
using _Scripts._Game.General.Managers;
using _Scripts._Game.Player;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts._Game.General.Interactable{
    
    public class DialogueInteractable : MonoBehaviour, IInteractable
    {
        #region Dialogue

        [SerializeField] 
        private EDialogueType _dialogueType;
        [SerializeField] 
        private Phrase _phrase;
        private Task _task;

        #endregion

        //IInteractable
        [SerializeField]
        private Transform _interactRoot;
        [SerializeField]
        private bool _isInteractableLocked;
        [SerializeField]
        private RangeParams _rangeParams = new RangeParams(false);
        private UnityEvent _onInteractStart;
        private UnityEvent _onInteractEnd;

        public Transform InteractRoot 
        {
            get => _interactRoot;
            set => _interactRoot = value;
        }

        public bool IsInteractionLocked
        {
            get => _isInteractableLocked;
            set => _isInteractableLocked = value;
        }

        public RangeParams RangeParams { get => _rangeParams; }
        public UnityEvent OnInteractStart { get => _onInteractStart; }
        public UnityEvent OnInteractEnd { get => _onInteractEnd; }

        private void Awake()
        {
            if (_interactRoot == null)
            {
                _interactRoot = transform;
            }
        }

        private void Update()
        {
            if (IsInteractionLocked)
            {

            }
        }

        private void OnEnable()
        { 
            InteractableManager.Instance?.AddInteractable(this);
        }

        private void OnDisable()
        {
            InteractableManager.Instance?.RemoveInteractable(this);
        }

        public bool IsInteractable()
        {
            Vector2 playerPos = PlayerEntity.Instance.GetControlledGameObject().transform.position;
            Vector2 interactablePos = InteractRoot.position;
            Vector2 difference = (interactablePos - playerPos);

            bool isInDotProductRange = true;

            if (_rangeParams._useDotProduct)
            {
                float dotProduct = Vector2.Dot(_rangeParams._dotDirection, difference.normalized);
                isInDotProductRange = dotProduct >= Mathf.Min(_rangeParams._dotRange.x, _rangeParams._dotRange.y);
            }

            bool isInDistanceRange = isInDotProductRange && (difference.SqrMagnitude() <= _rangeParams._maxSqDistance);
            
            return !IsInteractionLocked && isInDistanceRange;
        }

        public void OnInteract()
        {
            if (_task == null)
            {
                IsInteractionLocked = true;
                InputManager.Instance.TryEnableActionMap(EInputSystem.Menu);
                _task = DialogueManager.Instance.PostText<Phrase>(_phrase, _dialogueType);
            }
        }
    }
    
}
