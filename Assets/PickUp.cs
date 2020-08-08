using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    // public Gun
    // Start is called before the first frame update
    public GameObject item;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {

            Transform weapons = player.GetComponentsInChildren<Transform>().Last();
            for (int i = 0; i < weapons.transform.childCount; ++i)
            {
                Transform currentItem = weapons.transform.GetChild(i);
                if (currentItem.name.Equals(item.name + "(Clone)"))
                {
                    return;
                }
            }

            var transform1 = weapons.transform;
            GameObject weapon = Instantiate(
                item,
                transform1.position +
                (transform1.right * (this.GetComponent<BoxCollider2D>().bounds.size.x)),
                transform1.transform.rotation,
                transform1
            );
            if (weapons.transform.childCount > 1)
            {
                weapon.SetActive(false);
            }
            Destroy(gameObject);
        }
    }
}