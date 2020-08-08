using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ActionPlayer : MonoBehaviour
{
    public float radiusAction;
    public string furnitureTag;
    public string weaponTag;
    public GameObject[] weapons;
    public Slider slider;
    
    private CircleCollider2D circleCollider2D;
    private List<GameObject> furnitures;
    private GameObject selectedFurniture;
    private int selectedWeapon = -1;
    private GameObject spawnWeapon;
    
    private void Start()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        circleCollider2D.isTrigger = true;
        circleCollider2D.radius = radiusAction;
        furnitures = new List<GameObject>();
    }

    private void Update()
    {
        if (furnitures.Count > 0)
        {
            if (selectedFurniture != null)
            {
                int idSelectedFurniture = selectedFurniture.GetComponent<IFurniture>().GetID();
                foreach (GameObject item in furnitures)
                {
                    IFurniture _furniture = item.GetComponent<IFurniture>();
                    if (_furniture.GetID() != idSelectedFurniture)
                    {
                        _furniture.NoneSelected();
                    }
                    else
                    {
                        _furniture.Selected();
                    }
                }
            }

            selectedFurniture = furnitures
                .OrderBy(i => Vector2.Distance(transform.position, i.transform.position))
                .First();
        }

    }

    public GameObject GetSelectedFurniture()
    {
        return selectedFurniture;
    }

    private void SelectWeapon(int index)
    {
        spawnWeapon = Instantiate(weapons[index], gameObject.transform.parent);
        GunGrenadeLaunch launch = spawnWeapon.GetComponent<GunGrenadeLaunch>();
        if (launch != null)
        {
            launch.slider = slider;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(furnitureTag))
        {
            furnitures.Add(other.gameObject);
        }
        if (other.gameObject.CompareTag(weaponTag))
        {
            ItemGun item = other.gameObject.GetComponent<ItemGun>();
            SelectWeapon(item.weaponIndex);
            item.Kill();
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(furnitureTag))
        {
            int idFurniture = other.gameObject.GetComponent<IFurniture>().GetID();
            int findIndex = furnitures.Select((item, index) => new {Item = item, Index = index})
                .First(i =>
                {
                    int findID = i.Item.GetComponent<IFurniture>().GetID();
                    return findID == idFurniture;
                })
                .Index;
            furnitures[findIndex].GetComponent<IFurniture>().NoneSelected();
            furnitures.RemoveAt(findIndex);
            if (selectedFurniture != null && idFurniture == selectedFurniture.GetComponent<IFurniture>().GetID())
            {
                selectedFurniture = null;
            }
        }
    }
    
}
