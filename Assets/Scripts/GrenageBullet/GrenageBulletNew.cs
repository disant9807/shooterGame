using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;


public class GrenageBulletNew : MonoBehaviour
{
    public float timeToDamage;
    [FormerlySerializedAs("valueDamage")] public float damage;
    public float powerGarbage;
    public float powerGarbageFurniture;
    public float weakeningGarbage;
    public float weakeningGarbageFurniture;
    public float radiusDamage;
    public float radiusDamageFurniture;
    public string tagsFurniture;
    public string tagsMobs;

    private List<GameObject> furnitures;
    private List<GameObject> mobs;

    private CircleCollider2D _circleCollider2D;
    private bool isDamage;
    private bool permission;
    private bool readyDestroy;

    private void Start()
    {
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _circleCollider2D.isTrigger = true;
        _circleCollider2D.radius = radiusDamage;
        furnitures = new List<GameObject>();
        mobs = new List<GameObject>();
        permission = true;
        readyDestroy = false;
    }

    private void Update()
    {
        timeToDamage -= Time.deltaTime;
        if (timeToDamage <= 0.0f)
        {
            Damage();
        }

        if (isDamage)
        {
            if (permission)
            {
                StartCoroutine(AcyncDamage());
            }
        }
    }

    public float GetDamage()
    {
        return damage;
    }

    public float SetDamage(float _damage)
    {
        this.damage = _damage;
        return this.damage;
    }

    public void Damage()
    {
        isDamage = true;
    }

    public float GetTimeDestroy()
    {
        return timeToDamage;
    }

    public bool ReadyDestory()
    {
        return readyDestroy;
    }

    private IEnumerator AcyncDamage()
    {
        permission = false;
        Debug.Log(furnitures.Count);
        if (furnitures.Count > 0)
        {
            foreach (var furniture in furnitures)
            {
                Vector3 vectorFurniture = furniture.transform.position;
                Vector3 thisVector = transform.position;
                var forward = vectorFurniture - thisVector;
                float distance = Vector2.Distance(vectorFurniture, thisVector);
                IFurniture furnitureComponent = furniture.GetComponent<IFurniture>();
                furnitureComponent.Jerk(forward.normalized, powerGarbageFurniture - (distance * weakeningGarbageFurniture));
            }
        }

        if (mobs.Count > 0)
        {
            foreach (var mob in mobs)
            {
                Vector3 vectorFurniture = mob.transform.position;
                Vector3 thisVector = transform.position;
                var forward = vectorFurniture - thisVector;
                float distance = Vector2.Distance(vectorFurniture, thisVector);
                IMob mobComponent = mob.GetComponent<IMob>();
                mobComponent.SetFreeze(1.5f);
                mobComponent.NoCollision(1.5f);
                mobComponent.SetDamage(damage);
                mobComponent.Jerk(forward.normalized, powerGarbage - (distance * weakeningGarbage));
            }

        }
        yield return new WaitForSeconds(0f);
        readyDestroy = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if (other.gameObject.CompareTag(tagsFurniture))
        {
            Debug.Log("Furniture Add");
            furnitures.Add(other.gameObject);
        }

        if (other.gameObject.CompareTag(tagsMobs))
        {
            mobs.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(tagsFurniture))
        {
            int idFurniture = other.gameObject.GetComponent<IFurniture>().GetID();
            int findIndex = furnitures.Select((item, index) => new { Item = item, Index = index })
                .First(i =>
                {
                    IFurniture furni = i.Item.GetComponent<IFurniture>();
                    return furni.GetID() == idFurniture;
                })
                .Index;
            furnitures.RemoveAt(findIndex);
        }

        if (other.gameObject.CompareTag(tagsMobs))
        {
            int idmobs = other.gameObject.GetComponent<IMob>().GetID();
            int findIndex = mobs.Select((item, index) => new { Item = item, Index = index })
                .First(i =>
                {
                    IMob mobs = i.Item.GetComponent<IMob>();
                    if (mobs != null)
                    {
                        return mobs.GetID() == idmobs;
                    }
                    return false;

                })
                .Index;
            mobs.RemoveAt(findIndex);
        }
    }
}

