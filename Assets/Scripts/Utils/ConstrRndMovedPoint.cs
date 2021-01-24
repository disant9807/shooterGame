using System;
using UnityEngine;
using Assets.Core.Common;

[ExecuteAlways]
public class ConstrRndMovedPoint : MonoBehaviour
{
    [SerializeField] public GameObject mobs; // Моб для спавна
    [SerializeField] public int count; // Количество мобов для спавна
    
    [SerializeField] public GameObject onePoint;
    [SerializeField] public GameObject twoPoint;
    [SerializeField] public GameObject threePoint;
    [SerializeField] public GameObject foPoint;
    

    private Transform _transform;

    private LineRenderer _lineRender;
    private Transform[] _transformList;
    
    void Start()
    {
        _transformList = new[]
        {
            onePoint.transform,
            twoPoint.transform,
            threePoint.transform,
            foPoint.transform,
            onePoint.transform,
        };

        _lineRender = gameObject.AddComponent<LineRenderer>();
        _lineRender = gameObject.GetComponent<LineRenderer>();

        if (Application.IsPlaying(gameObject))
        {
            _transform = transform;

            for (var a = 0; a < count; a++)
            {
                GameObject mob = Instantiate(mobs, _transform.position, _transform.rotation);

                MovedRandom randomComponent = mob.GetComponent<MovedRandom>();

                if (randomComponent != null)
                {
                    // randomComponent.onePoint = onePoint;
                    // randomComponent.twoPoint = twoPoint;
                    // randomComponent.threePoint = threePoint;
                    // randomComponent.foPoint = foPoint;
                }
                else
                {
                    Debug.Log("Ошибка. У моба должен быть компонент MovedRandom");
                    return;
                }
            }
        }
    }

    void Update()
    {
        if (Application.IsPlaying(gameObject) == false)
        {
            Vector3[] thisVectorPoints = new Vector3 [ _transformList.Length ];
            
            for (var t = 0; t < thisVectorPoints.Length; t++)
            {
                thisVectorPoints[t] = _transformList[t].position;
            }
            
            
            RenderLineHelper.DrawLinePoints(thisVectorPoints, 0.15f, _lineRender, true);
        }
    }
}
