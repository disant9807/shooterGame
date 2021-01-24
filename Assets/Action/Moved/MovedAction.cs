using UnityEngine;
using Assets.Core.Action;
using Assets.Core.Logic;
using Assets.Core.Common.MobOrganisation;

namespace Assets.Action.Moved
{
    public class MovedAction: MonoBehaviour, IMovedAction
    {
        // public переменные
        [SerializeField]
        public float minDelay;// Минимальная дистанция перемещения до поворота

        [SerializeField]
        public float maxDelay; // Максимальная дистанция перемещения до поворота

        [SerializeField]
        public Vector3[] pointsRestriction;

        [SerializeField]
        public Transform pointFollow;

        [SerializeField]
        public float distanceFollow;

        [SerializeField]
        public Transform target;

        [SerializeField]
        public string tagWall;

        // private переменные
        private bool isMoved = false; // Указывает, двигается ли монстр ?
        private bool isRotate = false; // Указывает повернулся ли монстр
        private Vector2 lastPosition; // запоминает последнее положение монстра
        private float distanceToTurn = 0; // дистанция, которую пройдет мостр до поворота
        private float angle = 0; // градус угла, на который повернется монстр
        private Transform transformCache; // Кешируем компонент трансформ
        private IMotorLogic motor;
        private bool isReturn = false; // Возвращается ли моб назад в область ?

        void Start ()
        {
            transformCache = transform;
            //MobsOrganisationSystem.createdMob(transform);
            motor = GetComponent<IMotorLogic>();
            angle = Random.Range(-180f, 180f);
        }

        public void EscapeMove()
        {
            //null
        }

        public void AttackMove()
        {
            motor.GoToTarget(target.position);
        }

        public void RandomMove()
        {
            switch (isMoved)
            {
                case false when isRotate == false:
                    //RotateState();
                    break;
                case false when isRotate == true:
                    distanceToTurn = Random.Range(minDelay, maxDelay);
                    lastPosition = transformCache.position;
                    isMoved = true;
                    break;
                case true when isRotate == true:
                    {
                        if (Vector2.Distance(lastPosition, transformCache.position) < distanceToTurn)
                        {
                            var t = 1;
                        }
                        else
                        {
                            isMoved = false;
                            isRotate = false;
                            angle = Random.Range(-180f, 180f);
                        }

                        break;
                    }
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == tagWall)
            {
                var targetAngle = collision.gameObject.transform.position - transform.position;
                var angle = Vector2.Angle(transform.forward, targetAngle);
                Debug.Log("Test");
                Debug.Log(angle);

                isMoved = false;
                isRotate = false;
                //RotateState(180 - angle , 180 - angle);
            }
        }
    }
}
