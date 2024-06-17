using _Scripts._Game.General.LogicController;
using _Scripts._Game.General.Managers;
using _Scripts._Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts._Game.General.Interactable.Ability{

    public class AbilityUnlockEntity : MonoBehaviour, IInteractable
    {
        #region Interactable
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

        public EInteractableStimuli InteractableStimuli => _interactableStimuli;

        public EInteractableType InteractableType => _interactableType;

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
        #endregion

        #region Logic
        [SerializeField]
        private bool _isInteractable;
        [SerializeField]
        private bool _logicBlocksIsInteractable = false;
        private ILogicEntity _logicEntity;

        [SerializeField]
        private bool _disableAbilityOnUnlock = false;
        #endregion

        #region Ability
        [SerializeField]
        private EAbility _abilityToUnlock = EAbility.COUNT;
        #endregion

        public void Start()
        {
            if (_logicEntity == null)
            {
                _logicEntity = GetComponent<LogicEntity>();
            }
            _logicEntity.OnInputChanged.AddListener(OnInputChanged);
        }

        private void OnInputChanged()
        {
            Debug.LogWarning("ability logic input has changed");
        }

        public void OnEnable()
        {
            InteractableManager.Instance?.AddInteractable(this);
        }

        public void OnDisable()
        {
            InteractableManager.Instance?.RemoveInteractable(this);
        }

        public bool IsInteractable()
        {
            bool isLogicValid = !_logicBlocksIsInteractable || (_logicBlocksIsInteractable && _logicEntity.IsInputLogicValid);

            bool isAbilityUnlocked = false; 

            if (_abilityToUnlock != EAbility.COUNT)
            {
                isAbilityUnlocked = StatsManager.Instance.IsAbilityUnlocked(_abilityToUnlock);
            }

            if ((isLogicValid && _isInteractable && !isAbilityUnlocked) == false)
            {
                return false;
            }

            // range check
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

            if (!isInDistanceRange)
            {
                return false;
            }

            return true;
        }

        public void OnInteract()
        {
            _isInteractable = false;

            //start sequence to unlock ability?
            StatsManager.Instance.UnlockAbility(_abilityToUnlock);
            StatsManager.Instance.DisableAbility(_abilityToUnlock, _disableAbilityOnUnlock);

            _logicEntity.IsOutputLogicValid = true;
            LogicManager.Instance.OnOutputChanged(_logicEntity);
        }
    }

}
