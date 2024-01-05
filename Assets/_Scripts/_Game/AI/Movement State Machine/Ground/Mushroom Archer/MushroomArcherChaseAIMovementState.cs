using UnityEngine;
using _Scripts._Game.AI.MovementStateMachine;
using _Scripts._Game.AI.Entity.Ground.MushroomArcher;
using System.Xml;
using _Scripts._Game.General.Managers;
using _Scripts._Game.Player;
using _Scripts.CautionaryTalesScripts;
using Pathfinding;

namespace _Scripts._Game.AI.MovementStateMachine.Ground.MushroomArcher{
    
    public class MushroomArcherChaseAIMovementState : BaseAIMovementState
    {
        private MushroomArcherAIMovementStateMachine _maCtx;
        private MushroomArcherAIEntity _maEntity;
        private Transform _targetTransform;

        public MushroomArcherChaseAIMovementState(AIMovementStateMachineBase ctx, AIMovementStateMachineFactory factory) : base(ctx, factory)
        {
            _maCtx = ctx.GetStateMachine<MushroomArcherAIMovementStateMachine>();
            _maEntity = ctx.Entity.GetEntity<MushroomArcherAIEntity>();
        }
    
        public override bool CheckSwitchStates()
        {
            // debug settings
            if (DebugManager.Instance.DebugSettings.AIFreezeMovement)
            {
                SwitchStates(_factory.GetState(AIMovementState.Idle));
                return true;
            }

            GameObject target = PlayerEntity.Instance?.GetControlledGameObject();
            if (!CTGlobal.IsInSqDistanceRange(target, _maCtx.gameObject, _maCtx.ChaseLostDetectionSqRange))
            {
                SwitchStates(_factory.GetState(AIMovementState.Patrol));
                AudioManager.Instance.TryPlayAudioSourceAtLocation(EAudioType.SFX_Enemy_SmallEnemy_LostPlayer, _maCtx.transform.position);
                return true;
            }

            return false;
        }
    
        public override void EnterState()
        {
            _stateTimer = 0.0f;

            _maCtx.Seeker.enabled = true;
            _maCtx.DestinationSetter.enabled = true;
            _maCtx.AIPath.enabled = true;

            _maCtx.AIPath.autoRepath.mode = AutoRepathPolicy.Mode.Dynamic; // this should auto update path 

            //_targetTransform = TargetManager.Instance.GetTargetTypeTransform(ETargetType.AbovePlayer);
            //if (_targetTransform != null)
            //{
            //    _maCtx.Seeker.StartPath(_maCtx.Rb.position, _targetTransform.position);
            //    _maCtx.DestinationSetter.target = _targetTransform;
            //}

            AudioManager.Instance.TryPlayAudioSourceAtLocation(EAudioType.SFX_Enemy_SmallEnemy_DetectedPlayer, _maCtx.transform.position);
        }
    
        public override void ExitState()
        {
            
        }
    
        public override void InitialiseState()
        {
            
        }
    
        public override void ManagedStateTick()
        {
            _stateTimer += Time.deltaTime;
    
            if (CheckSwitchStates() == false)
            {
                if (_maCtx.AIPath.reachedEndOfPath)
                {
                    // move if not level with player target
                    GameObject target = PlayerEntity.Instance?.GetControlledGameObject();
                    float tolerance = _maCtx.Collider.bounds.extents.y * 0.5f;

                    bool isAlignedWithPlayer = CTGlobal.IsHorizontallyAligned(_maCtx.gameObject, target,
                        tolerance);

                    if (!isAlignedWithPlayer)
                    {
                        bool isRightOfTarget = CTGlobal.IsARightToB(_maCtx.gameObject, target);

                        _targetTransform = TargetManager.Instance?.GetTargetTypeTransform(isRightOfTarget ? ETargetType.RightPlayer : ETargetType.LeftPlayer);
                        if (_targetTransform != null)
                        {
                            _maCtx.Seeker.StartPath(_maCtx.Rb.position, _targetTransform.position);
                            _maCtx.DestinationSetter.target = _targetTransform;
                        }
                    }
                }
            }
        }
        
    }
    
}
