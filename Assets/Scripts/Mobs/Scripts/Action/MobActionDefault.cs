using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobActionDefault : MonoBehaviour
{
    private IMob monsterBehavior; 
    private Transform target; // Выбранная цель
    private bool targetSelected; // Если цель выбранна
    private bool beingAttacked; // Атакуют ли меня ?
    private bool selectLayer;
    
    // Start is called before the first frame update
    void Start()
    {
        monsterBehavior = GetComponent<IMob>();
        targetSelected = false;
        beingAttacked = false;
        selectLayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetSelected == false && beingAttacked == false)
        {
            if (monsterBehavior.IsVisible())
            {
                target = monsterBehavior.GetObjVisible().transform;
                targetSelected = true;
            }
            else
            {
                monsterBehavior.Idle();
            }
        }
        else if (beingAttacked && targetSelected == false)
        {
            if (monsterBehavior.IsVisible())
            {
                target = monsterBehavior.GetObjVisible().transform;
                targetSelected = true;
            }
            monsterBehavior.Find();
        }
        else
        {
            if (target != null)
            {
                monsterBehavior.Attack();
                if (!selectLayer)
                {
                    gameObject.layer = 9;
                    selectLayer = true;
                }
            }
            else
            {
                targetSelected = false;
                beingAttacked = false;
            }
        }
    }
    
    
}
