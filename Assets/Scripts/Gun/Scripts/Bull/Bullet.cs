using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class Bullet : MonoBehaviour, IBullet
{
    //public перменные
    [FormerlySerializedAs("valueDamage")] public float damage; // урон пули
    public string tagMobs; // Тег моба
    public string tagFloor;
    public float powerGarbage;

    private Vector3 _velocity;

    private void Start()
    {
        _velocity = GetComponent<Rigidbody2D>().velocity;
    }

    public float GetDamage()
    {
        return damage;
    }

    public float SetDamage(float _damage)
    {
        damage = _damage;
        return damage;
    }

    public void Damage()
    {
        // Пуля наносит урон только при соприкосновении
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject objTrigger = other.gameObject;
        if (objTrigger.CompareTag(tagMobs))
        {
            IMob mobs = objTrigger.GetComponent<IMob>();
            mobs.SetDamage(damage);
            mobs.Jerk(_velocity, powerGarbage);
            mobs.SetFreeze(0.5f);
            mobs.NoCollision(1.5f);
            Destroy(gameObject);            
        }

        if (objTrigger.CompareTag(tagFloor))
        {
            Destroy(gameObject);
        }
    }
    
}
