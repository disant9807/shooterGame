using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickShoot : MonoBehaviour
{
    private Joystick _shootJoystick;
    private Joystick[] _controllers;
    public bool shooting;
    private Transform _transform;

    void Start()
    {
        _transform = GetComponent<Transform>();
        _controllers = Joystick.FindObjectsOfType<Joystick>();

        _shootJoystick = _controllers[0].name == "MoveJoystick"
            ? _controllers[1]
            : _controllers[0];
    }

    // Update is called once per frame
    void Update()
    {
        /*
         * Условие вращает персонажа.
         * Нужно закрепить его за оружием.
         */
        if (_shootJoystick.Horizontal != 0 || _shootJoystick.Vertical != 0)
        {
            RotateWeapon();
            shooting = true;
        } else
        {
            shooting = false;
        }
    }

    void RotateWeapon()
    {
        float angle = Mathf.Atan2(_shootJoystick.Vertical, _shootJoystick.Horizontal) * Mathf.Rad2Deg;
        _transform.eulerAngles = new Vector3(0f, 0f, angle);
    }


    public bool IsShooting()
    {
        return shooting;
    }
 }
