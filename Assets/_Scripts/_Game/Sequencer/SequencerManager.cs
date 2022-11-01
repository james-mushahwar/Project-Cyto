using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts._Game.General;
using _Scripts._Game.Sequencer;
using System.Linq;

public struct SequenceHandlers
{
    public SequenceHandlers(int length) 
    {

    }

    public Sequence[] _sequenceContainers;

    private Sequence[] _queuedSequences;
    private int _nextFreeIndex;
    private int _nextQueuedIndex;
}

public struct Sequence
{
    public SequenceContainer[] _sequenceContainers;
}

namespace _Scripts._Game.Sequencer{
    
    public class SequencerManager : Singleton<SequencerManager>
    {
        private Sequence _sequence = new Sequence();

        // Start is called before the first frame update
        void Start()
        {
            
        }
    
        // Update is called once per frame
        void Update()
        {
            
        }

        public void TryQueueSequence(Sequence seq)
        {
            // is empty
            if (_sequence._sequenceContainers.Length == 0)
            {
                _sequence._sequenceContainers = seq._sequenceContainers;
            }
            else
            {
                _queuedSequences[_nextFreeIndex]._sequenceContainers = seq._sequenceContainers;

                _nextFreeIndex++;
                if (_nextFreeIndex > 4)
                {
                    _nextFreeIndex = 0;
                }
            }
        }

        protected void PushSequence()
        {

        }

        protected void PopSequence()
        {

        }
    }
    
}
