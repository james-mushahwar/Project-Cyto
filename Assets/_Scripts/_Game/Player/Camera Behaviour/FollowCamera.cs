using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts._Game.General;
using _Scripts._Game.Player;
using DG.Tweening;
using UnityEngine.Rendering;
using _Scripts._Game.General.Managers;
using _Scripts._Game.AI;
using _Scripts._Game.AI.AttackStateMachine;
using _Scripts._Game.AI.MovementStateMachine.Ground;
using _Scripts._Game.AI.MovementStateMachine;
using _Scripts._Game.AI.MovementStateMachine.Flying;






#if UNITY_EDITOR
using _Scripts.Editortools.Draw;
#endif

public class FollowCamera : Singleton<FollowCamera>
{
    #region Camera constraints
    [SerializeField]
    private Camera _camera;
    public Camera Camera { get { return _camera; } }
    [SerializeField]
    private AudioListener _audioListener;
    private BaseCameraBounds _cameraBounds;
    private float _defaultZOffset; // used to take the offset of the camera at the start
    private float _cameraWidth;
    private float _cameraHeight;
    #endregion

    #region Follow Behaviour
    private PlayerMovementStateMachine _ctx;
    private GameObject _playerControlledGO;
    private float _targetXOffset;
    private float _targetYOffset;
    private float _targetZOffset;
    private Tweener _targetZOffsetTweener;

    [Header("Target XY offsets")] 
    [SerializeField]
    private float _facingRightOffset;
    [SerializeField]
    private Vector2 _groundedXYCameraOffset;
    [SerializeField]
    private Vector2 _jumpingXYCameraOffset;
    [SerializeField]
    private Vector2 _fallingXYCameraOffset;
    [SerializeField]
    private Vector2 _dashingXYCameraOffset;
    [SerializeField]
    private Vector2 _floatingXYCameraOffset;
    [SerializeField]
    private Vector2 _bouncingXYCameraOffset;
    //AI offsets
    [SerializeField]
    private Vector2 _flyingAIXYCameraOffset;
    [SerializeField]
    private Vector2 _groundedAIXYCameraOffset;

    [Header("Lerp speeds")]
    [SerializeField]
    private Vector2 _groundedXYLerpSpeeds;
    [SerializeField]
    private Vector2 _jumpingXYLerpSpeeds;
    [SerializeField]
    private Vector2 _fallingXYLerpSpeeds;
    [SerializeField]
    private Vector2 _dashingXYLerpSpeeds;
    [SerializeField]
    private Vector2 _floatingXYLerpSpeeds;
    [SerializeField]
    private Vector2 _bouncingXYLerpSpeeds;
    //AI speeds
    [SerializeField]
    private Vector2 _flyingAIXYLerpSpeeds;
    [SerializeField]
    private Vector2 _groundedAIXYLerpSpeeds;

    [Header("Target Z offsets")]
    // attack
    [SerializeField]
    private float _playerAttackZOffset;
    [SerializeField]
    private float _playerAttackZOffsetDuration;
    [SerializeField]
    private Ease _playerAttackZOffsetEase;
    //exposed
    [SerializeField]
    private float _playerExposedZOffset;
    [SerializeField]
    private float _playerExposedZOffsetDuration;
    [SerializeField]
    private Ease _playerExposedZOffsetEase;

    [Header("External forces")] 
    [SerializeField]
    private Vector2 _targetXYCameraMagnitude;
    #endregion

    private void Start()
    {
        _ctx = PlayerEntity.Instance.MovementSM;
        _playerControlledGO = _ctx.gameObject;
        _defaultZOffset = transform.position.z;
        _targetZOffset = _defaultZOffset;
        Vector3 topRightBounds = _camera.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, _defaultZOffset));
        Vector3 topLeftBounds = _camera.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, _defaultZOffset));
        Vector3 bounds = topRightBounds - topLeftBounds;
        _cameraWidth = Mathf.Abs(bounds.x * 0.5f);
        _cameraHeight = Mathf.Abs(bounds.y * 0.5f);
    }


    private void FixedUpdate()
    {
        _playerControlledGO = PlayerEntity.Instance?.GetControlledGameObject();

        Vector2 lerpSpeeds = GetLerpSpeed();
        Vector3 newOffset = new Vector3(0.0f, 0.0f, _targetZOffset);
        Vector3 desiredPosition = transform.position;
        TargetOffsets();

        if (_cameraBounds != null)
        {
            TargetCameraOffsets();

            newOffset = new Vector3(_targetXOffset, _targetYOffset, _targetZOffset);

            desiredPosition = newOffset;
        }
        else
        {
            // follow behaviour X
            //_targetXOffset = _ctx.IsFacingRight == true ? _isFacingRightXOffset : -_isFacingRightXOffset;
            newOffset = new Vector3(_targetXOffset, _targetYOffset, _targetZOffset);

            if (_playerControlledGO != null)
            {
                desiredPosition = _playerControlledGO.transform.position + newOffset;
            }
        }

    #if UNITY_EDITOR
        DrawGizmos.ForPointsDebug(new Vector3(transform.position.x, transform.position.y, 0.0f), new Vector3(transform.position.x + _cameraWidth, transform.position.y, 0.0f));
        DrawGizmos.ForPointsDebug(new Vector3(transform.position.x, transform.position.y, 0.0f), new Vector3(transform.position.x, transform.position.y + _cameraHeight, 0.0f));
    #endif
        Vector3 smoothedPosition = new Vector3(Mathf.Lerp(transform.position.x, desiredPosition.x, lerpSpeeds.x * Time.deltaTime), Mathf.Lerp(transform.position.y, desiredPosition.y, lerpSpeeds.y * Time.deltaTime), desiredPosition.z);  //Vector3.Slerp(transform.position, desiredPosition, Time.deltaTime * lerpSpeeds);

        transform.position = smoothedPosition;

        // scene debug updates
        //DrawArrow.ForPointsDebug(transform.position, desiredPosition);
    }

    private Vector2 GetLerpSpeed()
    {
        Vector2 lerpSpeeds = Vector2.one;

        if (_cameraBounds != null)
        {
            return new Vector2(_cameraBounds.LerpXSpeed, _cameraBounds.LerpYSpeed);
        }

        PlayerEntity playerEntity = PlayerEntity.Instance;
        IPossessable possessed = playerEntity.Possessed;
        AIEntity aiEntity = null;

        if (possessed != null)
        {
            aiEntity = possessed.Transform.gameObject.GetComponent<AIEntity>();
            if (aiEntity != null)
            {
                AIMovementStateMachineBase movementSM = aiEntity.MovementSM;
                if (movementSM != null)
                {
                    FlyingAIMovementStateMachine flyingMovementSM = (FlyingAIMovementStateMachine)movementSM;
                    if (flyingMovementSM != null)
                    {
                        return _flyingAIXYLerpSpeeds;
                    }
                    else
                    {
                        return _groundedAIXYLerpSpeeds;
                    }
                }
            }
        }

        if (_ctx.CurrentState is GroundedMovementState)
        {
            return _groundedXYLerpSpeeds;
        }
        else if (_ctx.CurrentState is JumpingMovementState)
        {
            return _jumpingXYLerpSpeeds;
        }
        else if (_ctx.CurrentState is FallingMovementState)
        {
            return _fallingXYLerpSpeeds;
        }
        else if (_ctx.CurrentState is DashingMovementState)
        {
            return _dashingXYLerpSpeeds;
        }
        else if (_ctx.CurrentState is FloatingMovementState)
        {
            return _floatingXYLerpSpeeds;
        }
        else if (_ctx.CurrentState is BouncingMovementState)
        {
            return _bouncingXYLerpSpeeds;
        }

        return lerpSpeeds;
    }

    private void TargetOffsets()
    {
        bool facingRight = false;
        PlayerEntity playerEntity = PlayerEntity.Instance;
        IPossessable possessed = playerEntity.Possessed;
        AIEntity aiEntity = null;

        if (possessed != null)
        {
            aiEntity = possessed.Transform.gameObject.GetComponent<AIEntity>();
            facingRight = possessed.FacingRight;
            _targetXOffset = facingRight ? _facingRightOffset : -_facingRightOffset;

            if (aiEntity != null)
            {
                AIMovementStateMachineBase movementSM = aiEntity.MovementSM;
                if (movementSM != null)
                {
                    _targetXOffset *= _groundedAIXYCameraOffset.x;
                    _targetYOffset = _groundedAIXYCameraOffset.y;

                    FlyingAIMovementStateMachine flyingMovementSM = (FlyingAIMovementStateMachine)movementSM;
                    if (flyingMovementSM != null)
                    {
                        _targetXOffset *=   _flyingAIXYCameraOffset.x;
                        _targetYOffset =    _flyingAIXYCameraOffset.y;
                    }
                }
            }
        }
        else
        {
            facingRight = _ctx.IsFacingRight == true;
            _targetXOffset = facingRight ? _facingRightOffset : -_facingRightOffset;
            if (_ctx.CurrentState is GroundedMovementState)
            {
                _targetXOffset *= _groundedXYCameraOffset.x;
                _targetYOffset =  _groundedXYCameraOffset.y;
            }
            else if (_ctx.CurrentState is JumpingMovementState)
            {
                _targetXOffset *= _jumpingXYCameraOffset.x;
                _targetYOffset =  _jumpingXYCameraOffset.y;
            }
            else if (_ctx.CurrentState is FallingMovementState)
            {
                _targetXOffset *= _fallingXYCameraOffset.x;
                _targetYOffset =  _fallingXYCameraOffset.y;
            }
            else if (_ctx.CurrentState is DashingMovementState)
            {
                _targetXOffset *= _dashingXYCameraOffset.x;
                _targetYOffset =  _dashingXYCameraOffset.y;
            }
            else if (_ctx.CurrentState is FloatingMovementState)
            {
                _targetXOffset *= _floatingXYCameraOffset.x;
                _targetYOffset =  _floatingXYCameraOffset.y;
            }
            else if (_ctx.CurrentState is BouncingMovementState)
            {
                _targetXOffset *= _bouncingXYCameraOffset.x;
                _targetYOffset =  _bouncingXYCameraOffset.y;
            }
        }

        // target direction
        Transform target = null;
        if (TargetManager.Instance?.DamageableTarget != null)
        {
            target = TargetManager.Instance.DamageableTarget.Transform;
        }

        if (target == null)
        {
            if (TargetManager.Instance?.BondableTarget != null)
            {
                target = TargetManager.Instance.BondableTarget.BondTargetTransform;
            }
        }

        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            _targetXOffset += direction.x * _targetXYCameraMagnitude.x;
            _targetYOffset += direction.y * _targetXYCameraMagnitude.y;
        }

        // recently attacked
        if (PlayerEntity.Instance.AttackingSM.HasAttackedRecently())
        {
           
        }
    }

    private void TargetCameraOffsets()
    {
        if (_cameraBounds is TargetCameraBounds)
        {
            _targetXOffset = _cameraBounds.gameObject.transform.position.x;
            _targetYOffset = _cameraBounds.gameObject.transform.position.y;
            if (false)
            {
                //_targetXOffset = _ctx.IsFacingRight == true ? 1.0f : -1.0f;
                _targetXOffset = 0.0f;
                //_cameraBounds
                //float cameraX = 0.0f;
                float cameraY = 0.0f;

                if (_cameraBounds.Directions[0] == true) // up
                {
                    cameraY = transform.position.y + _cameraHeight;
                    if (_cameraBounds.BoxCollider.bounds.extents.y <= cameraY)
                    {
                        _targetYOffset = _cameraBounds.BoxCollider.bounds.extents.y - _cameraHeight;
                    }
                }
            }
        }
        
    }

    public void SetNewCameraBounds(BaseCameraBounds bounds)
    {
        if (_cameraBounds == bounds || bounds == null)
        {
            return;
        }

        _cameraBounds = bounds;
    }

    public void ClearCameraBounds(BaseCameraBounds bounds)
    {
        if (_cameraBounds != bounds)
        {
            return;
        }

        _cameraBounds = null;
    }

    public void OnAttack()
    {
        KillActiveTween(ref _targetZOffsetTweener);
        TweenZOffset(ref _targetZOffsetTweener, _defaultZOffset, _playerAttackZOffset, _playerAttackZOffsetDuration, _playerAttackZOffsetEase);
        _targetZOffsetTweener.OnComplete(() => _targetZOffset = _defaultZOffset);
    }

    public void OnExposed()
    {
        KillActiveTween(ref _targetZOffsetTweener);
        TweenZOffset(ref _targetZOffsetTweener, _defaultZOffset, _playerExposedZOffset, _playerExposedZOffsetDuration, _playerExposedZOffsetEase);
        _targetZOffsetTweener.OnComplete(() => _targetZOffset = _defaultZOffset);
    }

    private void KillActiveTween(ref Tweener tweener)
    {
        if (tweener != null)
        {
            if (tweener.IsActive())
            {
                DOTween.Kill(tweener);
                tweener = null;
            }
        }
    }

    private void TweenZOffset(ref Tweener tweener, float from, float to, float duration, Ease easeType)
    {
        tweener = DOVirtual.Float(from, to, duration, (float value) =>
        {
            _targetZOffset = value;
        }).SetEase(easeType);
    }

    public void ActivateCamera(bool set)
    {
        _audioListener.enabled = set;

        if (set)
        {
            Transform controlledTransform = PlayerEntity.Instance.GetControlledGameObject().transform;
            Vector3 newPosition = new Vector3(controlledTransform.position.x, controlledTransform.position.y, transform.position.z);
            transform.position = newPosition;
        }
    }

    public AudioListener GetAudioListener()
    {
        return _audioListener;
    }
}
