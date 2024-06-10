using _Scripts._Game.General;
using _Scripts._Game.General.LogicController;
using _Scripts._Game.General.Managers;
using _Scripts._Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Prefabs.Level.Hazards.Lasers{
    
    public class LaserEntity : ContactDamageEntity
    {
        private ILogicEntity _logicEntity;
        //private Collider2D _laserCollider;
        [SerializeField]
        private GameObject _colliderGO;

        [SerializeField]
        private LayerMask _aiLayerMask;
        [SerializeField]
        private LayerMask _playerLayerMask;

        void Awake()
        {
            _logicEntity = GetComponent<LogicEntity>();
            _logicEntity.IsInputLogicValid = LogicManager.Instance.AreAllInputsValid(_logicEntity);

            //_laserCollider = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
            if (_logicEntity == null)
            {
                _logicEntity = GetComponent<LogicEntity>();
            }
            _logicEntity.OnInputChanged.AddListener(OnInputChanged);
            _logicEntity.IsEntityLogicValid = IsLaserActive;
        }

        private bool IsLaserActive()
        {
            return gameObject.activeInHierarchy;
        }

        private void OnDisable()
        {
            if (_logicEntity == null)
            {
                _logicEntity = GetComponent<LogicEntity>();
            }
            _logicEntity.OnInputChanged.RemoveListener(OnInputChanged);

            ToggleCollision(false);
        }

        private void OnInputChanged()
        {
            if (_logicEntity.IsInputLogicValid)
            {
                Activate();
            }
            else
            {
                Deactivate();   
            }
        }

        private void Activate()
        {
            Animate(0);
            ToggleCollision(true);
        }

        private void Deactivate()
        {
            Animate(1);
            ToggleCollision(false);
        }

        private void ToggleCollision(bool set)
        {
            if (set)
            {
                //_laserCollider.enabled = true;
                _colliderGO.SetActive(true);
            }
            else
            {
                //_laserCollider.enabled = false;
                _colliderGO.SetActive(false);
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            GameObject collidedGO = collider.gameObject;
            if (collidedGO != null) 
            {
                if ((_playerLayerMask.value & (1 << collidedGO.layer)) > 0)
                {
                    PlayerEntity.Instance.TakeDamage(EDamageType.BombDroid_BombDrop_DirectHit, EEntityType.Enemy, transform.position);
                }
                else if ((_aiLayerMask.value & (1 << collidedGO.layer)) > 0)
                {
                    IPossessable possessable = collidedGO.GetComponent<IPossessable>();

                    bool isPlayerPossessed = possessable != null && (PlayerEntity.Instance.Possessed == possessable);

                    if (isPlayerPossessed)
                    {
                        IDamageable damageable = collidedGO.GetComponent<IDamageable>();
                        if (damageable != null)
                        {
                            damageable.TakeDamage(EDamageType.Laser_Impact, EEntityType.BondedEnemy, transform.position);
                        }
                    }

                }
            }
        }

        private void Animate(int hash)
        {
            //_animator.enabled = true;
            //_animator.CrossFade(hash, 0, 0);
        }
    }
    
}
