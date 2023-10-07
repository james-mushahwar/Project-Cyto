using _Scripts._Game.General.Managers;
using System.Collections;
using System.Collections.Generic;
using Assets._Scripts._Game.General.SceneLoading;
using UnityEngine;
using UnityEngine.Events;
using _Scripts._Game.Player;

namespace _Scripts._Game.General.SceneLoading{
    
    public class Doorway : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private SceneField _zoneScene;
        [SerializeField]
        private SceneField _areaScene;
        private int _areaBuildIndex;

        //IInteractable
        public EInteractableStimuli InteractableStimuli { get; }
        public EInteractableType InteractableType { get; }
        public Transform InteractRoot { get; set; }
        public bool IsInteractionLocked { get; set; }

        private RangeParams _rangeParams = new RangeParams(false);
        public RangeParams RangeParams { get; }

        public UnityEvent OnHighlight { get; }
        public UnityEvent OnUnhighlight { get; }
        public UnityEvent OnInteractStart { get; }
        public UnityEvent OnInteractEnd { get; }



        // Start is called before the first frame update
        void Start()
        {
            _areaBuildIndex = AssetManager.Instance.SceneNameToBuildIndex(_areaScene);
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


        }
    }
    
}
