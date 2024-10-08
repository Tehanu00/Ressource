using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Apple.ReplayKit;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    private Vector3 direction;
    public Rigidbody boxRb;
    private Vector3 boxPos;
    
    // vitesse du player
    public float speed;
    public float runSpeed;
    public bool run;
    
    public float drag;
    public Vector3 gravity;
    
    // ground check
    public LayerMask groundLayer;
    public float groundCheckRadius;
    public bool grounded;
    
    // object caisse check
    public LayerMask BoxLayer;
    public float boxCheckRadius;
    public bool boxCheck;
    
    // push
    public float pushForce;

    // jump
    public float jumpForce;
    
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
        
        // savoir si le joueur cours en appuyant sur une certaine touche
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            run = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            run = false;
        }
        
        // jump on the ground of player
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    
        // push une caisse
        if (Input.GetKey(KeyCode.E) && boxCheck)
        {
            
            if (direction.x > 0)
            {
                boxRb.AddForce(Vector3.right * pushForce, ForceMode.Impulse);
            }
            if (direction.x < 0)
            {
                boxRb.AddForce(Vector3.left * pushForce, ForceMode.Impulse);
            }
        }
    }
    private void FixedUpdate()
    {
        rb.AddForce(direction * speed, ForceMode.Impulse);
        
        // augmenter la vitesse du player s'il cours
        if (run) 
        {
            rb.AddForce(direction * runSpeed, ForceMode.Impulse);
        }
        
        rb.drag = drag;
        
        Gravity();
        Groundcheck();
        BoxCheck();
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

    void BoxCheck()
    {
        boxCheck = Physics.Raycast(transform.position, Vector3.right * transform.localScale.x, out RaycastHit hit,boxCheckRadius, BoxLayer);
        if (boxCheck)
        {
            boxRb = hit.transform.GetComponent<Rigidbody>();
        }
    }
}
