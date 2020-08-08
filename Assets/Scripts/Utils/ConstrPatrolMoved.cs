using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ConstrPatrolMoved : MonoBehaviour
{
    //public переменные
    [SerializeField] int countMobs; // Количество мобов
    [SerializeField] GameObject mobSpawn; // Мобы для спавна
    [SerializeField] GameObject [] point; // Точки патрулирования
    [SerializeField] public float lineWidth;
    
    //private переменные
    private Transform _transform; // Кешируем трансформ
    private LineRenderer _line; //Кешируем лайн
    private Vector3[] _vectorPoints;

    private void Start()
    {
        gameObject.AddComponent<LineRenderer>();
        _line = GetComponent<LineRenderer>();
        _transform = transform;
        
        if (Application.IsPlaying(gameObject))
        {
            _vectorPoints = new Vector3[point.Length];
            for (var t = 0; t < _vectorPoints.Length; t++)
            {
                _vectorPoints[t] = point[t].transform.position;
            }
            Spawn();
        }
    }

    private void Update()
    {
        PaintLine();
    }


    void Spawn()
    {
        for (var i = 0; i < countMobs; i++)
        {
            GameObject mobs = Instantiate(mobSpawn, _transform.position, _transform.rotation, _transform);
            mobs.AddComponent<MovedPatrol>();
            MovedPatrol moved = mobs.GetComponent<MovedPatrol>();
            moved.pointMoved = _vectorPoints;
        }
    }
    
    void PaintLine()
    {
        _line.positionCount = point.Length;
        _line.startWidth = lineWidth;
        _line.endWidth = lineWidth;
        for (var a = 0; a < point.Length; a++)
        {
             _line.SetPosition(a, point[a].transform.position);
        }
    }
}
