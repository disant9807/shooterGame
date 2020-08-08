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
   public Text hpText;
   public Color colorDamage;
   private RectTransform healthBar; 
   private Color colorDefault;
   private SpriteRenderer _spriteRenderer;
   
   public void Start()
   {
      _spriteRenderer = GetComponent<SpriteRenderer>();
      colorDefault = _spriteRenderer.color;
      _maxHP = hp;
      healthBar = RectTransform.FindObjectOfType<RectTransform>();
      hpText.text = "HP: " + hp;
   }  
   
   public void SetDamage(float damage)
   {
      if (hp > 0)
      {
         StartCoroutine(AcyncColorDamage());
         hp -= damage;
         hpText.text = "HP: " + hp;
      }
   }

   public void Freeze(bool type)
   {
      //freezy = type;
   }
   
   private IEnumerator AcyncColorDamage()
   {
      _spriteRenderer.color = colorDamage;
      yield return new WaitForSeconds(0.2f);
      _spriteRenderer.color = colorDefault;
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
         hpText.text = "Погиб...";
         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
      }
   }
}
