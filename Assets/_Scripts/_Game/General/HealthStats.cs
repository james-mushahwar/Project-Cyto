using _Scripts._Game.AI.MovementStateMachine.Flying.Bombdroid;
using _Scripts._Game.General.SaveLoad;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General{
    
    public enum EStatType
    {
        Player,
        Enemy,
    }

    public interface IDamageable
    {
        public EStatType StatType { get; }
        public float HitPoints { get; }

        public float AddHitPoints(int amount, bool react = false);
        public float RemoveHitPoints(int amount, bool react = false);
    }

    public abstract class HealthStats : IDamageable
    {
        private EStatType _statType;

        public EStatType StatType { get => _statType; protected set => _statType = value; }
        public abstract float HitPoints { get; }

        public abstract float AddHitPoints(int amount, bool react = false);
        public abstract float RemoveHitPoints(int amount, bool react = false);
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

            StatType = EStatType.Player;
        }

        public override float AddHitPoints(int amount, bool react = false)
        {
            float clampedAmount = Mathf.Min(amount, _maxHitPoints - _hitPoints);

            _hitPoints += clampedAmount;

            return _hitPoints;
        }

        public override float RemoveHitPoints(int amount, bool react = false)
        {
            float clampedAmount = Mathf.Min(amount, _hitPoints);

            _hitPoints -= clampedAmount;

            return _hitPoints;
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
        private float _hitPoints;
        private float _maxHitPoints;

        public override float HitPoints { get => _hitPoints; }

        public EnemyHealthStats(float hp, float maxHP)
        {
            _hitPoints = hp;
            _maxHitPoints = maxHP;

            StatType = EStatType.Enemy;
        }

        public override float AddHitPoints(int amount, bool react = false)
        {
            float clampedAmount = Mathf.Min(amount, _maxHitPoints - _hitPoints);

            _hitPoints += clampedAmount;

            return _hitPoints;
        }

        public override float RemoveHitPoints(int amount, bool react = false)
        {
            float clampedAmount = Mathf.Min(amount, _hitPoints);

            _hitPoints -= clampedAmount;

            return _hitPoints;
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
