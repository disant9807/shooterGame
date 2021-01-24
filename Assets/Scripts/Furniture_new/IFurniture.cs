using UnityEngine;

public interface IFurniture
{
    void Jerk(Vector3 forward, float velocity);

    Rigidbody2D GetRigiBody();

    int GetID();

    bool Selected();
    bool NoneSelected();

    void SetHP(float hp);
    float GetHP();
    float Damage(float damage);
    void Destroy();
}

