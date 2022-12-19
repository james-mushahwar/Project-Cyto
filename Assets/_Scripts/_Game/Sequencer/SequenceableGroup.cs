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

                if (seq)
                {
                    if (seq.IsComplete())
                    {
                        seq.Stop(); // clean up
                        seq = GetNextSequence();
                    }
                }

                if (seq)
                {
                    seq.Tick();
                }
                else
                {
                    ResetSequenceGroup();
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
                bool complete = _sequenceIndex >= _sequenceables.Count;
                //if (complete)
                //{
                //    _sequenceIndex = 0;
                //}
                return complete;
            }
        }

        private Sequenceable GetCurrentSequence()
        {
            if (_sequenceIndex >= _sequenceables.Count)
            {
                return null;
            }
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

        public void ResetSequenceGroup()
        {
            
        }
    }

}
