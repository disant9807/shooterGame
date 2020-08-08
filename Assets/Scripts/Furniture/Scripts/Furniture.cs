using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class Furniture : MonoBehaviour, IFurniture
{
    public Color colorSelected;
    public Color colorDamage;
    public float hp;
    public bool damaged = true;

    private Color colorDefault;
    private Rigidbody2D _rigidbody2d;
    private SpriteRenderer _spriteRenderer;
    private int id;
    private bool selectColor;

    private void Start()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        colorDefault = _spriteRenderer.color;
        id = GlobalID.GetID();
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

    public float GetHP ()
    {
        return hp;
    }

    public float Damage (float damage)
    {
        if (damaged)
        {
            StartCoroutine(AcyncColorDamage());
            hp -= damage;
        }
        return hp;
    }

    public void Jerk(Vector3 forward, float velocity)
    {
        _rigidbody2d.AddForce(forward * velocity, ForceMode2D.Impulse);
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
        return _rigidbody2d;
    }

    public bool Selected()
    {
        try
        {
            _spriteRenderer.color = colorSelected;
            selectColor = true;
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    private IEnumerator AcyncColorDamage()
    {
        _spriteRenderer.color = colorDamage;
        yield return new WaitForSeconds(0.2f);
        _spriteRenderer.color = colorDefault;
    }

    public bool NoneSelected()    
    {
        try
        {
            _spriteRenderer.color = colorDefault;
            selectColor = false;
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}
