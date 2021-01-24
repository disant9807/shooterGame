using UnityEngine;

namespace Assets.Core.Logic
{
    public interface IVisibleLogic
    {
        bool IsVisible();

        GameObject GetObjVisible();

        float DistanceVisible();

        void SetDistanceVisible(float newDistance);

        float GetDistanceVisible();

        void SetAngleVisible(float newAngle);
        float GetAngleVisible();

    }
}