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

        public void TickSequenceableGroup()
        {
            if (_isSimultaneousGroup)
            {

            }
            else
            {
                Sequenceable seq = GetCurrentSequence();

                if (seq.IsComplete())
                {
                    seq = GetNextSequence();
                }

                if (seq)
                {
                    seq.Tick();
                }
                else
                {
                    return;
                }
            }
        }

        public bool IsComplete()
        {
            if (_isSimultaneousGroup)
            {
                return false;
            }
            else
            {
                return _sequenceIndex >= _sequenceables.Count;
            }
        }

        private Sequenceable GetCurrentSequence()
        {
            return _sequenceables[_sequenceIndex];
        }

        private Sequenceable GetNextSequence()
        {
            _sequenceIndex++;
            if (IsComplete())
            {
                return null;
            }
            else
            {
                return _sequenceables[_sequenceIndex];
            }
        }
    }

}
