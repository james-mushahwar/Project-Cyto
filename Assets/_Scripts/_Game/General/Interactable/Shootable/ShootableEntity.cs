﻿using _Scripts._Game.General.SaveLoad;
using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General.LogicController;
using _Scripts._Game.General.Managers;
using UnityEngine;

namespace _Scripts._Game.General.Interactable.Shootable{

    [RequireComponent(typeof(LogicEntity))]
    public class ShootableEntity : MonoBehaviour, IDamageable, IExposable
    {
        #region General
        [SerializeField] 
        private int _exposeHealth;

        private ILogicEntity _logicEntity;
        #endregion

        //IDamageable
        public IExposable Exposable
        {
            get { return this; }
        }

        private void Awake()
        {
            _logicEntity = GetComponent<LogicEntity>();
            _logicEntity.IsInputLogicValid = LogicManager.Instance.AreAllInputsValid(_logicEntity);
        }

        private void OnEnable()
        {
            if (_logicEntity == null)
            {
                _logicEntity = GetComponent<LogicEntity>();
            }
            _logicEntity.OnInputChanged.AddListener(OnInputChanged);
        }

        private void OnDisable()
        {
            if (_logicEntity == null)
            {
                _logicEntity = GetComponent<LogicEntity>();
            }
            _logicEntity.OnInputChanged.RemoveListener(OnInputChanged);
        }

        public bool IsAlive()
        {
            return _exposeHealth > 0 && _logicEntity.IsInputLogicValid;
        }

        public void TakeDamage(EDamageType damageType, EEntityType causer, Vector3 damagePosition)
        {
            if (!IsAlive())
            {
                return;
            }

            _exposeHealth--;

            if (_exposeHealth <= 0)
            {
                _exposeHealth = 0;
                OnExposed();
            }
        }
        private Vector2 _damageDirection;

        public Vector2 DamageDirection
        {
            get { return _damageDirection; }
            set { _damageDirection = value; }
        }

        [SerializeField]
        private Transform _targetRoot;
        public Transform Transform
        {
            get { return _targetRoot == null ? transform : _targetRoot; }
        }

        public bool IsExposed()
        {
            return _exposeHealth <= 0;
        }

        public void OnExposed()
        {
            _logicEntity.IsOutputLogicValid = true;
            LogicManager.Instance.OnOutputChanged(_logicEntity);
        }

        public void OnUnexposed()
        {
            _exposeHealth = 1;

            _logicEntity.IsOutputLogicValid = false;
            LogicManager.Instance.OnOutputChanged(_logicEntity);
        }

        private void OnInputChanged()
        {
            if (_logicEntity.IsInputLogicValid)
            {
                OnExposed();
            }
            else
            {
                if (_logicEntity.LogicType != ELogicType.Constant)
                {
                    OnUnexposed();
                }
            }
        }

    }
    
}
