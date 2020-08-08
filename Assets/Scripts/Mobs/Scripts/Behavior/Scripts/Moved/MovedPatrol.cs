using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovedPatrol : MonoBehaviour, IMovedBehavior
{
    // public переменные
    public Vector3[] pointMoved { get; set; }

    // private переменные
    private int _currentPosition; // Текущая цель для перемещения
    private bool _back = false; // В какую сторону проходить точки патруля ?
    private Transform _thisUnit; // Кешируем компонент трансформ
    private IMotor _thisMotor;
    
    /*
    void Start ()
    {
        _thisUnit = transform;
        _thisMotor = GetComponent<IMotor>();

        if (pointsMoved != null && pointsMoved.Length > 0)
        {
            pointMoved = new Vector3[pointsMoved.Length];
            for(var t = 0; t < pointsMoved.Length; t++)
            {
                pointMoved[t] = pointsMoved[t].transform.position;
            }
        }
    }
    */
    MovedPatrol(Transform transform, Vector3[] pointMoved, IMotor motor)
    {
        _thisUnit = transform;
        _thisMotor = motor;
        this.pointMoved = pointMoved;
    }
    MovedPatrol(Transform transform, GameObject[] pointMoved, IMotor motor)
    {
        _thisUnit = transform;
        _thisMotor = motor;
        for(var t = 0; t < pointMoved.Length; t++)
        {
            this.pointMoved[t] = pointMoved[t].transform.position;
        }
    }
    public void Moved()
    {
        var position = _thisUnit.position;
        var angle = Vector2.Angle(Vector2.right, pointMoved[_currentPosition] - position);
        bool forward = position.y < pointMoved[_currentPosition].y;
        
        _thisMotor.RotateAngle(angle, forward);
        
        if (Vector2.Distance(pointMoved[_currentPosition], _thisUnit.position) > 0.1)
        {
            _thisMotor.GoToForward(_thisUnit.right);
        }
        else if (_back == false)
        {
            if (_currentPosition < pointMoved.Length -1)
            {
                _currentPosition ++;        
            }
            else
            {
                _back = true;
            }
        }
        else if (_back == true)
        {
            if (_currentPosition > 0)
            {
                _currentPosition --;
            }
            else
            {
                _back = false;
            }
        }
    }
}
