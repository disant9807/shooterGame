using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IPlayer
{
   //public переменные
   public float hp;
   private float _maxHP;
   
   public void Start()
   {
     // _spriteRenderer = GetComponent<SpriteRenderer>();
     // colorDefault = _spriteRenderer.color;
      _maxHP = hp;
     // healthBar = RectTransform.FindObjectOfType<RectTransform>();
      //hpText.text = "HP: " + hp;
   }  
   
   public void SetDamage(float damage)
   {
      if (hp > 0)
      {
         hp -= damage;
      }
   }

   public void Freeze(bool type)
   {
      //freezy = type;
   }
   

   public void NextWeapon()
   {
      for (int i = 0; i < this.transform.childCount; ++i)
      {
         Transform currentItem = this.transform.GetChild(i);
         if (currentItem.gameObject.active)
         {
            currentItem.gameObject.SetActive(false);
            if (i < this.transform.childCount-1)
            {
               this.transform.GetChild(i + 1).gameObject.SetActive(true);
               break;
            }
            else
            {
               this.transform.GetChild(0).gameObject.SetActive(true);
               break;
            }
         }
      }
   }

   private void Update()
   {
      if (hp <= 0)
      {
         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
      }
   }
}
