using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDialog : MonoBehaviour
{
   public dialog dialog;
   public bool after = false;
   public GameObject floor;
   private void Start()
   {
      if (after)
      {
         floor.SetActive(false);
      }
   }
   private void Update()
   {
      if (dialog.GeIsCaneled() && !after)
      {
         Destroy(floor);
      }
      else if (dialog.GeIsCaneled() && after)
      {
         floor.SetActive(true);
      }
   }
}
