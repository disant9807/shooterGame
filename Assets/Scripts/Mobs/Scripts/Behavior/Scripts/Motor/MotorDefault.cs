using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorTypical : MonoBehaviour, IMotor
{
    //public переменные
    public float speed { get; set; } //Скорость передвижения
    
    //private переменные
    private Transform _thisTransform; // Кешируем трансформ объекта
    private Vector3 _thisVectorRotate;
    private bool _rotateNow;

    MotorTypical(Transform transform, float _speed)
    {
        _thisTransform = transform;
        _rotateNow = true;
        speed = _speed;
    }
    
    /*
    private void Start()
    {
        _thisTransform = transform;
        _rotateNow = true;
    }
    */

    public void GoToTarget()
    {
        throw new NotImplementedException();
    }

    public void GoToVector(Vector3 vector)
    {
        var position = _thisTransform.position;
        Vector3 direction = (vector - position);
        position += direction.normalized * (speed * Time.deltaTime);
        _thisTransform.position = position;
    }

    public void GoToForward(Vector3 forward)
    {
        _thisTransform.position += forward * (speed * Time.deltaTime);
    }
    
    public void Rotate(Vector3 point)
    {
        var position = _thisTransform.position;
        float rotateDegree = Vector2.Angle(Vector2.right, point - position);
        _thisTransform.eulerAngles = new Vector3(0f, 0f, position.y < point.y ? rotateDegree : -rotateDegree);
    }

    public void RotateForward(Vector3 forward)
    {
        throw new NotImplementedException();
    }

    public void RotateAngle(float angle, bool forward)
    {
        _thisTransform.eulerAngles = new Vector3(0f, 0f, forward == true ? angle : -angle);
    }
    
    public void RotateFlow(Vector3 point)
    {
        float rotateDegree = Vector2.Angle(Vector2.right, point - _thisTransform.position);
        
        if (rotateDegree > 1)
        {
            var rotation = _thisTransform.rotation;
            _thisTransform.eulerAngles = new Vector3(0f, 0f, _thisTransform.position.y < point.y ? rotation.z + speed * Time.deltaTime : rotation.z + speed * Time.deltaTime * (-1));
        }
    }
    
    public void RotateFlowAngle(float angle, bool forwardRight)
    {
        if (_rotateNow)
        {
            _thisVectorRotate = _thisTransform.right;
            _rotateNow = false;
        }
        else if (_rotateNow == false)
        {
            float thisAngle = Vector2.Angle(_thisTransform.right, _thisVectorRotate);

            if (thisAngle < angle)
            {
                var eulerAngles = _thisTransform.eulerAngles;
                eulerAngles = new Vector3(0f, 0f,
                    forwardRight == true
                        ? eulerAngles.z + speed * Time.deltaTime
                        : eulerAngles.z + speed * Time.deltaTime * (-1));
                _thisTransform.eulerAngles = eulerAngles;
            }
            else
            {
                _rotateNow = true;
            }
        }
    }
    
    public void TpToPoint(Vector3 point)
    {
        _thisTransform.position = point;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public float GetSpeed()
    {
        return speed;
    }
}
