using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [Space]
    [Header("Character statistics:")]
    public Vector2 movementDirection;
    public float movementSpeed;

    [FormerlySerializedAs("MOVEMENT_BASE_SPEED")]
    [Space]
    [Header("Character attributes:")]
    public float movementBaseSpeed = 30.0f;

    
    [Space]
    [Header("Character references:")]
    public Rigidbody2D rigidBody;
    public Animator animator;
    private bool facingRight = true;
    
    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
        Move();
        Animate();
        Flip();
    }

    void ProcessInputs()
    {
        movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        movementSpeed = Mathf.Clamp(movementDirection.magnitude, 0.0f, 10.0f);
        movementDirection.Normalize();
    }

    void Move()
    {
        rigidBody.velocity = movementDirection * (movementSpeed * movementBaseSpeed);
    }

    void Flip()
    {
        if (Input.GetAxis("Horizontal") > 0 && !facingRight || Input.GetAxis("Horizontal") < 0 && facingRight)
        {
            facingRight = !facingRight;

            // var transform1 = transform;
            // Vector3 theScale = transform1.localScale;
            // theScale.x *= -1;
            // transform1.localScale = theScale;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    void Animate()
    {
        animator.SetFloat("Horizontal",  movementDirection.x);
        animator.SetFloat("Vertical",  movementDirection.y);
        animator.SetFloat("speed",  movementSpeed);
    }
}
