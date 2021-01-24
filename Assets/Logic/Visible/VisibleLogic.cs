using System;
using UnityEngine;
using Assets.Core.Logic;
using Assets.Core.Common;

namespace Assets.Logic.Visible
{
    public class VisibleLogic: MonoBehaviour, IVisibleLogic
    {
        //public переменные
        public int angle; // Угол видимости

        public float distance; // Дистанция видимости

        public LayerMask mask; // Слой видимости. например будет ли видеть сквозь окно ?

        public string tagMask; // Tag который будет чекаться скриптом

        // peivate переменные
        private Transform transformCache;
        private LineRenderer lineCache;
        // Цель для поиска. Чувствительна к регистру.
        private GameObject visibleTarget;
        public float scaleRadius;

        void Start()
        {
            visibleTarget = GameObject.FindWithTag(tagMask);
            transformCache = transform;
            gameObject.AddComponent<LineRenderer>();
            lineCache = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            //Debug.Log("Рисование сектора");
            RenderLineHelper.DrawSector(angle, distance / scaleRadius, 0.1f, lineCache);
            RenderLineHelper.DrawSector(angle, distance - (distance * 0.08f), 0.1f, lineCache);
        }

        public bool IsVisible()
        {
            return IsVisibleObject(transformCache, visibleTarget, angle, distance, mask);
        }

        public float DistanceVisible()
        {
            return Vector2.Distance(transformCache.position, visibleTarget.transform.position);
        }

        public GameObject GetObjVisible()
        {
            return visibleTarget;
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
                    result = (angleDeg <= angleNew / 2);
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
}
