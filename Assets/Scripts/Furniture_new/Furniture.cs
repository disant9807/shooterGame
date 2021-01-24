using UnityEngine;
using Assets.Core.Common;
using Assets.Action.Spriter;
using System.Collections;
using System;

public class Furniture: MonoBehaviour, IFurniture
{
    public Color colorSelected;
    public Color colorDamage;
    public float hp;
    public bool damaged = true;

    private Color colorDefault;
    private Rigidbody2D rigibody2DCache;
    private ISpriterAction spriterAction;
    private int id;

    private void Start()
    {
        rigibody2DCache = GetComponent<Rigidbody2D>();
        id = GlobalIDHelper.GetID();
        spriterAction = GetComponent<ISpriterAction>();
    }

    private void Update()
    {
        if (hp <= 0)
            this.Destroy();
    }

    public void SetHP(float _hp)
    {
        hp = _hp;
    }

    public float GetHP()
    {
        return hp;
    }

    public float Damage(float damage)
    {
        if (damaged)
        {
            spriterAction.TemporaryColorChange(colorDamage, 1f);
            hp -= damage;
        }
        return hp;
    }

    public void Jerk(Vector3 forward, float velocity)
    {
        rigibody2DCache.AddForce(forward * velocity, ForceMode2D.Impulse);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public int GetID()
    {
        return id;
    }

    public Rigidbody2D GetRigiBody()
    {
        return rigibody2DCache;
    }

    public bool Selected()
    {
        spriterAction.ColorChange(colorSelected);
        return true;
    }


    public bool NoneSelected()
    {
        spriterAction.ColorChange(colorDefault);
        return true;
    }
}

