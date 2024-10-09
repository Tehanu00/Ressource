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
    public float raycastCubeLenght;
    public bool boxCheck;
    private Vector3 raycastDir;
    private Vector3 lastDirection;
    
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
        if (direction.magnitude > 0.9f && !Input.GetKey(KeyCode.E))
        {
            lastDirection = new Vector3(Mathf.RoundToInt(direction.x), 0, Mathf.RoundToInt(direction.z));
        }
        
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
            if (boxRb.transform.parent == null)
            {
                boxRb.transform.SetParent(transform);
                var offset = 0.2f;
                boxRb.transform.position += lastDirection * offset;
            }
        }
        else
        {
            boxRb.transform.SetParent(null);
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
        raycastDir = new Vector3(Input.GetKey(KeyCode.E) ? lastDirection.x : direction.x, 0, 0);
        boxCheck = Physics.Raycast(transform.position, raycastDir, out RaycastHit hit, raycastCubeLenght, BoxLayer);
        if (boxCheck)
        {
            boxRb = hit.transform.GetComponent<Rigidbody>();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, raycastDir * raycastCubeLenght);
    }
}
