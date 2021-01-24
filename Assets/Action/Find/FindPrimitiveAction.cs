using UnityEngine;
using Assets.Core.Action;
using Assets.Core.Logic;

namespace Assets.Action.Find
{
    public class FindPrimitiveAction : MonoBehaviour, IFindAction
    {
        [SerializeField]
        public float distanceFind; // Дистанция для поиска

        private bool isFind; // Идет ли поиск ?
        private float rangePoint; // Расстояние между отрезками поиска
        private Transform transformCache; // Кешируем трансформ

        private float lastDistance; //Дистанция видимости до поиска
        private float lastAngle; //Последний угол видимости

        private Vector3 lastRotation;
        private Vector3 rotate;
        private IMotorLogic motor;
        private IVisibleLogic visible;
        private bool isGoToLine = true;
        private bool isGoToRotate = true;
        
        public void Start()
        {
            motor = gameObject.GetComponent<IMotorLogic>();
            visible = gameObject.GetComponent<IVisibleLogic>();
            lastDistance = visible.GetDistanceVisible();
            lastAngle = visible.GetAngleVisible();
        }

        public void Find()
        {
            visible.SetDistanceVisible(distanceFind);
            visible.SetAngleVisible(30f);
            //motor.RotateFlowAngle(359f, true);

        }

        public void EndFind()
        {
            visible.SetDistanceVisible(lastDistance);
            visible.SetAngleVisible(lastAngle);
        }

        public bool CheckTarget()
        {
            return visible.IsVisible();
        }

        public void FindForward(Vector3 forward, float range)
        {
            throw new System.NotImplementedException();
        }

        public void FindTarget(Vector3 target, float range)
        {
            visible.SetDistanceVisible(distanceFind);
            visible.SetAngleVisible(30f);

            //motor.Rotate(target);
        }
    }
}
