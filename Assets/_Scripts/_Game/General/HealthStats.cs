using _Scripts._Game.AI.MovementStateMachine.Flying.Bombdroid;
using _Scripts._Game.General.SaveLoad;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General{
    
    public enum EHealthStatType
    {
        Player,
        EnemyHealth,
        BondableHealth
    }

    public enum EEntityType
    {
        Player, 
        Ally,
        Enemy,
        BondedEnemy
    }


    public interface IDamageable
    {
        bool IsAlive();
        void TakeDamage(float damage, EEntityType causer);
        //Components
        Transform Transform { get; }
        //Inputs
        Vector2 GetMovementInput();
    }

    public abstract class HealthStats
    {
        private EHealthStatType _statType;

        public EHealthStatType StatType { get => _statType; protected set => _statType = value; }
        public abstract float HitPoints { get; }

        public abstract float AddHitPoints(float amount, bool react = false);
        public abstract float RemoveHitPoints(float amount, bool react = false);

        public abstract bool IsAlive();
    }

    public class PlayerHealthStats : HealthStats, ISaveable
    {
        private float _hitPoints;
        private float _maxHitPoints;

        public override float HitPoints { get => _hitPoints; }

        public PlayerHealthStats(float hp, float maxHP)
        {
            _hitPoints = hp;
            _maxHitPoints = maxHP;

            StatType = EHealthStatType.Player;
        }

        public override float AddHitPoints(float amount, bool react = false)
        {
            float clampedAmount = Mathf.Min(amount, _maxHitPoints - _hitPoints);

            _hitPoints += clampedAmount;

            return _hitPoints;
        }

        public override float RemoveHitPoints(float amount, bool react = false)
        {
            float clampedAmount = Mathf.Min(amount, _hitPoints);

            _hitPoints -= clampedAmount;

            return _hitPoints;
        }

        public override bool IsAlive()
        {
            return _hitPoints > 0.0f;
        }

        // ISaveable
        [System.Serializable]
        private struct SaveData
        {
            public float _hitPoints;
            public float _maxHitPoints;
        }

        public object SaveState()
        {
            return new SaveData()
            {
                _hitPoints = _hitPoints,
                _maxHitPoints = _maxHitPoints
            };
        }

        public void LoadState(object state)
        {
            SaveData saveData = (SaveData)state;

            _hitPoints = saveData._hitPoints;
            _maxHitPoints = saveData._maxHitPoints;
        } 
    }

    public class EnemyHealthStats : HealthStats, ISaveable
    {
        #region Normal Health
        private float _hitPoints;
        private float _maxHitPoints;

        public override float HitPoints { get => _hitPoints; }
        #endregion

        public EnemyHealthStats(float hp, float maxHP, EHealthStatType statType)
        {
            _hitPoints = hp;
            _maxHitPoints = maxHP;

            StatType = statType;
        }

        public override float AddHitPoints(float amount, bool react = false)
        {
            float clampedAmount = Mathf.Min(amount, _maxHitPoints - _hitPoints);

            _hitPoints += clampedAmount;

            return _hitPoints;
        }

        public override float RemoveHitPoints(float amount, bool react = false)
        {
            float clampedAmount = Mathf.Min(amount, _hitPoints);

            _hitPoints -= clampedAmount;

            return _hitPoints;
        }

        public override bool IsAlive()
        {
            return _hitPoints > 0.0f;
        }

        // ISaveable
        [System.Serializable]
        private struct SaveData
        {
            public float _hitPoints;
            public float _maxHitPoints;
        }

        public object SaveState()
        {
            return new SaveData()
            {
                _hitPoints = _hitPoints,
                _maxHitPoints = _maxHitPoints
            };
        }

        public void LoadState(object state)
        {
            SaveData saveData = (SaveData)state;

            _hitPoints = saveData._hitPoints;
            _maxHitPoints = saveData._maxHitPoints;
        }
    }
}
