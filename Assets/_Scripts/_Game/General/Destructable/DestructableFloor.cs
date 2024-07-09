using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Destructable{
    
    public class DestructableFloor : MonoBehaviour, IDamageable, ISaveable
    {

        [SerializeField]
        private List<EDamageType> _damageTypeExclusions;
        public List<EDamageType> DamageTypeExclusions { get { return _damageTypeExclusions; } }

        [SerializeField] 
        private int _maxHitPoints;
        private int _hitPoints;

        private void Awake()
        {
            LoadState(null);
        }

        //IDamageable
        public IExposable Exposable
        {
            get => null;
        }

        public bool IsAlive()
        {
            return _hitPoints > 0;
        }

        public void TakeDamage(EDamageType damageType, EEntityType causer, Vector3 damagePosition)
        {
            if (!IsAlive())
            {
                Debug.LogWarning("DestructableFloor: took damage but is supposed to be destroyed");
                return;
            }

            _hitPoints--;
            if (!IsAlive())
            {
                DestroyedState();
            }
        }

        private void DestroyedState()
        {
            gameObject.SetActive(false);
        }

        public Vector2 DamageDirection { get; set; }
        public Transform Transform { get; }

        //save state
        [System.Serializable]
        private struct SaveData
        {
            public int hitPoints;
        }

        public object SaveState()
        {
            return new SaveData()
            {
                hitPoints = _hitPoints
            };
        }

        public void LoadState(object state)
        {
            if (state != null)
            {
                SaveData saveData = (SaveData)state;

                _hitPoints = saveData.hitPoints;
            }
            else
            {
                _hitPoints = _maxHitPoints;
            }

            if (!IsAlive())
            {
                DestroyedState();
            }
        }
    }
    
}
