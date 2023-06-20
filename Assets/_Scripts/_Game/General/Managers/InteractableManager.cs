using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Managers{
    
    public class InteractableManager : Singleton<InteractableManager>
    {
        private List<IInteractable> _activeInteractables = new List<IInteractable>();
        private IInteractable _closestInteractable;

        public void AddInteractable(IInteractable interactable)
        {
            _activeInteractables.Add(interactable);
        }

        public void RemoveInteractable(IInteractable interactable)
        {
            _activeInteractables.Remove(interactable);
            if (interactable == _closestInteractable)
            {
                _closestInteractable = null;
            }
        }

        private void FixedUpdate()
        {
            foreach (IInteractable interactable in _activeInteractables)
            {
                if (interactable.IsInteractable())
                {
                    _closestInteractable = interactable;
                    break;
                }
            }
        }

        public void InteractInput()
        {
            if (PauseManager.Instance.IsPaused || _closestInteractable == null)
            {
                return;
            }

            if (_closestInteractable.IsInteractionLocked)
            {
                return;
            }

            InputManager.Instance.NullifyInput(EPlayerInput.SButton);
            _closestInteractable.OnInteract();
        }
    }
    
}
