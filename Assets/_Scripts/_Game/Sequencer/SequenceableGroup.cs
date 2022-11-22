using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.Sequencer{
    
    public class SequenceableGroup : MonoBehaviour
    {
        [SerializeField]
        private bool _isSimultaneousGroup;
        [SerializeField]
        public List<Sequenceable> _sequenceables = new List<Sequenceable>();

        private int _sequenceIndex;

        public Sequenceable GetCurrentSequence()
        {
            return _sequenceables[_sequenceIndex];
        }

        public bool IsComplete()
        {
            return true;
        }
    }

}
