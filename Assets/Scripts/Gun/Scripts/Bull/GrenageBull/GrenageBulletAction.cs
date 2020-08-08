using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenageBulletAction : MonoBehaviour, IBullet
{
    private GrenageBullet bullet;
    private float timeToDamage;

    private void Start()
    {
        bullet = gameObject.transform.GetChild(0).GetComponent<GrenageBullet>();
        if (bullet)
        {
            timeToDamage = bullet.GetTimeDestroy();
        }
        else
        {
            timeToDamage = 0f;
        }
    }

    private void Update()
    {
        timeToDamage -= Time.deltaTime;
        if (timeToDamage <= 0.0f)
        {
            if (bullet && bullet.ReadyDestory())
            {
                Destroy(gameObject);
            }
        }
    }

    public void Damage()
    {
        if (bullet)
        {
            bullet.Damage();
        }
    }

    public float SetDamage(float damage)
    {
        if (bullet)
        {
            return bullet.SetDamage(damage);
        }

        return 0f;
    }

    public float GetDamage()
    {
        if (bullet)
        {
            return bullet.GetDamage();
        }

        return 0;
    }
}
