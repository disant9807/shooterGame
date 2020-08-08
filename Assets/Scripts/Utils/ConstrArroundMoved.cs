using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstrArroundMoved : MonoBehaviour
{
    //public переменные
    [SerializeField] public int countMobs; //Количество мобов
    [SerializeField] public float distanceTp; //Дистанция при которой происходит телепортация до цели
    [SerializeField] public float distance; // Дистанция при которой моб начинает бежать за целью
    [SerializeField] public GameObject mobs; // Моб для спавна
    
    //private переменные
     private Transform _transform;
     private Transform[] _mobs;

    void Start()
    {
        _transform = transform;
        Spawn();
    }

    
    void Spawn()
    {
        _mobs = new Transform[countMobs];
        
        for (var t = 0; t < countMobs; t++)
        {
            GameObject mob = Instantiate(mobs, _transform.position, _transform.rotation);
            mob.AddComponent<MovedFollowingObj>();
            
            MovedFollowingObj moved = mob.GetComponent<MovedFollowingObj>();
            moved.distance = distance;
            moved.maxDistance = distanceTp;
            moved.unit = _transform;

            _mobs[t] = mob.transform;
        }
    }
    
    
}
