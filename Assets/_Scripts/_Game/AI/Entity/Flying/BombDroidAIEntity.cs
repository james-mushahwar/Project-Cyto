using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts._Game.General;
using _Scripts._Game.General.Managers;
using _Scripts._Game.AI.MovementStateMachine;
using _Scripts._Game.AI.MovementStateMachine.Flying.Bombdroid;

namespace _Scripts._Game.AI.Entity.Flying{
    
    public class BombDroidAIEntity : AIEntity
    {
        public BombDroidAIMovementStateMachine BombDroidMovementSM
        {
            get { return _movementSM as BombDroidAIMovementStateMachine; }
        }

        #region Audio
        [SerializeField] 
        private AudioHandler _bombDroidMovementAudioHandler;

        public AudioHandler BombDroidMovementAudioHandler
        {
            get { return _bombDroidMovementAudioHandler; }
        }
        #endregion

       public BombDroidAIEntity()
       {
            _entity = EEntity.BombDroid;
       }

       protected override void Awake()
       {
            base.Awake();

            _bombDroidMovementAudioHandler.Owner = gameObject;
            _bombDroidMovementAudioHandler.IsActiveMethod = ShouldMovementAudioBeActive;
       }

       protected override void FixedUpdate()
       {
            base.FixedUpdate();

            UpdateAudio();
       }

       //audio handler
       private bool ShouldMovementAudioBeActive()
       {
            bool active = false;
            AIMovementState movementState = _movementSM.CurrentStateEnum;
            AIBondedMovementState bondedMovementState = _movementSM.CurrentBondedStateEnum;

            if (!IsPossessed())
            {
                if ((movementState == AIMovementState.Idle) || (movementState == AIMovementState.Patrol) || (movementState == AIMovementState.Chase) || (movementState == AIMovementState.Attack))
                {
                    active = true;
                }
            }
            else
            {
                if ((bondedMovementState == AIBondedMovementState.Flying) || (bondedMovementState == AIBondedMovementState.Attacking))
                {
                    active = true;
                }
            }
           return gameObject.activeSelf && active;
       }

       private void UpdateAudio()
       {
            bool updateMovementAudio = false; 
            AIMovementState movementState = _movementSM.CurrentStateEnum;
            AIBondedMovementState bondedMovementState = _movementSM.CurrentBondedStateEnum;
            
            // update audio handles
            if (_bombDroidMovementAudioHandler._active)
            {
                float movementAlpha = 0.0f;
                float lerpSpeed = 1.0f;
                if (!IsPossessed())
                {
                    movementAlpha = (movementState == AIMovementState.Idle || movementState == AIMovementState.Attack) ? 0.0f : 0.5f;
                    lerpSpeed = (movementState == AIMovementState.Idle || movementState == AIMovementState.Attack) ? 0.75f : 1.0f;
                    _bombDroidMovementAudioHandler.PitchAlpha = Mathf.MoveTowards(_bombDroidMovementAudioHandler.PitchAlpha, movementAlpha, lerpSpeed * Time.deltaTime);
                }
                else
                {
                    //movementAlpha = Mathf.Clamp(_movementSM.Rb.velocity.sqrMagnitude / new Vector2(BombDroidMovementSM.FlyingMaximumHorizontalVelocity, BombDroidMovementSM.FlyingMaximumVerticalVelocity).sqrMagnitude, 0.0f, 1.0f);
                    movementAlpha = Mathf.Clamp(_movementSM.Rb.velocity.sqrMagnitude / (BombDroidMovementSM.FlyingMaximumHorizontalVelocity * BombDroidMovementSM.FlyingMaximumVerticalVelocity), 0.0f, 1.0f);
                    lerpSpeed = 1.0f;
                    _bombDroidMovementAudioHandler.PitchAlpha = Mathf.MoveTowards(_bombDroidMovementAudioHandler.PitchAlpha, movementAlpha, lerpSpeed * Time.deltaTime);
                }
            }
            
            
       }
    }
    
}
