using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FindAngry : MonoBehaviour, IFind
{
    public float distanceFind; // Дистанция для поиска

    private bool _actionFind; // Идет ли поиск ?
    private float _rangePoint; // Расстояние между отрезками поиска
    private Transform _thisTransform; // Кешируем трансформ

    private float _lastDistance; //Дистанция видимости до поиска
    private float _lastAngle; //Последний угол видимости
    
    private Vector3 _lastPosition;
    private Vector3 _rotate;
    private IMotor _thisMotor;
    private IVisible _thisVisible;
    private bool _goToLineBool = true;
    private bool _goToRotate = true;

    /*
    private void Start()
    {
        _thisMotor = gameObject.GetComponent<IMotor>();
        _thisVisible = gameObject.GetComponent<IVisible>();
        _lastDistance = _thisVisible.GetDistanceVisible();
        _lastAngle = _thisVisible.GetAngleVisible();
    }
    */

    public FindAngry(Transform transform, IMotor motor, IVisible visible, float distanceFind)
    {
        _thisTransform = transform;
        _thisMotor = motor;
        _thisVisible = visible;
        this.distanceFind = distanceFind;
        
        _lastDistance = _thisVisible.GetDistanceVisible();
        _lastAngle = _thisVisible.GetAngleVisible();
    }

    public void Find()
    {
        _thisVisible.SetDistanceVisible(distanceFind);
        _thisVisible.SetAngleVisible(30f);
        _thisMotor.RotateFlowAngle(359f, true);
        
    }

    public void EndFind()
    {
        _thisVisible.SetDistanceVisible(_lastDistance);
        _thisVisible.SetAngleVisible(_lastAngle);
    }
    
    public void FindForward(Vector3 forward, float range)
    {
        throw new System.NotImplementedException();
    }

    public void FindTarget(Vector3 target, float range)
    {
        _thisVisible.SetDistanceVisible(distanceFind);
        _thisVisible.SetAngleVisible(30f);
        
        _thisMotor.Rotate(target);
    }

 
}
