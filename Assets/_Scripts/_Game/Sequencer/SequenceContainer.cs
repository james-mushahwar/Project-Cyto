using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.Sequencer{
    
    public class SequenceContainer : MonoBehaviour
    {
        [SerializeField]
        private bool _canBeQueued;
        [SerializeField]
        private bool _isSimultaneous;

        [SerializeField]
        private List<SequenceableGroup> _sequenceableGroups = new List<SequenceableGroup>();

        private int _groupIndex;

        public bool CanBeQueued { get => _canBeQueued; }
        // Start is called before the first frame update
        void Start()
        {
            
        }
    
        // Update is called once per frame
        void Update()
        {
            
        }

        public void TickSequenceContainer()
        {
            SequenceableGroup group = GetCurrentGroup();

            if (group == null || group.IsComplete()) 
            {
                group = GetNextGroup();
            }

            if (group != null)
            {
                group.TickSequenceableGroup();
            }
            else
            {
                return;
            }

        }

        public bool IsContainerComplete()
        {
            return _groupIndex >= _sequenceableGroups.Count;
        }

        private SequenceableGroup GetCurrentGroup()
        {
            if (_groupIndex < _sequenceableGroups.Count)
            {
                return _sequenceableGroups[_groupIndex];
            }

            return null;
        }

        private SequenceableGroup GetNextGroup()
        {
            _groupIndex++;
            if (IsContainerComplete())
            {
                return null;
            }
            else
            {
                return _sequenceableGroups[_groupIndex];
            }
        }
    }
    
}
