using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVisible
{
    bool IsVisible();
    
    GameObject GetObjVisible();

    float DistanceVisible();

    void SetDistanceVisible(float newDistance);

    float GetDistanceVisible();

    void SetAngleVisible(float newAngle);
    float GetAngleVisible();

}