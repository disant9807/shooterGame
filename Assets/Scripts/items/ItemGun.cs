using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGun : MonoBehaviour
{
  public int weaponIndex;

  public int GetWeapon()
  {
    return weaponIndex;
  }

  public void Kill()
  {
    Destroy(gameObject);
  }
}
