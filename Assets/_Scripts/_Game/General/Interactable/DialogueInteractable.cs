using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using _Scripts._Game.Dialogue;
using _Scripts._Game.General.Managers;
using _Scripts._Game.Player;
using _Scripts._Game.UI.World;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts._Game.General.Interactable{
    
    public class DialogueInteractable : MonoBehaviour, IInteractable
    {
        #region Dialogue
        [SerializeField] 
        private ScriptableDialogue _scriptableDialogue;
        [SerializeField] 
        private EDialogueType _dialogueTypeOverride = EDialogueType.INVALID;
        private Task _task;
        #endregion

        #region Animation
        private Animator _animator;
        private SpriteRenderer _renderer;

        private readonly int _idle = Animator.StringToHash("LoopStone_Idle");
        private readonly int _powerOn = Animator.StringToHash("LoopStone_On");
        private readonly int _powerOff = Animator.StringToHash("LoopStone_Off");
        #endregion

        //IInteractable
        [SerializeField]
        private Transform _interactRoot;
        private bool _isInteractableLocked;
        [SerializeField]
        private EInteractableStimuli _interactableStimuli;
        [SerializeField]
        private EInteractableType _interactableType;
        [SerializeField]
        private RangeParams _rangeParams = new RangeParams(false);
        private UnityEvent _onHighlight = new UnityEvent();
        private UnityEvent _onUnhighlight = new UnityEvent();
        private UnityEvent _onInteractStart = new UnityEvent();
        private UnityEvent _onInteractEnd = new UnityEvent();

        public EInteractableStimuli InteractableStimuli { get => _interactableStimuli; }
        public EInteractableType InteractableType { get => _interactableType; }
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
        public UnityEvent OnHighlight { get => _onHighlight; }
        public UnityEvent OnUnhighlight { get => _onUnhighlight; }
        public UnityEvent OnInteractStart { get => _onInteractStart; }
        public UnityEvent OnInteractEnd { get => _onInteractEnd; }

        private InputPrompt _inputPrompt;

        private void Awake()
        {
            if (_interactRoot == null)
            {
                _interactRoot = transform;
            }

            if (_inputPrompt == null)
            {
                _inputPrompt = GetComponentInChildren<InputPrompt>();
            }

            _animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            if (IsInteractionLocked)
            {
                if (_task != null)
                {
                    if (_task.Running == false && _task.Paused == false)
                    {
                        _task = null;
                        IsInteractionLocked = false;
                    }
                }
                else
                {
                    IsInteractionLocked = false;
                }
                
            }
        }

        private void OnEnable()
        { 
            _onHighlight.AddListener(PowerOn);
            _onUnhighlight.AddListener(PowerOff);
            InteractableManager.Instance?.AddInteractable(this);
        }

        private void OnDisable()
        {
            _onHighlight.RemoveAllListeners();
            _onUnhighlight.RemoveAllListeners();
            InteractableManager.Instance?.RemoveInteractable(this);
        }

        private void PowerOn()
        {
            Animate(_powerOn);
        }

        private void PowerOff()
        {
            Animate(_powerOff);
            _inputPrompt?.EndPrompt();
        }

        public void FinishedOn()
        {
            _animator.enabled = false;
            _inputPrompt?.StartPrompt();
        }

        public void FinishedOff()
        {
            Animate(_idle);
        }

        private void Animate(int hash)
        {
            _animator.enabled = true;
            _animator.CrossFade(hash, 0, 0);
        }

        public bool IsInteractable()
        {
            if (IsInteractionLocked)
            {
                return false;
            }

            Vector2 playerPos = PlayerEntity.Instance.GetControlledGameObject().transform.position;
            Vector2 interactablePos = InteractRoot.position;
            Vector2 difference = (interactablePos - playerPos);

            bool isInDotProductRange = true;

            if (_rangeParams._useDotProduct)
            {
                float dotProduct = Vector2.Dot(_rangeParams._dotDirection, difference.normalized);
                isInDotProductRange = dotProduct >= Mathf.Min(_rangeParams._dotRange.x, _rangeParams._dotRange.y);
            }

            //Debug.Log("SqDistance from interactable is" + difference.SqrMagnitude());
            bool isInDistanceRange = isInDotProductRange && (difference.SqrMagnitude() <= _rangeParams._maxSqDistance);
            
            return isInDistanceRange;
        }

        public void OnInteract()
        {
            if (_task == null)
            {
                InputManager.Instance.TryEnableActionMap(EInputSystem.Menu);
                _task = DialogueManager.Instance.PostText<Phrase>(_scriptableDialogue.GetPhrase(0), _dialogueTypeOverride != EDialogueType.INVALID ? _dialogueTypeOverride : _scriptableDialogue.DialogueType);
                IsInteractionLocked = true;
            }
        }
    }
    
}
