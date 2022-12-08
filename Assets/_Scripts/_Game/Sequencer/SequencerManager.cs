using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts._Game.General;
using _Scripts._Game.Sequencer;
using System.Linq;

 namespace _Scripts._Game.Sequencer{
  
    public class SequencerManager : Singleton<SequencerManager>
    {
        private bool _isSequenceActive = false;
        private SequenceContainer _sequenceContainer;
        private SequenceContainer[] _queuedSequenceContainers;
        [SerializeField] private int _maxQueuedContainers;
        private int _nextFreeSlot;
        private int _queueHeadSlot;
        private int _freeQueueSlots;

         // Start is called before the first frame update
         void Start()
         {
            _queuedSequenceContainers = new SequenceContainer[_maxQueuedContainers];
            _nextFreeSlot = _maxQueuedContainers - 1;
            _queueHeadSlot = -1;
            _freeQueueSlots = _maxQueuedContainers;

            foreach (var container in FindObjectsOfType<SequenceContainer>())
            {
                Debug.Log("Found seqcontainer");
                QueueSequenceContainer(container);
                if (_sequenceContainer == null)
                {
                    _sequenceContainer = _queuedSequenceContainers[4];
                }
            }
         }
  
         void FixedUpdate()
         {
            if (_sequenceContainer == null)
            {
                if (_freeQueueSlots < _maxQueuedContainers)
                {
                    // take next queued sequence
                    _sequenceContainer = GetNextQueuedSequence();
                }
            }

            if (_sequenceContainer)
            {
                Debug.Log("Tick SeqManager");
                float deltaTime = Time.deltaTime;
                TickSequenceableContainer(deltaTime);
            }
         }

         public void TryNewSequenceContainer(SequenceContainer seqContainer)
         {
            if (_isSequenceActive)
            {
                if (_freeQueueSlots > 0 && !SequenceContainerIsQueued(seqContainer))
                {
                    if (seqContainer.CanBeQueued)
                    {
                        QueueSequenceContainer(seqContainer);
                    }
                }

                return;
            }

            _sequenceContainer = seqContainer;
         }

         private bool SequenceContainerIsQueued(SequenceContainer seqContainer)
         {
            for (int i = _maxQueuedContainers - 1; i >= 0; --i)
            {
                if (_queuedSequenceContainers[i] == seqContainer)
                {
                    return true;
                }
            }

            return false;
         }

        private void QueueSequenceContainer(SequenceContainer seqContainer)
        {
            _queuedSequenceContainers[_nextFreeSlot] = seqContainer;
            if (_queueHeadSlot == -1)
            {
                _queueHeadSlot = _nextFreeSlot;
            }
            _freeQueueSlots--;
            if (_freeQueueSlots > 0)
            {
                _nextFreeSlot--;
                if (_nextFreeSlot == -1)
                {
                    _nextFreeSlot = _maxQueuedContainers - 1;
                }
            }
            else
            {
                _nextFreeSlot = -1;
            }
        }
     
        private SequenceContainer GetNextQueuedSequence()
        {
            if (_freeQueueSlots >= _maxQueuedContainers - 1)
            {
                return null;
            }

            SequenceContainer seqCont = _queuedSequenceContainers[_queueHeadSlot];
            _queuedSequenceContainers[_queueHeadSlot] = null;

            _queueHeadSlot--;
            if (_queueHeadSlot < 0)
            {
                _queueHeadSlot = _maxQueuedContainers - 1;
            } 

            return seqCont;
        }

        private void TickSequenceableContainer(float deltaTime)
        {
            _sequenceContainer.TickSequenceContainer();
        }
    }
  
}