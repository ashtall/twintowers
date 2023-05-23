using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    public LayerMask groundLayer;
    public GroundCheck groundCheck;

    private float horizontal;
    public float moveSpeed = 5f;
    public float jumpHeight = 5f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private bool isFacingRight = true;
    private bool isGrounded = false;

    //custom Gravity
    public float gravityScale = 1f;
    public static float globalGravity = -9.81f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    private void Update()
    {
        isGrounded = groundCheck.isGrounded;

        //if (rb.velocity.y < 0)
        //{
        //    rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        //}
        //else if (rb.velocity.y > 0)
        //{
        //}
    }

    private void FixedUpdate()
    {
        //custom gravity
        Vector3 gravity = globalGravity * gravityScale * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);

        //movement
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);

        //flipping
        if (!isFacingRight && horizontal > 0f)
        {
            Flip();
        }
        else if (isFacingRight && horizontal < 0f)
        {
            Flip();
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            float jumpingPower = Mathf.Sqrt(jumpHeight * (Physics.gravity.y * gravityScale) * -2) * rb.mass;
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }
}