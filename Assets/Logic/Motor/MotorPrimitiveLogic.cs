using System;
using UnityEngine;
using Assets.Core.Logic;

namespace Assets.Logic.Motor
{
    public class MotorPrimitiveLogic : MonoBehaviour, IMotorLogic
    {
        //public переменные
        public float speed { get; set; } //Скорость передвижения

        //private переменные
        private Transform transformCache; // Кешируем трансформ объекта
        private Vector3 rotateVector;
        private bool rotate;

        public void Start()
        {
            transformCache = transform;
            rotate = true;
        }

        public void GoToTarget(Vector3 point)
        {
            throw new NotImplementedException();
        }

        public void GoToVector(Vector3 vector)
        {
            var position = transformCache.position;
            Vector3 direction = (vector - position);
            position += direction.normalized * (speed * Time.deltaTime);
            transformCache.position = position;
        }

        public void GoToForward(Vector3 forward)
        {
            transformCache.position += forward * (speed * Time.deltaTime);
        }

        public void Rotate(Vector3 point)
        {
            var position = transformCache.position;
            float rotateDegree = Vector2.Angle(Vector2.right, point - position);
            transformCache.eulerAngles = new Vector3(0f, 0f, position.y < point.y ? rotateDegree : -rotateDegree);
        }

        public void RotateForward(Vector3 forward)
        {
            throw new NotImplementedException();
        }

        public void RotateAngle(float angle, bool forward)
        {
            transformCache.eulerAngles = new Vector3(0f, 0f, forward == true ? angle : -angle);
        }

        public void RotateFlow(Vector3 point)
        {
            float rotateDegree = Vector2.Angle(Vector2.right, point - transformCache.position);

            if (rotateDegree > 1)
            {
                var rotation = transformCache.rotation;
                transformCache.eulerAngles = new Vector3(0f, 0f, transformCache.position.y < point.y ? rotation.z + speed * Time.deltaTime : rotation.z + speed * Time.deltaTime * (-1));
            }
        }

        public void RotateFlowAngle(float angle, bool forwardRight)
        {
            if (rotate)
            {
                rotateVector = transformCache.right;
                rotate = false;
            }
            else if (rotate == false)
            {
                float thisAngle = Vector2.Angle(transformCache.right, rotateVector);

                if (thisAngle < angle)
                {
                    var eulerAngles = transformCache.eulerAngles;
                    eulerAngles = new Vector3(0f, 0f,
                        forwardRight == true
                            ? eulerAngles.z + speed * Time.deltaTime
                            : eulerAngles.z + speed * Time.deltaTime * (-1));
                    transformCache.eulerAngles = eulerAngles;
                }
                else
                {
                    rotate = true;
                }
            }
        }

        public void TpToPoint(Vector3 point)
        {
            transformCache.position = point;
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
}
