using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class JoystickControl : MonoBehaviour
{
    private Joystick _moveJoystick;
    private Joystick[] _controllers;

    private Rigidbody2D _rigidBody;
    private Transform _transform;
    private Vector2 _movementDirection;
    private float _movementSpeed;
    public Animator animator;
    private bool _facingRight = true;
    
    [FormerlySerializedAs("MOVEMENT_BASE_SPEED")]
    [Space]
    [Header("Character attributes:")]
    public float movementBaseSpeed = 30.0f;


    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
        /*
         * Временно закомментировал GetComponent. Потому выбираю его из дочернего объекта
         * Потом найду решение как это исправить
         */
        // animator = GetComponent<Animator>();

        _controllers = Joystick.FindObjectsOfType<Joystick>();

        _moveJoystick = _controllers[0].name == "MoveJoystick"
            ? _controllers[0]
            : _controllers[1];
    }

    void Update()
    {
        /*
         * Отвечает за движение персонажа и анимацию
         */
        ProcessInput();
        Move();
        Animate();
        Flip();
    }

    void ProcessInput() {
        _movementDirection = new Vector2(_moveJoystick.Horizontal, _moveJoystick.Vertical);
        _movementSpeed = Mathf.Clamp(_movementDirection.magnitude, 0.0f, 10.0f);
        _movementDirection.Normalize();
    }
    void Move () {
        _rigidBody.velocity = _movementDirection * (_movementSpeed * movementBaseSpeed);
    }

    void Flip()
    {
        if (_moveJoystick.Horizontal > 0 && !_facingRight || _moveJoystick.Horizontal < 0 && _facingRight)
        {
            _facingRight = !_facingRight;
            
            // Vector3 theScale = _transform.localScale;
            // theScale.x *= -1;
            // _transform.localScale = theScale;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    void Animate()
    {
        animator.SetFloat("Horizontal",  _movementDirection.x);
        animator.SetFloat("Vertical",  _movementDirection.y);
        animator.SetFloat("speed",  _movementSpeed);
    }
}
