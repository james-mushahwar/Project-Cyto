using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts._Game.General;
using _Scripts._Game.Sequencer;

public struct Sequence
{
    SequenceContainer[] sequenceContainers;
}

namespace _Scripts._Game.Sequencer{
    
    public class SequencerManager : Singleton<SequencerManager>
    {
        private Sequence _sequence;

        // Start is called before the first frame update
        void Start()
        {
            
        }
    
        // Update is called once per frame
        void Update()
        {
            
        }
    }
    
}
