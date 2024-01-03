using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General.SaveLoad;
using UnityEngine;

namespace _Scripts._Game.General.Managers{
    
    public class InteractableManager : Singleton<InteractableManager>, IManager
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

        public void ManagedTick()
        {
            if (!GameStateManager.Instance.IsGameRunning)
            {
                return;
            }

            if (_closestInteractable != null)
            {
                if (_closestInteractable.IsInteractionLocked)
                {
                    return;
                }
            }

            IInteractable newInteractable = null;
            foreach (IInteractable interactable in _activeInteractables)
            {
                if (interactable.IsInteractable())
                {
                    newInteractable = interactable;
                    break;
                }
            }

            if (newInteractable == null)
            {
                if (_closestInteractable != null)
                {
                    _closestInteractable.OnUnhighlight.Invoke();
                }
            }
            else
            {
                if (_closestInteractable == null)
                {
                    newInteractable.OnHighlight.Invoke();
                }
            }

            _closestInteractable = newInteractable;
        }

        public void InteractInput()
        {
            if (PauseManager.Instance.IsPaused || _closestInteractable == null)
            {
                return;
            }

            if (_closestInteractable.InteractableStimuli != EInteractableStimuli.PlayerInput)
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

        public bool IsInteractableLocked(IInteractable interactable)
        {
            if (interactable == null)
            {
                return false;
            }

            if (interactable.InteractableType == EInteractableType.SaveStation)
            {
                return SaveLoadSystem.IsSaveOrLoadInProgress;
            }

            if (interactable.InteractableType == EInteractableType.Doorway)
            {
                return GameStateManager.Instance.IsLoadInProgress;
            }
            return true;
        }

        public void ResolveInteraction(IInteractable interactable)
        {
            if (interactable == null)
            {
                return;
            }

            // do nothing
            if (interactable.InteractableType == EInteractableType.SaveStation)
            {
                return;
            }

        }

        public void PreInGameLoad()
        {
             
        }

        public void PostInGameLoad()
        {
             
        }

        public void PreMainMenuLoad()
        {
             
        }

        public void PostMainMenuLoad()
        {
             
        }
    }
    
}
