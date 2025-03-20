using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;

class PlayerMovement : MonoBehaviour
{

    [Header("Refrences")]
    public PlayerMovementStats _playerMovementStats;
    [SerializeField] private Collider2D _feetCol;
    [SerializeField] private Collider2D _bodyCol;
    [SerializeField] private Animator animator;
    private Rigidbody2D rb;

    private Vector2 moveVelocity;
    private bool isFacingRight;

    private RaycastHit2D groundHit;
    private RaycastHit2D headHit;

    private bool isGrounded;
    private bool _bumpedHead;

    // Jump vars
    public float VerticalVelocity { get; private set; }
    private bool _isJumping;
    private bool _isFastFalling;
    private bool _isFalling;
    private float _fastFallTime;
    private float _fastFallReleaseSpeed;
    private int _numberOfJumpsUsed;

    // Apex vars
    private float _apexPoint;
    private float _timePastApexThreshold;
    private bool _isPastApexThreshold;

    // Jump buffer vars
    private float _jumpBufferTimer;
    private bool _jumpReleasedDuringBuffer;

    // Coyote time vars
    private float _coyoteTimer;

    private float _fallSpeedYDampingChangeThreshold;

    private void Start()
    {
        _fallSpeedYDampingChangeThreshold = CameraManager.instance._fallSpeedDampingChangeThreshold;
    }
    private void Awake() 
    {
        isFacingRight = true;

        rb = GetComponent<Rigidbody2D>();

    }
    private void Update()
    {
        JumpChecks();
        CountTimers();

    }
    private void FixedUpdate()
    {
        CollisionCheck();
        Jump();
        if (isGrounded)
        {
            Move(_playerMovementStats.GroundAccleration, _playerMovementStats.GroundDeceleration, InputManager._movement);
        }
        else
        {
            Move(_playerMovementStats.AirAccleration, _playerMovementStats.AirDeceleration, InputManager._movement);
        }
    }
    #region Movement

    private void Move(float accleration, float decelration, Vector2 moveInput)
    {
        if ((moveInput != Vector2.zero))
        {
            Vector2 targetVelocity = Vector2.zero;
            TurnCheck(moveInput);
            if (InputManager.RunIsHeld)
            {
                targetVelocity = new Vector2(moveInput.x, 0f) * _playerMovementStats.maxRunSpeed;
            }
            else
            {
                targetVelocity = new Vector2(moveInput.x, 0f) * _playerMovementStats.maxWalkSpeed;

            }
            moveVelocity = Vector2.Lerp(moveVelocity, targetVelocity, accleration * Time.fixedDeltaTime);
            rb.velocity = new Vector2(moveVelocity.x, rb.velocity.y);
            animator.SetFloat("Speed", Mathf.Abs(moveInput.x));

        }
        else if(moveInput == Vector2.zero)
        {
            moveVelocity = Vector2.Lerp(moveVelocity, Vector2.zero, decelration * Time.fixedDeltaTime);
            rb.velocity = new Vector2(moveVelocity.x, rb.velocity.y);
            animator.SetFloat("Speed", Mathf.Abs(moveInput.x));
        }

    }

    private void TurnCheck(Vector2 moveInput)
    {
        if ((isFacingRight && moveInput.x < 0))
        {
            Turn(false);
        }
        else if(!isFacingRight && moveInput.x > 0)
        {
            Turn(true);
        }
    }

    private void Turn(bool turnRight)
    {
        if (turnRight)
        {
            isFacingRight = true;
            transform.Rotate(0f, 180f, 0f);
        }
        else
        {
            isFacingRight = false;
            transform.Rotate(0f, -180f, 0f);
        }
    }
    #endregion

    #region Jump

    private void JumpChecks()
    {
        //Debug.Log(isGrounded);
        if (InputManager.jumpWasPressed)
        {
            _jumpBufferTimer = _playerMovementStats.JumpBufferTime;
            _jumpReleasedDuringBuffer = false;
        }

        // WHEN WE RELEASE THE JUMP BUTTON
        if (InputManager.jumpWasReleased)
        {
            if (_jumpBufferTimer > 0f)
            {
                _jumpReleasedDuringBuffer = true;
            }

            if (_isJumping && VerticalVelocity > 0f)
            {
                if (_isPastApexThreshold)
                {
                    _isPastApexThreshold = false;
                    _isFastFalling = true;
                    _fastFallTime = _playerMovementStats.TimeForUpwardsCancel;
                    VerticalVelocity = 0f;
                }
                else
                {
                    _isFastFalling = true;
                    _fastFallReleaseSpeed = VerticalVelocity;
                }
            }
        }

        // INITIATE JUMP WITH JUMP BUFFERING AND COYOTE TIME
        if (_jumpBufferTimer > 0f && !_isJumping && (isGrounded || _coyoteTimer > 0f))
        {
            InitiateJump(1);

            if (_jumpReleasedDuringBuffer)
            {
                _isFastFalling = true;
                _fastFallReleaseSpeed = VerticalVelocity;
            }
        }
        // DOUBLE JUMP
        else if (_jumpBufferTimer > 0f && _isJumping && _numberOfJumpsUsed < _playerMovementStats.NumberOfJumpsAllowed)
        {
            _isFastFalling = false;
            InitiateJump(1);
        }

        // AIR JUMP AFTER COYOTE TIME LAPSED
        else if (_jumpBufferTimer > 0f && _isFalling && _numberOfJumpsUsed < _playerMovementStats.NumberOfJumpsAllowed - 1)
        {
            InitiateJump(2);
            _isFastFalling = false;
        }

        // LANDED
        if ((_isJumping || _isFalling) && isGrounded && VerticalVelocity <= 0f)
        {
            _isJumping = false;
            _isFalling = false;
            _isFastFalling = false;
            _fastFallTime = 0f;
            _isPastApexThreshold = false;
            _numberOfJumpsUsed = 0;

            VerticalVelocity = Physics2D.gravity.y;
            animator.SetBool("isJumping", _isJumping);
        }


    }
    private void InitiateJump(int numberOfJumpsUsed)
    {
        if (!_isJumping)
        {
            _isJumping = true;
        }
        animator.SetBool("isJumping", _isJumping);
        _jumpBufferTimer = 0f;
        _numberOfJumpsUsed += numberOfJumpsUsed;
        VerticalVelocity = _playerMovementStats.InitialJumpVelocity;
    }

    private void Jump()
    {
        
        if (_isJumping)
        {
            // CHECK FOR HEAD BUMP
            if (_bumpedHead)
            {
                _isFastFalling = true;
            }

            // GRAVITY ON ASCENDING
            if (VerticalVelocity >= 0f)
            {
                // APEX CONTROLS
                _apexPoint = Mathf.InverseLerp(_playerMovementStats.InitialJumpVelocity, 0f, VerticalVelocity);

                if (_apexPoint > _playerMovementStats.ApexThreshold)
                {
                    if (!_isPastApexThreshold)
                    {
                        _isPastApexThreshold = true;
                        _timePastApexThreshold = 0f;
                    }

                    if (_isPastApexThreshold)
                    {
                        _timePastApexThreshold += Time.fixedDeltaTime;
                        if (_timePastApexThreshold < _playerMovementStats.ApexHangTime)
                        {
                            VerticalVelocity = 0f;
                        }
                        else
                        {
                            VerticalVelocity = -0.01f;
                        }
                    }
                }

                // GRAVITY ON ASCENDING BUT NOT PAST APEX THRESHOLD
                else
                {
                    VerticalVelocity += _playerMovementStats.Gravity * Time.fixedDeltaTime;
                    if (_isPastApexThreshold)
                    {
                        _isPastApexThreshold = false;
                    }
                }

            }
            // GRAVITY ON DESCENDING
            else if (!_isFastFalling)
            {
                VerticalVelocity += _playerMovementStats.Gravity * _playerMovementStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }

            else if (VerticalVelocity < 0f)
            {
                if (!_isFalling)
                {
                    _isFalling = true;
                }
            }

        }
        // JUMP CUT
        if (_isFastFalling)
        {
            if (_fastFallTime >= _playerMovementStats.TimeForUpwardsCancel)
            {
                VerticalVelocity += _playerMovementStats.Gravity * _playerMovementStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }
            else if (_fastFallTime < _playerMovementStats.TimeForUpwardsCancel)
            {
                VerticalVelocity = Mathf.Lerp(_fastFallReleaseSpeed, 0f, (_fastFallTime / _playerMovementStats.TimeForUpwardsCancel));
            }

            _fastFallTime += Time.fixedDeltaTime;
        }

        // NORMAL GRAVITY WHILE FALLING
        if (!isGrounded && !_isJumping)
        {
            if (!_isFalling)
            {
                _isFalling = true;
            }

            VerticalVelocity += _playerMovementStats.Gravity * Time.fixedDeltaTime;
        }

        // CLAMP FALL SPEED
        VerticalVelocity = Mathf.Clamp(VerticalVelocity, -_playerMovementStats.MaxFallSpeed, 50f);

        rb.velocity = new Vector2(rb.velocity.x, VerticalVelocity);

    }
    #endregion

    #region Timers
    private void CountTimers()
    {
        _jumpBufferTimer -= Time.deltaTime;

        if (!isGrounded)
            _coyoteTimer -= Time.deltaTime;
        else
            _coyoteTimer = _playerMovementStats.JumpCoyoteTime;
    }

    #endregion

    #region Collision Checks
    private void IsGrounded()
    {
        Debug.Log("isGrounded");
        Vector2 boxCastOrigin = new Vector2(_feetCol.bounds.center.x, _feetCol.bounds.min.y);
        Vector2 boxCastSize = new Vector2(_feetCol.bounds.size.x, _playerMovementStats.GroundDetectionRayLength);

        groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, _playerMovementStats.GroundDetectionRayLength, _playerMovementStats.GroundLayer);
        
        //Debug.Log(groundHit.collider.gameObject.name);
        if(groundHit.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false; 
        }
        #region Debug Visualization
        if (_playerMovementStats.DebugShowIsGroundedBox)
        {
            Color rayColor;
            if (isGrounded)
            {
                rayColor = Color.green;
            }
            else
            {
                rayColor = Color.red;
            }

            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * _playerMovementStats.GroundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * _playerMovementStats.GroundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y - _playerMovementStats.GroundDetectionRayLength), Vector2.right * boxCastSize.x, rayColor);
        }
        #endregion


    }
    private void HeadCheck()
    {
        Vector2 boxCastOrigin = new Vector2(_feetCol.bounds.center.x, _bodyCol.bounds.max.y);
        Vector2 boxCastSize = new Vector2(_feetCol.bounds.size.x * _playerMovementStats.HeadWidth, _playerMovementStats.HeadDetectionRayLength);
        headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, _playerMovementStats.HeadDetectionRayLength, _playerMovementStats.GroundLayer);

        if(headHit.collider != null)
        {
            _bumpedHead = true;
        }
        else
        {
            _bumpedHead = false;
        }
    }
    private void CollisionCheck()
    {
        IsGrounded();
        HeadCheck();
    }
    #endregion
}
