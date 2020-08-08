using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFind
{
    void Find(); // Просто поиск

    void FindTarget(Vector3 target, float range); //Поиск около области target - область, возле которой будет идти поиск, range - как далеко будет идти поиск возле точки

    void FindForward(Vector3 target, float range); // Поиск по направлению

    void EndFind(); //конец поиска

}
