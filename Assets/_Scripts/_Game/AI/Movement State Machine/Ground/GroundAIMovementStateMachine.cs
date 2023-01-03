using UnityEngine;

namespace _Scripts._Game.AI.MovementStateMachine.Ground
{

    public class GroundAIMovementStateMachine : AIMovementStateMachineBase
    {

        #region Input
        //private Vector2 _currentMovementInput = Vector2.zero;
        //private Vector2 _currentDirectionInput = Vector2.zero;
        //private bool _isMovementPressed = false;
        //private bool _isDirectionPressed = false;
        //private bool _isJumpPressed = false;
        //private bool _isDashPressed = false;
        //private bool _isFloatPressed = false;
        //private bool _isBouncePressed = false;
        //private bool _isAttackPressed = false;
        //private bool _isBashPressed = false;

        //public Vector2 CurrentMovementInput { get => _currentMovementInput; }
        //public Vector2 CurrentDirectionInput { get => _currentDirectionInput; }
        //public bool IsMovementPressed { get => _isMovementPressed; }
        //public bool IsJumpPressed { get => _isJumpPressed; }
        //public bool IsDashPressed { get => _isDashPressed; }
        //public bool IsFloatPressed { get => _isFloatPressed; }
        //public bool IsBouncePressed { get => _isBouncePressed; }
        //public bool IsAttackPressed { get => _isAttackPressed; }
        //public bool IsBashPressed { get => _isBashPressed; }
        //public bool IsDirectionPressed { get => _isDirectionPressed; set => _isDirectionPressed = value; }

        //private bool _isJumpInputValid = false;
        //private bool _isDashInputValid = false;
        //private bool _isFloatInputValid = false;
        //private bool _isBounceInputValid = false;
        //private bool _isAttackInputValid = false;
        //private bool _isBashInputValid = false;

        //public bool IsJumpInputValid { get => _isJumpInputValid; }
        //public bool IsDashInputValid { get => _isDashInputValid; }
        //public bool IsFloatInputValid { get => _isFloatInputValid; }
        //public bool IsBounceInputValid { get => _isBounceInputValid; }
        //public bool IsAttackInputValid { get => _isAttackInputValid; }
        //public bool IsBashInputValid { get => _isBashInputValid; }

        private PlayerInput _playerInput;
        private bool _isFacingRight = true;

        public bool IsFacingRight { get => _isFacingRight; }
        #endregion

        #region Player attributes
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
        private float _fallingMaximumDownwardsVelocity;
        [SerializeField]
        private float _fallingHorizontalVelocity;
        [SerializeField]
        private float _fallingAcceleration;
        [SerializeField]
        private float _fallingDeceleration;
        [SerializeField]
        private float _fallingVelocityPower;

        public float FallingGravityScale { get => _fallingGravityScale; set => _fallingGravityScale = value; }
        public float FallingMaximumDownwardsVelocity { get => _fallingMaximumDownwardsVelocity; }
        public float FallingHorizontalVelocity { get => _fallingHorizontalVelocity; }
        public float FallingAcceleration { get => _fallingAcceleration; }
        public float FallingDeceleration { get => _fallingDeceleration; }
        public float FallingVelocityPower { get => _fallingVelocityPower; }

        [Header("Collision detection")]
        [SerializeField]
        private Collider2D _closestCollider;
        [SerializeField]
        private LayerMask _collisionDetectionLayerMask;
        [SerializeField]
        private float _collidersDetectionDistance;
        [SerializeField]
        private float _directionToNormalThreshold = 0.5f;

        public Collider2D ClosestCollider { get => _closestCollider; }
        #endregion

        protected override void Awake()
        {
            base.Awake();
            
            
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
