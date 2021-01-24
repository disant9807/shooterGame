using UnityEngine;

public interface IMobNew
{
    void Damage(float damage);

    void Kill();

    void Freeze(float seconds);

    void UnFreeze();

    void Jerk(Vector3 forward, float velocity);

    int GetID();

    void OffCollision(float seconds);

    void OnCollision();
}

