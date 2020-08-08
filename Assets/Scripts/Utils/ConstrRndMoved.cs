using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstrRndMoved : MonoBehaviour
{
    // public переменыые
    [SerializeField] int countMobs; // Количество мобов
    //[SerializeField] float distance; // Зона действия
   // [SerializeField] float speed; // Скорость перемещения
    [SerializeField] GameObject mobSpawn; // Мобы для спавна
    [SerializeField] GameObject player; // Мобы для спавна
    public dialog dialogSpawn;
    public dialog dialogDestroy;
    
    // private переменные
    private GameObject[] _unitCreated; // Кешируем создаваемых монстров
    private LineRenderer _lineRenderer;
    private bool spawn;
    
    void Start()
    {
        _unitCreated = new GameObject[countMobs]; 
        spawn = true;
        mobSpawn.GetComponent<BehaviorDefault>().target = player;
        if (!dialogSpawn)
        {
            Spawn();
        }
    }

    private void Update()
    {
        if (spawn)
        {
            if (dialogSpawn && dialogSpawn.GeIsCaneled())
            {
                Spawn();
            }
        }
        else if (!spawn)
        {
            if (!dialogDestroy)
            {
                Destroy(gameObject);
            }
            else if (dialogDestroy && dialogDestroy.GeIsCaneled())
            {
                DestroyAll();
                Destroy(gameObject);
            }
        }
    }
    
    
    
    private void Spawn()
    {
        for (var t = 0; t < countMobs; t++)
        {
            _unitCreated[t] = Instantiate(mobSpawn, transform.position, transform.rotation);
            //unit[t].GetComponent<BehaviorDefault>().SetMotor();
        }
        spawn = false;
    }

    private void DestroyAll()
    {
        for (var t = 0; t < countMobs; t++)
        {
            Destroy(_unitCreated[t]);
        }
    }
}
