using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Android;

public class PhysicsActionDefault : MonoBehaviour
{
    public string tagsFurniture;
    public float damage;
    public float frequency;
    public float strongJerk;

    private List<GameObject> furnitures;
    private bool permission;

    // Start is called before the first frame update
    void Start()
    {
        permission = true;
        furnitures = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (permission)
            StartCoroutine(AcyncDamage());
    }

    private IEnumerator AcyncDamage()
    {
        permission = false;
        if (furnitures.Count > 0)
        {
            foreach (var furniture in furnitures)
            {
                Vector3 vectorFurniture = furniture.transform.position;
                Vector3 thisVector = transform.position;
                var forward = vectorFurniture - thisVector;
                float distance = Vector2.Distance(vectorFurniture, thisVector);
                IFurniture furnitureComponent = furniture.GetComponent<IFurniture>();
                furnitureComponent.Damage(damage);
                furnitureComponent.Jerk(forward.normalized, strongJerk);
            }
        }

        yield return new WaitForSeconds(frequency);
        permission = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(tagsFurniture))
        {
            furnitures.Add(other.gameObject);
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        try
        {
            if (other.gameObject.CompareTag(tagsFurniture))
            {
                int idFurniture = other.gameObject.GetComponent<IFurniture>().GetID();
                var findIndex = furnitures.Select((item, index) => new {Item = item, Index = index})
                    .First(i =>
                    {
                        IFurniture furni = i.Item.GetComponent<IFurniture>();
                        return furni.GetID() == idFurniture;
                    });
                if (findIndex != null)
                {
                    furnitures.RemoveAt(findIndex.Index);
                }
            }
        }
        catch(Exception e)
        {
            
        }
    }
}
