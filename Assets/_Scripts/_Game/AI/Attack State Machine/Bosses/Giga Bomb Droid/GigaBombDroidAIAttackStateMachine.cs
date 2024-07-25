using UnityEngine;
using _Scripts._Game.AI.Bonding;
using _Scripts._Game.AI.AttackStateMachine;
using _Scripts._Game.General;
using _Scripts._Game.AI.AttackStateMachine.Flying.Bombdroid;
using System.Collections.Generic;
using _Scripts._Game.General.Managers;
using _Scripts._Game.AI.Entity.Bosses.GigaBombDroid;

namespace _Scripts._Game.AI.AttackStateMachine.Bosses.GigaBombDroid{
    
    public class GigaBombDroidAIAttackStateMachine : BossAIAttackStateMachine
    {
        private GigaBombDroidAIEntity _gbdEntity;

        #region Idle Stats
        [SerializeField]
        private List<float> _idleToShootDelay;

        public float IdleToShootDelay { get { return _idleToShootDelay[_gbdEntity.DamageState]; } }
        #endregion

        #region Attack Stats
        [SerializeField]
        private float _shootCooldown;

        public float ShootCooldown { get => _shootCooldown; }
        #endregion

        #region Cannons
        [Header("Cannons")]
        [SerializeField]
        private List<GigaBombDroidCannon> _cannons = new List<GigaBombDroidCannon>();
        [SerializeField]
        private List<float> _radiusPerDamageState = new List<float>();
        [SerializeField]
        private List<float> _orbitSpeedPerDamageState = new List<float>();

        public float Radius { get => _radiusPerDamageState[_gbdEntity.DamageState]; }
        public float OrbitSpeed { get => _orbitSpeedPerDamageState[_gbdEntity.DamageState]; }

        [SerializeField]
        private float _bondedCannonAimSpeed;
        [SerializeField]
        private Transform _cannonGroup;
        #endregion


        #region Bonded Attacks
        [Header("Attack 1 - Bomb fire")]
        [SerializeField]
        private float _postBombFireCooldownDuration;

        public float PostBombFireCooldownDuration { get => _postBombFireCooldownDuration; }
        #endregion

        protected override void Awake()
        {
            base.Awake();

            _gbdEntity = Entity as GigaBombDroidAIEntity;

            // ai attacks
            States.AddState(AIAttackState.Idle, new GigaBombDroidIdleAIAttackState(this, States));
            States.AddState(AIAttackState.Attack1, new GigaBombDroidCannonFireAIAttackState(this, States));
            //bonded attacks
            States.AddState(AIAttackState.Idle, new GigaBombDroidIdleAIBondedAttackState(this, States));
            States.AddState(AIAttackState.Attack1, new GigaBombDroidCannonFireAIBondedAttackState(this, States));

            BondInputsDict.Add(PossessInput.Movement, OnMovementInput);
            BondInputsDict.Add(PossessInput.WButton, OnWestButtonInput);

            for (int i = 0; i < _cannons.Count; i++)
            {
                GigaBombDroidCannon cannon = _cannons[i];
                if (cannon != null)
                {
                    cannon.Initialise(_gbdEntity, i);
                }

            }
        }

        public override void Tick()
        {
            if (!Entity.IsPossessed())
            {
                CurrentState.ManagedStateTick();
            }
            else
            {
                CurrentBondedState.ManagedStateTick();
            }

            if (IsAttackInterrupted)
            {
                IsAttackInterrupted = false;
            }

            //update cannons
            for (int i = 0; i < _cannons.Count; i++)
            {
                GigaBombDroidCannon cannon = _cannons[i];
                cannon.Tick();
            }

            if (!Entity.IsPossessed())
            {
                float interval = 360 / _cannons.Count;
                int damageState = _gbdEntity.DamageState;
                Vector3 posOffset = Entity.transform.position + (_radiusPerDamageState[damageState] * Vector3.up);
                float speed = _orbitSpeedPerDamageState[damageState];

                for (int i = damageState; i < _cannons.Count; i++)
                {
                    float cannonAngleOffset = interval * i;
                    GameObject cannon = _cannons[i].gameObject;
                    if (cannon != null)
                    {
                        cannon.transform.RotateAround(Entity.transform.position, Vector3.forward, speed * Time.deltaTime);

                        Vector3 direction = (cannon.transform.position - Entity.transform.position).normalized;
                        cannon.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
                    }
                }
            }
            else
            {

                bool input = _gbdEntity.MovementSM.CurrentMovementInput.sqrMagnitude > 0.0f;

                Vector3 cannonAimDirection = input ? _gbdEntity.MovementSM.CurrentMovementInput.normalized : _gbdEntity.MovementSM.Rb.velocity.normalized;

                Transform cannonGroup = _cannonGroup;
                if (cannonGroup != null && cannonAimDirection.sqrMagnitude > 0.0f)
                {
                    //Vector3 targetDirection = (cannonGroup.up - cannonAimDirection).normalized;
                    //Quaternion targetRotation = Quaternion.LookRotation(cannonAimDirection);
                    //cannonGroup.rotation = Quaternion.RotateTowards(cannonGroup.rotation, targetRotation, Time.deltaTime * _bondedCannonAimSpeed);

                    //Debug.Log("Target rotation: " + targetRotation.eulerAngles +  " Cannon group euler angles: " + cannonGroup.eulerAngles);

                    Vector3 lerpDirection = Vector3.MoveTowards(cannonGroup.up, cannonAimDirection, _bondedCannonAimSpeed * Time.deltaTime).normalized;

                    cannonGroup.rotation *= Quaternion.FromToRotation(cannonGroup.up, lerpDirection);
                    cannonGroup.rotation *= Quaternion.FromToRotation(cannonGroup.forward, Vector3.forward);

                    GameObject cannon = _cannons[3].gameObject;

                    Vector3 direction = (cannon.transform.position - Entity.transform.position).normalized;
                    cannon.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
                }
            }
        }

        public void FireCannons()
        {
            int damageState = _gbdEntity.DamageState;

            for (int i = damageState; i < _cannons.Count; i++)
            {
                if (_cannons[i].IsConnected == false)
                {
                    continue;
                }

                GameObject cannon = _cannons[i].gameObject;
                if (cannon != null)
                {
                    Vector3 direction = -cannon.transform.up;

                    EEntityType entityType = _gbdEntity.IsPossessed() ? EEntityType.BondedEnemy : EEntityType.Enemy;
                    
                    ProjectileManager.Instance.TryBombDroidBombDropProjectile(entityType, cannon.transform.position, direction);
                
                    if (entityType == EEntityType.BondedEnemy)
                    {
                        FeedbackManager.Instance.TryFeedbackPattern(EFeedbackPattern.Game_FireWeapon_Light);
                    }
                }
            }
        }

        public override void OnPossess()
        {
            base.OnPossess();

            int damageState = _gbdEntity.DamageState;

            Transform cannonTransform = _cannons[3].transform;

            cannonTransform.localPosition = new Vector3(0.0f, _radiusPerDamageState[damageState], 0.0f);
        }

        //public float GetIdleToShootDelay(int state)
        //{
        //    return _idleToShootDelay[state];
        //}
    }
    
}
