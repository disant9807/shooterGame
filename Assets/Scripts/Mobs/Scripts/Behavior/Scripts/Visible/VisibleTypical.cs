using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class VisibleTypical : MonoBehaviour, IVisible
{
    //public переменные
    public int angle { get; set; } // Угол видимости
    public float distance { get; set; } // Дистанция видимости
    public LayerMask mask { get; set; } // Слой видимости. например будет ли видеть сквозь окно ?
    public string tagMask { get; set; } // Tag который будет чекаться скриптом
    
    // peivate переменные
    private Transform _thisTransform;
    private LineRenderer _thisLine;
    // Цель для поиска. Чувствительна к регистру.
    private GameObject _target;
    public float scaleRadius;
    
    /*
    void Start()
    {
        _target = GameObject.FindWithTag(tagMask);
        _thisTransform = transform;
        gameObject.AddComponent<LineRenderer>();
        _thisLine = GetComponent<LineRenderer>();
    }
    */
    public VisibleTypical(Transform transform, LineRenderer lineRenderer, GameObject target, LayerMask mask, int angle, float distance, string tagMask)
    {
        _thisTransform = transform;
        _thisLine = lineRenderer;
        _target = target;
        this.angle = angle;
        this.distance = distance;
        this.tagMask = tagMask;
        this.mask = mask;
        //RenderLine.DrawSector(angle, distance,0.1f,lineRenderer);
    }

    private void Update()
    {
        //Debug.Log("Рисование сектора");
        //RenderLine.DrawSector(angle, distance/scaleRadius, 0.1f, _thisLine);
        //RenderLine.DrawSector(angle, distance - (distance * 0.08f), 0.1f, _thisLine);
    }

    public bool IsVisible()
    {
        return IsVisibleObject(_thisTransform, _target, angle, distance, mask);
    }

    public float DistanceVisible()
    {
        return Vector2.Distance(_thisTransform.position, _target.transform.position);
    }

    public GameObject GetObjVisible()
    {
        return _target;
    }
    
    /*
     * Проверяем видны ли мы объекту из одной его точки
     * from - наше местоположение
     * point - одна из точек объекта
     * target - где находится цель
     * angle - угол видимости объекта
     * distance - ддистанция или дальность видимости объекта
     * mask - маска слоев
     */

    public bool IsVisibleObject(Transform point, GameObject target, float angleNew, float distanceNew, LayerMask maskNew)
    {
        if (target == null)
            return false;
        
        bool result = false;
        if (IsAvailablePoint(target.transform, point, angleNew, distanceNew))
        {
            print("IsAvailablePoint: " + target);
            var position = target.transform.position;
            Vector2 from2D = new Vector2(position.x, position.y);
            var position1 = point.position;
            Vector2 point2D = new Vector2(position1.x, position1.y);

            //находим вектор которые показывает на нашу цель от нас
            Vector2 direction = (point2D - from2D);
            //Запускаем луч по вектору и если он пересекается с нужной целью, то true
            RaycastHit2D hit = Physics2D.Raycast(from2D, direction, distanceNew, maskNew.value);

            result = true;
            if (hit)
            {
                if (hit.collider.CompareTag(tagMask))
                {
                    result = true;
                }
            }

        }
        return result;
    }

   /*
    * Здесь просто проверяется попадаем ли мы в область видимости объекта, без учета стен и преград
    * Высчитывется угол между нашими векторами и дистанцию. Если мы попадаем в угол и дистанцию, то true  
    * from - наше местоположение
    * point - одна из точек объекта
    * target - где находится объект
    * angle - угол видимости объекта
    * distance - ддистанция или дальность видимости объекта
    */

    public bool IsAvailablePoint(Transform from, Transform point, float angleNew, float distanceNew)
    {
        var position = from.position;
        Vector2 from2D = new Vector2(position.x, position.y);
        var position1 = point.position;
        Vector2 point2D = new Vector2(position1.x, position1.y);

        // print("position: " + position + ". distanceNew: " + distanceNew + ". from2D: " + from2D + ". point2D: " + point2D);
        bool result = false;
        //если расстояние между вектором объекта и цели меньше установленного
        if (from != null && Vector2.Distance(from2D, point2D) <= distanceNew)
        {
            //высчитываем скалярное произведение векторов, чтобы найти угол между объектом и его целью
            Vector2 direction = (point2D - from2D);

            result = true;
            float dot = Vector2.Dot(point.right * -1, direction.normalized);
            if (dot < 1)
            {
                float angleRadians = Mathf.Acos(dot);
                float angleDeg = angleRadians * Mathf.Rad2Deg;
                result = (angleDeg <= angleNew/2);
            }
            else
            {
                result = false;
            }
        }
        //print("result: " + result + ". Vector2.Distance: " + Vector2.Distance(from2D, point2D) + ". distanceNew: " + distanceNew);
        return result;
    }

    public void SetDistanceVisible(float newDistance)
    {
        distance = newDistance;
    }

    public float GetDistanceVisible()
    {
        return distance;
    }

    public void SetAngleVisible(float newAngle)
    {
        angle = Convert.ToInt32(newAngle);
    }

    public float GetAngleVisible()
    {
        return angle;
    }
}
