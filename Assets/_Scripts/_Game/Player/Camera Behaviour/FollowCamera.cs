using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    #region Camera constraints
    [SerializeField]
    private Camera _camera;
    private BaseCameraBounds _cameraBounds;
    private float _fixedZOffset; // used to take the offset of the camera at the start
    private float _cameraWidth;
    private float _cameraHeight;
    #endregion

    #region Follow Behaviour
    [SerializeField]
    private PlayerMovementStateMachine _ctx;
    private Transform _playerTransform;
    private float _targetXOffset;
    private float _targetYOffset;

    [Header("Target offsets")]
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
    #endregion

    private void Start()
    {
        _playerTransform = _ctx.gameObject.transform;
        _fixedZOffset = transform.position.z;
        Vector3 topRightBounds = _camera.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, _fixedZOffset));
        Vector3 topLeftBounds = _camera.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, _fixedZOffset));
        Vector3 bounds = topRightBounds - topLeftBounds;
        _cameraWidth = bounds.x * 0.5f;
        _cameraHeight = bounds.y * 0.5f;
        Debug.Log("Camera bounds = " + _cameraWidth + " and " + _cameraHeight);
    }


    private void FixedUpdate()
    {
        Vector2 lerpSpeeds = GetLerpSpeed();
        TargetOffsets();

        if (_cameraBounds != null)
        {
            TargetCameraOffsets();
        }
        else
        {
            // follow behaviour X
            //_targetXOffset = _ctx.IsFacingRight == true ? _isFacingRightXOffset : -_isFacingRightXOffset;
        }

        
        Vector3 newOffset = new Vector3(_targetXOffset, _targetYOffset, _fixedZOffset);

        Vector3 desiredPosition = _playerTransform.position + newOffset;
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
            return new Vector2(5.0f, 5.0f);
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
        _targetXOffset = _ctx.IsFacingRight == true ? 1.0f : -1.0f;
        if (_ctx.CurrentState is GroundedMovementState)
        {
            _targetXOffset *= _groundedXYCameraOffset.x;
            _targetYOffset =  _groundedXYCameraOffset.y;
            return;
        }
        else if (_ctx.CurrentState is JumpingMovementState)
        {
            _targetXOffset *= _jumpingXYCameraOffset.x;
            _targetYOffset =  _jumpingXYCameraOffset.y;
            return;
        }
        else if (_ctx.CurrentState is FallingMovementState)
        {
            _targetXOffset *= _fallingXYCameraOffset.x;
            _targetYOffset =  _fallingXYCameraOffset.y;
            return;
        }
        else if (_ctx.CurrentState is DashingMovementState)
        {
            _targetXOffset *= _dashingXYCameraOffset.x;
            _targetYOffset =  _dashingXYCameraOffset.y;
            return;
        }
        else if (_ctx.CurrentState is FloatingMovementState)
        {
            _targetXOffset *= _floatingXYCameraOffset.x;
            _targetYOffset =  _floatingXYCameraOffset.y;
            return;
        }
        else if (_ctx.CurrentState is BouncingMovementState)
        {
            _targetXOffset *= _bouncingXYCameraOffset.x;
            _targetYOffset =  _bouncingXYCameraOffset.y;
            return;
        }
    }

    private void TargetCameraOffsets()
    {
        _targetXOffset = _ctx.IsFacingRight == true ? 1.0f : -1.0f;
        //_cameraBounds
    }

    public void SetNewCameraBounds(BaseCameraBounds bounds)
    {
        if (_cameraBounds == bounds)
        {
            return;
        }


    }
}
