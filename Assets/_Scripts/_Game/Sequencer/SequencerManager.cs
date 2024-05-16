using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts._Game.General;
using _Scripts._Game.Sequencer;
using System;
using _Scripts._Game.Sequencer.Triggers;

namespace _Scripts._Game.Sequencer{
    
    [Serializable]  
    public class SequenceSettings
    {
        //Player
        public bool _freezePlayer = false;

        //General
        public bool _canAlwaysRun = false;
    }

    public class SequencerManager : Singleton<SequencerManager>, IManager
    {
        private List<Sequenceable> _activeSequences = new List<Sequenceable>();
        private int _freezePlayerStack;
        private Dictionary<string, SequenceSettings> _sequenceSettings = new Dictionary<string, SequenceSettings>();

        // Start is called before the first frame update
        public void ManagedPreInGameLoad()
        {
            _freezePlayerStack = 0;
            _activeSequences.Clear();
            _sequenceSettings.Clear();
        }

        public void ManagedPostInGameLoad()
        {
            
        }

        public void ManagedPreMainMenuLoad()
        {
            if (_freezePlayerStack != 0)
            {
                //unfreeze player 

                _freezePlayerStack = 0;
            }
            _activeSequences.Clear();
            _sequenceSettings.Clear();
        }

        public void ManagedPostMainMenuLoad()
        {
            
        }

        public void ManagedTick()
        {
            foreach (var sequence in _activeSequences)
            {
                if (sequence.IsStarted() == false)
                {
                    sequence.Begin();
                }

                sequence.Tick();
            }

            for (int i = _activeSequences.Count - 1; i >= 0; i--)
            {
                Sequenceable sequence = _activeSequences[i];

                if (sequence.IsComplete())
                {
                    sequence.Stop();

                    // resolve settings
                    bool found = _sequenceSettings.TryGetValue(sequence.RuntimeID, out SequenceSettings settings);
                    if (found)
                    {
                        if (settings._freezePlayer)
                        {
                            _freezePlayerStack--;
                            if (_freezePlayerStack == 0)
                            {
                                // unfreeze player 
                            }
                        }
                    }
                    _sequenceSettings.Remove(sequence.RuntimeID);
                    _activeSequences.RemoveAt(i);
                }
            }
        }

        public bool TryRegisterSequence(Sequenceable seq, SequenceSettings seqSettings)
        {
            bool register = false;
            string runtimeID = seq.RuntimeID;

            if (!_sequenceSettings.ContainsKey(runtimeID))
            {
                _activeSequences.Add(seq);

                if (seqSettings._freezePlayer)
                {
                    _freezePlayerStack++;
                    if (_freezePlayerStack == 1)
                    {
                        //freeze player
                    }
                }
                _sequenceSettings.Add(runtimeID, seqSettings);

                register = true;
            }

            return register;
        }
    }
}