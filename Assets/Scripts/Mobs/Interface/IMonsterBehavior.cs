using UnityEngine;

public interface IMonsterBehavior
{
    void Attack();

    void Death();
    
    bool IsVisible();
    
    GameObject GetObjVisible();
    
    float DistanceVisible();
    void Find();
    bool SetDamage(float damage);

    void Follow(Transform target);
    void Idle();
    
    void Jerk (Vector3 forward, float velocity);
    
    int GetID();

    void SetFreeze(float time);

    void NoCollision(float time);
}