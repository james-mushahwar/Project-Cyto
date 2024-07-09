using UnityEngine;
using _Scripts._Game.AI.Bonding;
using _Scripts._Game.AI.AttackStateMachine;
using _Scripts._Game.General;
using _Scripts._Game.AI.AttackStateMachine.Flying.Bombdroid;
using System.Collections.Generic;
using _Scripts._Game.General.Managers;

namespace _Scripts._Game.AI.AttackStateMachine.Bosses.GigaBombDroid{
    
    public class GigaBombDroidAIAttackStateMachine : BossAIAttackStateMachine
    {
        #region Idle Stats
        [SerializeField]
        private List<float> _idleToShootDelay;
        #endregion

        #region Attack Stats
        [SerializeField]
        private float _shootCooldown;

        public float ShootCooldown { get => _shootCooldown; }
        #endregion

        #region Cannons
        [Header("Cannons")]
        [SerializeField]
        private List<GameObject> _cannons = new List<GameObject>();
        [SerializeField]
        private List<float> _radiusPerDamageState = new List<float>();
        [SerializeField]
        private List<float> _orbitSpeedPerDamageState = new List<float>();

        #endregion

        protected override void Awake()
        {
            base.Awake();

            // ai attacks
            States.AddState(AIAttackState.Idle, new GigaBombDroidIdleAIAttackState(this, States));
            States.AddState(AIAttackState.Attack1, new GigaBombDroidCannonFireAIAttackState(this, States));
            //bonded attacks
            States.AddState(AIAttackState.Idle, new GigaBombDroidIdleAIBondedAttackState(this, States));

            BondInputsDict.Add(PossessInput.Movement, OnMovementInput);
            BondInputsDict.Add(PossessInput.WButton, OnWestButtonInput);
        }

        public override void Tick()
        {
            if (!Entity.IsPossessed())
            {
                CurrentState.ManagedStateTick();
            }
            else
            {
            }
    
            if (IsAttackInterrupted)
            {
                IsAttackInterrupted = false;
            }

            //update cannons
            float interval = 360 / _cannons.Count;
            Vector3 posOffset = Entity.transform.position + (_radiusPerDamageState[0] * Vector3.up);
            float speed = _orbitSpeedPerDamageState[0];

            for (int i = 0; i < _cannons.Count; i++)
            {
                float cannonAngleOffset = interval * i;
                GameObject cannon = _cannons[i];
                if (cannon != null)
                {
                    cannon.transform.RotateAround(Entity.transform.position, Vector3.forward, speed * Time.deltaTime);

                    Vector3 direction = (cannon.transform.position - Entity.transform.position).normalized;
                    cannon.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
                }
            }
        }

        public void FireCannons()
        {
            for (int i = 0; i < _cannons.Count; i++)
            {
                GameObject cannon = _cannons[i];
                if (cannon != null)
                {
                    Vector3 direction = -cannon.transform.up;
                    ProjectileManager.Instance.TryBombDroidBombDropProjectile(General.EEntityType.Enemy, cannon.transform.position, direction);
                }
            }
        }

        public float GetIdleToShootDelay(int state)
        {
            return _idleToShootDelay[state];
        }
    }
    
}
