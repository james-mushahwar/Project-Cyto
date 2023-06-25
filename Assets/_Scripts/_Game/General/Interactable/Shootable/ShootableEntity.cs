using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Interactable.Shootable{
    
    public class ShootableEntity : MonoBehaviour, IDamageable, IExposable
    {
        #region General
        [SerializeField] 
        private int _exposeHealth;
        #endregion

        //IDamageable
        public IExposable Exposable
        {
            get => this;
        }

        public bool IsAlive()
        {
            return _exposeHealth > 0;
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

        public Vector2 DamageDirection { get; set; }

        [SerializeField]
        private Transform _targetRoot;
        public Transform Transform
        {
            get => _targetRoot == null ? transform : _targetRoot;
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public bool IsExposed()
        {
            return _exposeHealth <= 0;
        }

        public void OnExposed()
        {
            
        }

        public void OnUnexposed()
        {
            
        }


    }
    
}
