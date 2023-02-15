﻿using _Scripts._Game.Animation;
using _Scripts._Game.Animation.Character.Player;
using _Scripts._Game.General;
using _Scripts._Game.General.Managers;
using _Scripts._Game.Player.AttackingStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.Player{
    
    public enum ERespawnReason
    {
        None,
        Death,
        QuickRespawn,
        OutOfBounds,
    }
    public struct RespawnReason
    {
        public bool _isRespawning;
        public ERespawnReason _reaspawnReason;
        public float _respawnTimer;
    }

    public class PlayerEntity : Singleton<PlayerEntity>, IPossessable, IDamageable
    {
        #region Properties
        private bool _isPossessed;
        private IPossessable _possessed; //save what we're possessing
        private RespawnReason _playerRespawnReason;
        #endregion

        #region State Machines
        private PlayerMovementStateMachine _movementSM;
        private PlayerAttackingStateMachine _attackingSM;
        private PlayerSpriteAnimator _spriteAnimator;

        public PlayerMovementStateMachine MovementSM { get => _movementSM; set => _movementSM = value; }
        public PlayerAttackingStateMachine AttackingSM { get => _attackingSM; set => _attackingSM = value; }
        public PlayerSpriteAnimator SpriteAnimator { get => _spriteAnimator; set => _spriteAnimator = value; }
        #endregion

        #region Player Components
        private PlayerHealthStats _playerHealthStats;

        public PlayerHealthStats PlayerHealthStats { get => _playerHealthStats; }
        #endregion

        protected override void Awake()
        {
            base.Awake();

            FEntityStats playerEntityStats = StatsManager.Instance.GetEntityStat(EEntity.Player);
            _playerHealthStats = new PlayerHealthStats(playerEntityStats.MaxHealth, playerEntityStats.MaxHealth);
        }

        void Start()
        {
            OnPossess();
        }

        private void FixedUpdate() 
        {
            if (_playerRespawnReason._isRespawning == true)
            {
                if ((_playerRespawnReason._respawnTimer -= Time.deltaTime) < 0.0f)
                {
                    //respawn
                    transform.position = new Vector3(26.0f, 146.0f, 0.0f);
                    _playerRespawnReason._reaspawnReason = ERespawnReason.None;
                    _playerRespawnReason._isRespawning = false;

                    FEntityStats playerEntityStats = StatsManager.Instance.GetEntityStat(EEntity.Player);
                    _playerHealthStats.AddHitPoints(playerEntityStats.MaxHealth);
                }
            }
        }

        public GameObject GetControlledGameObject()
        {
            if (!IsPossessed())
            {
                return _possessed.Transform.gameObject; 
            }
            else
            {
                return Transform.gameObject;
            }
        }

        // IPossessable
        public bool IsPossessed()
        {
            return _isPossessed;
        }

        public bool CanBePossessed()
        {
            return !_isPossessed && _playerHealthStats.IsAlive();
        }

        public void OnPossess()
        {
            InputManager.Instance.TryEnableActionMap(EInputSystem.Player);
            _isPossessed = true;
            _possessed = null;

            _movementSM.Rb.isKinematic = false;
            _movementSM.enabled = true;
            _attackingSM.enabled = true;
            _spriteAnimator.enabled = true;
        }

        public void OnDispossess()
        {
            // dispossess this player for something else
            InputManager.Instance.TryDisableActionMap(EInputSystem.Player);
            _isPossessed = false;

            if (_movementSM.BondableTarget != null)
            {
                _possessed = _movementSM.BondableTarget;
                _possessed.OnPossess();
            }

            gameObject.transform.SetParent(_movementSM.BondableTarget.Transform);

            _movementSM.Rb.isKinematic = true;
            _movementSM.enabled = false;
            _attackingSM.enabled = false;
            _spriteAnimator.enabled = false;
        }

        public Transform Transform { get => transform; }

        public Vector2 GetMovementInput()
        {
            return _movementSM.CurrentMovementInput;
        }

        public bool IsAlive()
        {
            return _playerHealthStats.IsAlive() && !_playerRespawnReason._isRespawning;
        }

        public void TakeDamage(float damage, EEntityType causer)
        {
            float resultsHealth = 0.0f;
            resultsHealth = _playerHealthStats.RemoveHitPoints(damage, false);

            if (resultsHealth <= 0.0f)
            {
                //dead reaction
                if (_playerRespawnReason._isRespawning == false)
                {
                    _playerRespawnReason._reaspawnReason = ERespawnReason.Death;
                    OnRespawnStart();
                }
            }
        }

        public void OnRespawnStart()
        {
            _playerRespawnReason._isRespawning = true;
            if (_playerRespawnReason._reaspawnReason == ERespawnReason.Death)
            {
                _playerRespawnReason._respawnTimer = 3.0f;
            }
            else
            {
                _playerRespawnReason._respawnTimer = 1.0f;
            }
        }
    }
    
}
