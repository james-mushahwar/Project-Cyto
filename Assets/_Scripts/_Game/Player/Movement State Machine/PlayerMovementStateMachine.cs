using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using _Scripts._Game.General;

public class PlayerMovementStateMachine : Singleton<PlayerMovementStateMachine>
{
    #region Input
    private Vector2 _currentMovementInput = Vector2.zero;
    private bool _isMovementPressed = false;
    private bool _isJumpPressed = false;
    private bool _isDashPressed = false;
    private bool _isFloatPressed = false;
    private bool _isBouncePressed = false;
    private bool _isAttackPressed = false;
    private bool _isBashPressed = false;

    public Vector2 CurrentMovementInput { get => _currentMovementInput; }
    public bool IsMovementPressed { get => _isMovementPressed; }
    public bool IsJumpPressed { get => _isJumpPressed; }
    public bool IsDashPressed { get => _isDashPressed; }
    public bool IsFloatPressed { get => _isFloatPressed; }
    public bool IsBouncePressed { get => _isBouncePressed; }
    public bool IsAttackPressed { get => _isAttackPressed; }
    public bool IsBashPressed { get => _isBashPressed; }

    private bool _isJumpInputValid = false;
    private bool _isDashInputValid = false;
    private bool _isFloatInputValid = false;
    private bool _isBounceInputValid = false;
    private bool _isAttackInputValid = false;
    private bool _isBashInputValid = false;

    public bool IsJumpInputValid { get => _isJumpInputValid; }
    public bool IsDashInputValid { get => _isDashInputValid; }
    public bool IsFloatInputValid { get => _isFloatInputValid; }
    public bool IsBounceInputValid { get => _isBounceInputValid; }
    public bool IsAttackInputValid { get => _isAttackInputValid; }
    public bool IsBashInputValid { get => _isBashInputValid; }

    private PlayerInput _playerInput;
    private bool _isFacingRight = true;

    public bool IsFacingRight { get => _isFacingRight; }
    #endregion

    #region State Machine
    private BaseMovementState _currentState;
    private PlayerMovementStateMachineFactory _states;

    public BaseMovementState CurrentState { get => _currentState; set => _currentState = value; }
    #endregion

    #region Player attributes
    private Rigidbody2D _rb;
    private CapsuleCollider2D _capsule;
    [SerializeField]
    private LayerMask _groundedLayer;

    public Rigidbody2D Rb { get => _rb; }
    public LayerMask GroundedLayer { get => _groundedLayer; }

    [Header("Grounded properties")]
    [SerializeField]
    private float _distanceToGroundThreshold = 10.0f;
    [SerializeField]
    private float _groundedGravityScale = 1.0f;
    [SerializeField]
    private float _groundedHorizontalVelocity;
    [SerializeField]
    private float _groundedAcceleration;
    [SerializeField]
    private float _groundedDeceleration;
    [SerializeField]
    private float _groundedVelocityPower;

    private float _distToGround;
    private bool _isGrounded;

    public float DistanceToGroundThreshold { get => _distanceToGroundThreshold; }
    public float GroundedGravityScale { get => _groundedGravityScale; }
    public float GroundedHorizontalVelocity { get => _groundedHorizontalVelocity; }
    public float GroundedAcceleration { get => _groundedAcceleration; }
    public float GroundedDeceleration { get => _groundedDeceleration; }
    public float GroundedVelocityPower { get => _groundedVelocityPower; }

    public float DistToGround { get => _distToGround; }
    public bool IsGrounded { get => _isGrounded; }

    [Header("Jump properties")]
    [SerializeField]
    private float _jumpForce = 1.0f;
    [SerializeField]
    private float _jumpingGravityScale;
    [SerializeField]
    private float _jumpInputForceDelay = 0.10f;
    [SerializeField]
    private float _jumpInputDuration = 1.0f;
    [SerializeField]
    private float _coyoteTimeLimit = 0.35f;
    [SerializeField]
    private float _jumpBufferLimit = 0.35f;
    private int _jumpCounter = 0;

    public float JumpForce { get => _jumpForce; }
    public float JumpingGravityScale { get => _jumpingGravityScale; }
    public float JumpInputForceDelay { get => _jumpInputForceDelay; }
    public float JumpInputDuration { get => _jumpInputDuration; }
    public float CoyoteTimeLimit { get => _coyoteTimeLimit; }
    public float JumpBufferLimit { get => _jumpBufferLimit; }
    public int JumpCounter { get => _jumpCounter; set => _jumpCounter = value; }

    [Header("Fall Properties")]
    [SerializeField]
    private float _fallingGravityScale;
    [SerializeField]
    private float _fallingHorizontalVelocity;
    [SerializeField]
    private float _fallingAcceleration;
    [SerializeField]
    private float _fallingDeceleration;
    [SerializeField]
    private float _fallingVelocityPower;

    public float FallingGravityScale { get => _fallingGravityScale; set => _fallingGravityScale = value; }
    public float FallingHorizontalVelocity { get => _fallingHorizontalVelocity; }
    public float FallingAcceleration { get => _fallingAcceleration; }
    public float FallingDeceleration { get => _fallingDeceleration; }
    public float FallingVelocityPower { get => _fallingVelocityPower; }

    [Header("Dash Properties")]
    [SerializeField]
    private float _dashingGravityScale;
    [SerializeField]
    private float _dashingForce;
    [SerializeField]
    private float _dashingStateDuration;
    [SerializeField]
    private float _dashingJumpBufferLimit;
    private int _dashCounter = 0;

    public float DashingGravityScale { get => _dashingGravityScale; }
    public float DashingForce { get => _dashingForce; }
    public float DashingStateDuration { get => _dashingStateDuration; }
    public float DashingJumpBufferLimit { get => _dashingJumpBufferLimit; }
    public int DashCounter { get => _dashCounter; set => _dashCounter = value; }

    [Header("Float Properties")]
    [SerializeField]
    private float _floatingGravityScale;
    [SerializeField]
    private float _floatingUpwardsEnterForce;
    [SerializeField]
    private float _floatingHorizontalVelocity;
    [SerializeField]
    private float _floatingMaximumDownwardsVelocity;
    [SerializeField]
    private float _floatingInputForceDelay;
    [SerializeField]
    private float _floatingHorizontalAcceleration;
    [SerializeField]
    private float _floatingHorizontalDeceleration;

    public float FloatingGravityScale { get => _floatingGravityScale; }
    public float FloatingUpwardsEnterForce { get => _floatingUpwardsEnterForce; }
    public float FloatingHorizontalVelocity { get => _floatingHorizontalVelocity; }
    public float FloatingMaximumDownwardsVelocity { get => _floatingMaximumDownwardsVelocity; }
    public float FloatingInputForceDelay { get => _floatingInputForceDelay; }
    public float FloatingHorizontalAcceleration { get => _floatingHorizontalAcceleration; }
    public float FloatingHorizontalDeceleration { get => _floatingHorizontalDeceleration; }

    [Header("Bouncing Properties")]
    [SerializeField]
    private float _bouncingUpwardGravityScale;
    [SerializeField]
    private float _bouncingDownwardGravityScale;
    [SerializeField]
    private float _bouncingMaximumHorizontalVelocity;
    [SerializeField]
    private float _bouncingDefaultHorizontalVelocity;
    [SerializeField]
    private float _bouncingMaximumDownwardsVelocity;
    [SerializeField]
    private float _bouncingDefaultDownwardsVelocity;
    [SerializeField]
    private float _bouncingInputForceDelay;
    [SerializeField]
    private Vector2 _bouncingMinMaxUpwardsBounceForce;
    [SerializeField]
    private float _bouncingHorizontalAcceleration;
    [SerializeField]
    private float _bouncingHorizontalDeceleration;
    [SerializeField]
    private float _bouncingVelocityPower;
    [SerializeField]
    private Vector3 _bouncingPowerMultiplier;
    private int _bouncingCounter = 0;

    public float BouncingUpwardGravityScale         { get => _bouncingUpwardGravityScale; }
    public float BouncingDownwardGravityScale       { get => _bouncingDownwardGravityScale; }
    public float BouncingMaximumHorizontalVelocity  { get => _bouncingMaximumHorizontalVelocity; }
    public float BouncingDefaultHorizontalVelocity  { get => _bouncingDefaultHorizontalVelocity; }
    public float BouncingMaximumDownwardsVelocity   { get => _bouncingMaximumDownwardsVelocity; }
    public float BouncingDefaultDownwardsVelocity   { get => _bouncingDefaultDownwardsVelocity; }
    public float BouncingInputForceDelay            { get => _bouncingInputForceDelay; }
    public Vector2 BouncingMinMaxUpwardsBounceForce { get => _bouncingMinMaxUpwardsBounceForce; }
    public float BouncingHorizontalAcceleration     { get => _bouncingHorizontalAcceleration; }
    public float BouncingHorizontalDeceleration     { get => _bouncingHorizontalDeceleration; }
    public int BouncingCounter { get => _bouncingCounter; set => _bouncingCounter = value; }
    public float BouncingVelocityPower { get => _bouncingVelocityPower; }
    public Vector3 BouncingPowerMultiplier { get => _bouncingPowerMultiplier; }
    #endregion

    new void Awake()
    {
        base.Awake();

        _playerInput = new PlayerInput();

        _playerInput.Enable();

        // set up player input callbacks
        _playerInput.Player.Movement.started += OnMovementInput;
        _playerInput.Player.Movement.canceled += OnMovementInput;
        _playerInput.Player.Movement.performed += OnMovementInput;

        _playerInput.Player.Jump.started += OnJumpInput;
        _playerInput.Player.Jump.canceled += OnJumpInput;

        _playerInput.Player.Dash.started += OnDashInput;
        _playerInput.Player.Dash.canceled += OnDashInput;

        _playerInput.Player.Float.started += OnFloatInput;
        _playerInput.Player.Float.canceled += OnFloatInput;
        _playerInput.Player.Float.performed += OnFloatInput;

        _playerInput.Player.Bounce.started += OnBounceInput;
        _playerInput.Player.Bounce.canceled += OnBounceInput;
        _playerInput.Player.Bounce.performed += OnBounceInput;

        _playerInput.Player.Bash.started += OnBashInput;
        _playerInput.Player.Bash.canceled += OnBashInput;

        _states = new PlayerMovementStateMachineFactory(this);
        _currentState = _states.GetState(MovementState.Grounded);
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _capsule = GetComponent<CapsuleCollider2D>();
        _distToGround = _capsule.bounds.extents.y;

        _currentState.EnterState();
    }

    // Update is called once per frame
    void Update()
    {
        // scene debug updates
        DrawArrow.ForPointsDebug(transform.position, transform.position + (-(Vector3)Vector2.up * (_distToGround + DistanceToGroundThreshold)));
    }

    void FixedUpdate()
    {
        _isGrounded = IsGroundedCheck();
        IsFacingRightCheck();
        _currentState.ManagedStateTick();
    }

    void OnMovementInput(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _isMovementPressed = _currentMovementInput.magnitude != 0.0f;
    }

    void OnJumpInput(InputAction.CallbackContext context)
    {
        _isJumpPressed = context.ReadValueAsButton();
        _isJumpInputValid = _isJumpPressed;
    }

    void OnDashInput(InputAction.CallbackContext context)
    {
        _isDashPressed = context.ReadValueAsButton();
        _isDashInputValid = _isDashPressed;
    }

    void OnFloatInput(InputAction.CallbackContext context)
    {
        _isFloatPressed = context.ReadValueAsButton();

        //Debug.Log("trigger value is = " + context.ReadValue)
        _isFloatInputValid = _isFloatPressed;
    }

    void OnBounceInput(InputAction.CallbackContext context)
    {
        _isBouncePressed = context.ReadValueAsButton();
        _isBounceInputValid = _isBouncePressed;
    }

    void OnAttackInput(InputAction.CallbackContext context)
    {
        _isAttackPressed = context.ReadValueAsButton();
        _isAttackInputValid = _isAttackPressed;
    }

    void OnBashInput(InputAction.CallbackContext context)
    {
        _isBashPressed = context.ReadValueAsButton();
        _isBashInputValid = _isBashPressed;
    }

    bool IsGroundedCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, DistToGround + DistanceToGroundThreshold, GroundedLayer);
        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }

    void IsFacingRightCheck()
    {
        if (Mathf.Abs(Rb.velocity.x) < 0.01f || _currentMovementInput.x == 0.0f)
        {
            return;
        }

        _isFacingRight = _currentMovementInput.x > 0.0f;
    }

    public void NullifyInput(MovementState state)
    {
        switch (state)
        {
            case MovementState.Jumping:
                _isJumpInputValid = false;
                break;
            case MovementState.Dashing:
                _isDashInputValid = false;
                break;
            case MovementState.Floating:
                _isFloatInputValid = false;
                break;
            case MovementState.Bouncing:
                _isBounceInputValid = false;
                break;
            default:
                break;
        }
    }
}
