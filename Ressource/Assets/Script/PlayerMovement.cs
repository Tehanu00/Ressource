using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    private Vector3 direction;
    
    // vitesse du player
    public float speed;
    
    public float drag;
    public Vector3 gravity;
    
    // ground check
    public LayerMask groundLayer;
    public float groundCheckRadius;
    public bool grounded;

    // jump
    public float jumpForce;
    public bool IsJumping;
    public float jumpTimer;
    
    // dash
    public float dashTimer;
    public float dashCooldown;
    public float dashForce;
    public bool canDash;
    
    void Awake()
    {
        // appel le rigibody de ton player
        rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        // décide des touches pour avancer son player
        var dir = Input.GetAxis("Horizontal");
        
        // faire bouger son player que en x et le reste est en 0
        direction = new Vector3(dir, 0f, 0f);
        
        // jump on the ground of player
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            IsJumping = true;
            jumpTimer = 0.25f;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        jumpTimer += Time.deltaTime;
        if (jumpTimer < 0)
        {
            IsJumping = false;
        }

        // dash
        if (dashTimer < 0)
        {
            canDash = false;
            dashTimer -= Time.deltaTime;
        }
        else
        {
            canDash = true;
            dashTimer = 0;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (canDash)
            {
                dashTimer = dashCooldown;
                canDash = false;
                rb.AddForce(direction * dashForce, ForceMode.Impulse);
            }
            
        }
        
    }

    private void FixedUpdate()
    {
        rb.AddForce(direction * speed, ForceMode.Impulse);
        rb.drag = drag;
        
        Gravity();
        Groundcheck();
    }

    // use the gravity
    void Gravity()
    {
        rb.AddForce(gravity);
    }
    
    // use the raycast for player object for Ground
    void Groundcheck()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, groundCheckRadius, groundLayer);
    }
}
