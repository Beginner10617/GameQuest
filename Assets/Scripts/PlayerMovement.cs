using UnityEngine;
using UnityEngine.InputSystem;

[SerializeField] class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private bool invokingJump;

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpAnimationDelay = 0.1f;

    [SerializeField] float jumpForce = 14f;  // Increased for a stronger jump
    [SerializeField] float fallMultiplier = 3.5f;  // Faster falling for a snappier feel
    [SerializeField] float lowJumpMultiplier = 3f; // More control over jump height
    [SerializeField] float coyoteTime = 0.1f;  // Small window for late jumps

    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.2f;

    private bool isGrounded;
    private bool isJumping;
    private float moveInput;
    private float coyoteTimeCounter;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
    }

    private void Update()
    {
        
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        moveInput = moveAction.ReadValue<Vector2>().x;

        
        if (moveInput < 0)
            transform.eulerAngles = new Vector3(0, 180, 0);
        else if (moveInput > 0)
            transform.eulerAngles = new Vector3(0, 0, 0);

        
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        
        if (jumpAction.triggered && coyoteTimeCounter > 0)
        {
            isJumping = true;
            coyoteTimeCounter = 0;
        }

        if(invokingJump)
        {
            animator.SetBool("isGrounded", false);
        }
        else
        {
            animator.SetBool("isGrounded", isGrounded);
        }
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
    }

    private void FixedUpdate()
    {
        
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        
        if (isJumping)
        {
            Invoke("Jump", jumpAnimationDelay);
            invokingJump = true;
            isJumping = false;
        }

        
        if (rb.velocity.y < 0) 
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0 && !jumpAction.IsPressed()) // Letting go of jump early
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        invokingJump = false;
    }
}
