using _Scripts._Game.General.Managers;
using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General.Identification;
using Assets._Scripts._Game.General.SceneLoading;
using UnityEngine;
using UnityEngine.Events;
using _Scripts._Game.Player;
using _Scripts._Game.General.SaveLoad;
using _Scripts._Game.Events;

namespace _Scripts._Game.General.SceneLoading{
    [RequireComponent(typeof(RuntimeID))]
    public class Doorway : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private SceneField _zoneScene;
        [SerializeField]
        private SceneField _areaScene;
        private int _areaBuildIndex;

        [SerializeField] 
        private string _doorwayID;

        //IInteractable
        [SerializeField]
        private EInteractableStimuli _interactableStimuli;
        [SerializeField]
        private EInteractableType _interactableType;
        [SerializeField]
        private Transform _interactRoot;
        private bool _isInteractableLocked;
        [SerializeField]
        private RangeParams _rangeParams = new RangeParams(false);
        private UnityEvent _onHighlight = new UnityEvent();
        private UnityEvent _onUnhighlight = new UnityEvent();
        [SerializeField]
        private UnityEvent _onInteractStart = new UnityEvent();
        [SerializeField]
        private UnityEvent _onInteractEnd = new UnityEvent();
        [SerializeField]
        private GameEvent _onInteractStartGE;
        [SerializeField]
        private GameEvent _onInteractEndGE;

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

        // Start is called before the first frame update
        void Start()
        {
            _areaBuildIndex = AssetManager.Instance.SceneNameToBuildIndex(_areaScene);
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
            if (InteractableManager.Instance.IsInteractableLocked(this))
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
            if (GameStateManager.Instance.IsLoadInProgress)
            {
                return;
            }

            // set doorway id
            RespawnManager.Instance.DoorwayGOID = _doorwayID;
            // load new zone and area
            GameStateManager.Instance.TryNewZoneAndArea(_zoneScene, _areaScene);
        }
    }
    
}
