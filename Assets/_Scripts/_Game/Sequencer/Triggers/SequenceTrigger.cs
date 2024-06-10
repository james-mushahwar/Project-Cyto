﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.Sequencer.Triggers{
    
    public abstract class SequenceTrigger : MonoBehaviour
    {
        [SerializeField]
        protected List<SequenceableSettings> _sequenceableSettings;

        public virtual bool RegisterSequences()
        {
            bool registered = true;

            foreach (SequenceableSettings seqSettings in _sequenceableSettings)
            {
                bool register = SequencerManager.Instance.TryRegisterSequence(seqSettings._sequenceable, seqSettings._sequenceSettings);

                if (!register && seqSettings._sequenceSettings._canAlwaysRun)
                {
                    register = true;
                }

                if (!register && registered)
                {
                    registered = false;
                }

            }

            return registered;
        }

        public virtual bool UnregisterSequences()
        {
            bool unregistered = true;

            foreach (SequenceableSettings seqSettings in _sequenceableSettings)
            {
                bool unregister = SequencerManager.Instance.TryUnregisterSequence(seqSettings._sequenceable, seqSettings._sequenceSettings);

                if (!unregister && unregistered)
                {
                    unregistered = false;
                }

            }

            return unregistered;
        }
    }
    
}
