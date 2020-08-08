using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMotor
{
    void GoToVector(Vector3 vector);

    void GoToForward(Vector3 forward);

    void GoToTarget();

    void Rotate(Vector3 point);
    

    void RotateAngle(float angle, bool forward);

    void RotateFlow(Vector3 point);

    void RotateFlowAngle(float angle, bool forwardRight);
    
    void SetSpeed(float newSpeed);
    
    float GetSpeed();

    void TpToPoint(Vector3 point);
}
