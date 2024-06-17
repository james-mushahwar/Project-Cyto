﻿using _Scripts._Game.Dialogue;
using _Scripts._Game.General.LogicController;
using _Scripts._Game.General.Managers;
using _Scripts._Game.General.SaveLoad;
using _Scripts._Game.Player;
using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.Events;
using _Scripts._Game.General.Identification;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts._Game.General.Interactable{

    [RequireComponent(typeof(LogicEntity))]
    public class InteractableEntity : MonoBehaviour, IInteractable
    {
        #region Interactable
        private SaveableEntity _saveStationSE;
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
        #endregion

        private void Awake()
        {
            if (_interactRoot == null)
            {
                _interactRoot = transform;
            }

            if (_interactableType == EInteractableType.SaveStation)
            {
                _saveStationSE = gameObject.GetComponent<SaveableEntity>();
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
            if (IsInteractionLocked)
            {
                OnInteractEnd.Invoke();
                _onInteractEndGE.TriggerEvent();
            }
            else
            {
                if (_interactableType == EInteractableType.SaveStation)
                {
                    GameStateManager.Instance.SetAreaSpawnScene(gameObject.scene.buildIndex);
                    if (_saveStationSE != null)
                    {
                        RespawnManager.Instance.RespawnGOID = _saveStationSE.Id;
                    }

                    AudioManager.Instance.TryPlayAudioSourceAtLocation(EAudioType.SFX_Environment_SaveStation_Save, gameObject.transform.position);
                }
                OnInteractStart.Invoke();
                _onInteractStartGE.TriggerEvent();
            }

            InteractableManager.Instance.ResolveInteraction(this);
        }
    }
    
}
