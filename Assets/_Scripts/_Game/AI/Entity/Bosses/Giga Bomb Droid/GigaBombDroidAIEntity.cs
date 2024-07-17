using _Scripts._Game.AI.AttackStateMachine.Bosses.GigaBombDroid;
using _Scripts._Game.AI.MovementStateMachine.Bosses.GigaBombDroid;
using _Scripts._Game.AI.MovementStateMachine.Flying.Bombdroid;
using _Scripts._Game.General;
using _Scripts._Game.General.Managers;
using _Scripts._Game.General.Spawning.AI;
using _Scripts._Game.Player;
using EZCameraShake;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.Entity.Bosses.GigaBombDroid{
    
    public class GigaBombDroidAIEntity : BossAIEntity
    {
        private int _damageState = 0;
        public int DamageState
        {
            get { return _damageState;}
        }

        public GigaBombDroidAIEntity()
        {
            _entity = General.EEntity.GigaBombDroid;
        }

        public GigaBombDroidAIMovementStateMachine GigaBombDroidMovementSM
        {
            get { return _movementSM as GigaBombDroidAIMovementStateMachine; }
        }

        public GigaBombDroidAIAttackStateMachine GigaBombDroidAIAttackSM
        {
            get { return _attackSM as GigaBombDroidAIAttackStateMachine; }
        }
    
        protected override void Awake()
        {
             base.Awake();
            
        }

        public override void Tick()
        {
             base.Tick();
        }

        public override bool TakeDamage(EDamageType damageType, EEntityType causer, Vector3 damagePosition)
        {
            if(DamageManager.CanBeDamaged(damageType, this) == false)
            {
                return false;
            }

            if (EnemyHealthStats.IsAlive() == false)
            {
                return false;
            }

            float resultHealth = 100.0f;
            bool killedOrBroken = false;

            float damageAmount = DamageManager.GetDamageFromTypeToEntity(damageType, this);

            //if (EnemyHealthStats.IsAlive())
            //{
            //    //where is the damage coming from?
            //    DamageDirection = (Transform.position - damagePosition).normalized;
            //    if (CanTakeDamage())
            //    {
            //        resultHealth = _enemyBondableHealthStats.RemoveHitPoints(damageAmount, true);
            //    }

            //    bool brokenShield = resultHealth <= 0.0f;

            //    if (brokenShield)
            //    {
            //        OnExposed();
            //        killedOrBroken = true;
            //        if (causer == EEntityType.Player || causer == EEntityType.BondedEnemy)
            //        {
            //            PlayerEntity.Instance.AttackingSM.RestartChargedMode();
            //            VolumeManager.Instance.OnExposed();
            //            FollowCamera.Instance.OnExposed();
            //        }
            //    }
            //    //broken shield
            //    FeedbackManager.Instance.TryFeedbackPattern(brokenShield ? EFeedbackPattern.Game_OnAIExposed : EFeedbackPattern.Game_BasicAttackLight);

            //    _spriteAnimator.DamageFlash();
            //    _onHitEvent.Invoke(gameObject);
            //    if (!killedOrBroken)
            //    {
            //        VolumeManager.Instance.OnBondableHit();
            //    }
            //}
            //else
            {
                //where is the damage coming from?
                DamageDirection = (Transform.position - damagePosition).normalized;
                if (CanTakeDamage())
                {
                    resultHealth = EnemyHealthStats.RemoveHitPoints(damageAmount, true);
                }

                bool killed = resultHealth <= 0.0f;

                FeedbackManager.Instance.TryFeedbackPattern(killed ? EFeedbackPattern.Game_BasicAttackHeavy : EFeedbackPattern.Game_BasicAttackLight);

                if (killed)
                {
                    // death reaction needed
                    //Despawn(true);
                    //killedOrBroken = true;
                }
                else
                {
                    OnTakeDamageEvent.Invoke(gameObject);
                    VolumeManager.Instance.OnBondableHit();
                }
                //_spriteAnimator.DamageFlash();
            }

            _damageState = (int)(EnemyHealthStats.MaxHitPoints - EnemyHealthStats.HitPoints);

            CameraShaker.Instance.ShakeOnce(killedOrBroken ? 4.0f : 2.0f, killedOrBroken ? 0.5f : 0.2f, 0.0f, 0.15f);

            return true;
        }

        public override void Spawn()
        {
            _damageState = 0;

            base.Spawn();
        }

        public override void Despawn(bool killed = false)
        {
            base.Despawn(killed);

            AIManager.Instance.UnassignSpawnedEntity(this);
        }

        public override bool IsExposed()
        {
            return DamageState >= 3;
        }

        public override bool CanBeBonded()
        {
            return !IsPossessed() && ((isActiveAndEnabled && IsExposed()) || DebugManager.Instance.DebugSettings.AlwaysBondable);
        }

        public override bool CanBePossessed()
        {
            return !IsPossessed() && ((isActiveAndEnabled && IsExposed()) || DebugManager.Instance.DebugSettings.AlwaysBondable);
        }
    }
}
