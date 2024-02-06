using _Scripts._Game.General.Identification;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.Sequencer.Gameplay{
    
    public class GameObjectSequence : Sequenceable
    {
        [SerializeField]
        private List<GameObject> _activateGOs;
        [SerializeField]
        private List<GameObject> _deactivateGOs;
        [SerializeField]
        private List<GameObject> _toggleGOs;

        [SerializeField]
        private List<MonoBehaviour> _activateMonobehaviours;
        [SerializeField]
        private List<MonoBehaviour> _deactivateMonobehaviours;
        [SerializeField]
        private List<MonoBehaviour> _toggleMonobehaviours;

        private RuntimeID _runtimeID;
        public override string RuntimeID => _runtimeID.Id;

        private bool _isStarted;
        private bool _isComplete;

        private void Awake()
        {
            _runtimeID = GetComponent<RuntimeID>();
        }

        public override void Begin()
        {
            _isStarted = true;
        }

        public override bool IsStarted()
        {
            return _isStarted;
        }

        public override bool IsComplete()
        {
            return _isComplete;
        }

        public override void Stop()
        {
        }

        public override void Tick()
        {
            //gameobjects
            foreach (GameObject go in _activateGOs)
            {
                go.SetActive(true);
            }

            foreach (GameObject go in _deactivateGOs)
            {
                go.SetActive(false);
            }

            foreach (GameObject go in _toggleGOs)
            {
                go.SetActive(!go.activeSelf);
            }

            //monobehaviours
            foreach(MonoBehaviour mono in _activateMonobehaviours)
            {
                mono.enabled = true;
            }

            foreach (MonoBehaviour mono in _deactivateMonobehaviours)
            {
                mono.enabled = false;
            }

            foreach (MonoBehaviour mono in _toggleMonobehaviours)
            {
                mono.enabled = !mono.enabled;
            }

            _isComplete = true;
        }
    }
    
}
