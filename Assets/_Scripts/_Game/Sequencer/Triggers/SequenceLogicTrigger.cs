using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General.LogicController;
using UnityEngine;

namespace _Scripts._Game.Sequencer.Triggers{
    
    public class SequenceLogicTrigger : SequenceTrigger
    {
        private LogicEntity _logicEntity;
        [SerializeField]
        private bool _isOneShot;
        private bool _hasActivated;

        private void Awake()
        {
            _logicEntity = GetComponent<LogicEntity>();
        }

        private void OnEnable()
        {
            if (_logicEntity == null)
            {
                _logicEntity = GetComponent<LogicEntity>();
            }

            bool canChangeInput = false;

            if (!_hasActivated || !_isOneShot)
            {
                _logicEntity.OnInputChanged.AddListener(OnInputChanged);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            if (_logicEntity == null)
            {
                _logicEntity = GetComponent<LogicEntity>();
            }
            _logicEntity.OnInputChanged.RemoveListener(OnInputChanged);
        }

        private void OnInputChanged()
        {
            if (_logicEntity.IsInputLogicValid)
            {
                Activate();
            }
        }

        private void Activate()
        {
            if (_hasActivated && _isOneShot)
            {
                return;
            }

            bool register = RegisterSequences();

            if (register)
            {
                _hasActivated = true;
            }
        }
    }

}
