using _Scripts._Game.General.LogicController;
using _Scripts._Game.General.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts._Game.General.Spawning.Projectile{
    
    public class ProjectileSpawner : MonoBehaviour
    {
        [SerializeField]
        private EProjectileType _projectileType;
        [SerializeField]
        private EEntityType _instigator;
        [SerializeField]
        private Transform _spawnTransform;
        private ILogicEntity _logicEntity;

        [SerializeField]
        private float _delayTime = 0.0f;
        private float _delayTimer;

        private void Awake()
        {
            _logicEntity = GetComponent<ILogicEntity>();
        }

        private void Update()
        {
            if (_delayTimer >= 0.0f)
            {
                _delayTimer -= Time.deltaTime;

                if (_delayTimer <= 0.0f)
                {
                    _delayTimer = -1.0f;
                    SpawnProjectile();
                }
            }
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

        public void OnInputChanged()
        {
            if (_delayTime >= 0.0f)
            {
                _delayTime = _delayTimer;
            }
            else
            {
                SpawnProjectile();
            }
        }

        private void SpawnProjectile()
        {
            ProjectileManager.Instance.SpawnProjectileByType(_projectileType, _spawnTransform.position, _instigator);
        }
    }
    
}
