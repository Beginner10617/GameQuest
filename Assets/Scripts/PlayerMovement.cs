using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;

    public float moveSpeed = 5f;
    public float jumpForce = 14f;  // Increased for a stronger jump
    public float fallMultiplier = 3.5f;  // Faster falling for a snappier feel
    public float lowJumpMultiplier = 3f; // More control over jump height
    public float coyoteTime = 0.1f;  // Small window for late jumps

    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

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

        
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        animator.SetBool("isGrounded", isGrounded);
    }

    private void FixedUpdate()
    {
        
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        
        if (isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
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
}
