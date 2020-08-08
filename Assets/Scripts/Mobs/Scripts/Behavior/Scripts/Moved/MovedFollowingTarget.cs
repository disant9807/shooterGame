using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovedFollowingObj : MonoBehaviour, IMovedBehavior
{
    //Public переменные
    public Transform unit { get; set; }
    public float distance { get; set; }
    public float maxDistance { get; set; }

    // Private переменные
    private Transform _thisUnit; // Кешируем компонент трансформ
    private bool _movedAction = false; // Указывает, двигается ли монстр ?
    private bool _rotation = false; // Указывает повернулся ли монстр
    private Vector2 _point; // запоминает последнее положение монстра
    private float _distance = 0; // дистанция, которую пройдет мостр до поворота
    private float _gradus = 0; // градус угла, на который повернется монстр
    private IMotor _thisMotor;
    
    /*
    void Start ()
    {
        _thisUnit = transform;
        _thisMotor = GetComponent<IMotor>();
    }
    */
    public MovedFollowingObj(Transform transform, Transform unit, IMotor motor, float distance, float maxDistance)
    {
        _thisUnit = transform;
        _thisMotor = motor;
        this.distance = distance;
        this.maxDistance = maxDistance;
        this.unit = unit;
    }
    
    public MovedFollowingObj(Transform transform, IMotor motor, float distance, float maxDistance)
    {
        _thisUnit = transform;
        _thisMotor = motor;
        this.distance = distance;
        this.maxDistance = maxDistance;
    }
    
    public void Moved()
    {
        float thisDistance = Vector2.Distance(unit.position, _thisUnit.position);
        
        if ( thisDistance > distance && thisDistance < maxDistance)
        {
            var position = _thisUnit.position;
            var position1 = unit.position;
            var angle = Vector2.Angle(Vector2.right, position1 - position);
            _thisUnit.eulerAngles = new Vector3(0f, 0f, position.y < position1.y ? angle : -angle);
            
            _thisMotor.GoToForward(_thisUnit.right);
        }
        else if (thisDistance > distance && thisDistance > maxDistance)
        {
            var position = unit.position;
            Vector3 tpPoint = new Vector3()
            {
                x = Random.Range(position.x - distance, position.x + distance),
                y = Random.Range(position.y - distance, position.y + distance)
            };
            
            _thisMotor.TpToPoint(tpPoint);
        }
        else
        {
            this.RandomMoved(distance/4, distance/2);
        }
    }
    
    public void RandomMoved( float minDis, float maxDis)
    {
        switch (_movedAction)
        {
            case false when _rotation == false:
                RotateState();
                break;
            case false when _rotation == true:
                _distance = Random.Range(minDis, maxDis);
                _point = _thisUnit.position;
                _movedAction = true;
                break;
            case true when _rotation == true:
            {
                if (Vector2.Distance(_point, _thisUnit.position) < _distance)
                    _thisMotor.GoToForward(_thisUnit.right);
                else
                {
                    _movedAction = false;
                    _rotation = false;
                    _gradus = Random.Range(-180f, 180f);
                }

                break;
            }
        }
    }
    
    private void RotateState()
    {
        float rotateGradus = Random.Range(-180f, 180f);
        bool forward = Random.Range(-1, 1) > 0;
        _thisMotor.RotateAngle(rotateGradus, forward);
        _rotation = true;
    }
}
