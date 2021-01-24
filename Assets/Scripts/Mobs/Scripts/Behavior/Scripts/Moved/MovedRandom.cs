using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MovedRandom : MonoBehaviour, IMovedBehavior
{
   // public переменные
   public float minDelay { get; set; } // Минимальная дистанция перемещения до поворота
   public float maxDelay { get; set; } // Максимальная дистанция перемещения до поворота

   // private переменные
   private bool _movedAction = false; // Указывает, двигается ли монстр ?
   private bool _rotation = false; // Указывает повернулся ли монстр
   private Vector2 _point; // запоминает последнее положение монстра
   private float _period = 0; // дистанция, которую пройдет мостр до поворота
   private float _gradus = 0; // градус угла, на который повернется монстр
   private Transform _unit; // Кешируем компонент трансформ
   private MotorNow _thisMotor;
   private Vector3[] _thisPoints; // Кешируем точки за которые мы не сможем выйти
   private bool _returnMoved = false; // Возвращается ли моб назад в область ?
   private bool _hitWall = false; // Уперся в стену ?
   private Transform pointFollow;
   private float distanceFollow;

   
   /*
   void Start ()
   {
      _thisUnit = transform;
      _thisMotor = GetComponent<IMotor>();
      _gradus = Random.Range(-180f, 180f);
      _period = 0;
      if (onePoint != null && twoPoint != null && threePoint != null && foPoint != null)
      {
         _thisPoints = new[]
         {
            onePoint.transform.position, twoPoint.transform.position, threePoint.transform.position,
            foPoint.transform.position
         };
         
      }
   }
   */
   public MovedRandom(Transform unit, Vector3[] point, MotorNow motor, float minDelay, float maxDelay, Transform pointFollow = null, float distanceFollow = 0f)
   {
      _unit = unit;
      _thisPoints = point;
      _thisMotor = motor;
      this.minDelay = minDelay;
      this.maxDelay = maxDelay;
      this.pointFollow = pointFollow;
      this.distanceFollow = distanceFollow;
   }
   
   /// <summary>
   /// 
   /// </summary>
   public void Moved()
   {
      if (false)
      {
         if (pointFollow != null && Vector2.Distance(_unit.position, pointFollow.position) > distanceFollow)
         {
            _thisMotor.GoToTarget(pointFollow.position);
         }
         else if (_hitWall)
         {
            _unit.Rotate(new Vector3(0, 0, 1), Random.Range(90f, 180f));
            _hitWall = false;
         }
         else if (!_movedAction && !_rotation)
         {
            RotateState();
         }
         else if (!_movedAction && _rotation)
         {
            _point = _unit.position;
            _movedAction = true;
         }
         else if (_movedAction && _rotation)
         {
            if (Time.time < _period)
            {
               _thisMotor.GoToForward(_unit.right);
            }
            else
            {
               _movedAction = false;
               _rotation = false;
               _gradus = Random.Range(-180f, 180f);
               _period = Random.Range(minDelay, maxDelay) + Time.time;
            }
         }
         _returnMoved = false;
      }
      else if(_returnMoved == false)
      {
         _returnMoved = true;
         float[] coordinatesCenter = getCenterFigure(_thisPoints[0].x, _thisPoints[0].y, _thisPoints[2].x, _thisPoints[2].y, _thisPoints[1].x, _thisPoints[1].y, _thisPoints[3].x, _thisPoints[3].y);
         RotateState(new Vector3(coordinatesCenter[0], coordinatesCenter[1]));
      }
      else if (_returnMoved == true)
      {
         _thisMotor.GoToForward(_unit.right);
      }
   }

   private bool ExistInBorder()
   {
      if (_thisPoints != null)
      {
         for (var i = 0; i < _thisPoints.Length; i++)
         {
            if (i != _thisPoints.Length - 1)
            {
               Vector2 vectorTops = _thisPoints[i + 1] - _thisPoints[i];
               Vector2 vectorPoint = _unit.position - _thisPoints[i];
               float result = Vector3.Dot(vectorPoint, vectorTops);
               if (result < 0)
                  return false;
            }
            else
            {
               Vector2 vectorTops = _thisPoints[0] - _thisPoints[_thisPoints.Length - 1];
               Vector2 vectorPoint = _unit.position - _thisPoints[_thisPoints.Length - 1];
               float result = Vector3.Dot(vectorPoint, vectorTops);
               if (result < 0)
                  return false;
            }
         }
      }

      return true;
   }

   private float[] getCenterFigure(float x1, float y1, float x2, float y2, float twoX1, float twoY1, float twoX2, float twoY2)
   {
      var k1 = (y1 - y2) / (x1 - x2);
      var b1 = y2 - k1 * x2;
      
      var k2 = (twoY1 - twoY2) / (twoX1 - twoX2);
      var b2 = twoY2 - k2 * twoX2;

      var x = (b2 - b1) / (k1 - k2);
      var y = k1 * x + b1;

      return new[] {x, y};
      
   }
   
   private void RotateState()
   {
      float rotateDegree = Random.Range(-180f, 180f);
      bool forward = Random.Range(-1, 1) > 0;
      
      _thisMotor.RotateAngle(rotateDegree, forward);
      _rotation = true;
   }

   private void RotateState(Vector3 point)
   {
      _thisMotor.Rotate(point);
      _rotation = true;
   }
   
   private void RotateState(float min, float max)
   {
      float rotateDegree = Random.Range(min, max);
      bool forward = Random.Range(-1, 1) > 0;
      
      _thisMotor.RotateAngle(rotateDegree, forward);
      _rotation = true;
   }

   public void HitWall()
   {
      _hitWall = true;
   }
}
