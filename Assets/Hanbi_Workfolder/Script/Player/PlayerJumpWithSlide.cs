using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJumpWithSlide : MonoBehaviour
{
    [Header("Player Jump Settings")]
    [SerializeField] private float jumpForce = 10f;

    [Header("Reference")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;

    private bool isGrounded;
    private bool isSliding;
    private Rigidbody2D rb;
    private PlayerController controller;
    private Collider2D playerCollider;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        controller = GetComponent<PlayerController>();
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }

    // jump
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.linearVelocityY = jumpForce;
        }
    }

    //slide
    public void OnSlide(InputAction.CallbackContext context)
    {
        float direction = controller.moveInput;
        if (context.performed && isGrounded && isSliding)
        {
            // playerCollider 조정 및 애니메이션 적용
        }
    }   

    // gizmos
    void OnDrawGizmos()
    {
        if (groundCheck == null) return;

        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, 0.1f);
    }
}
