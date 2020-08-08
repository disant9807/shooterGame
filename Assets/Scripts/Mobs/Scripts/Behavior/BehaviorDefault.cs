using System;
using System.Collections;
using Pathfinding;
using UnityEngine;

public class BehaviorDefault : MonoBehaviour, IMonsterBehavior
{
    [SerializeField] public float hp; //Жизненные силы
    [SerializeField] public float damage; //Урон наносимый мобом
    [SerializeField] public float distanceDamage; // Дистанция атаки
    [SerializeField] public float attackFrequency; //Скорость атаки
    [SerializeField] public GameObject target; //Игрок
    [SerializeField] public LayerMask layerMask; //Слои видимости
    [SerializeField] public int angle; // Угол видимости
    [SerializeField] public float distance; // Дистанция видимости
    [SerializeField] public float distanceFind; // Дистанция поиска
    [SerializeField] public string tagMask; // Тег игрока
    [SerializeField] public float speed; //Скорость моба
    [SerializeField] public float speedRotate; // Скорость поворота моба
    [SerializeField] public float timeFind; // Время поиска
    [SerializeField] public float minDelay; // Минимальный угол поворота моба при рандомном перемещении
    [SerializeField] public float maxDelay; // Максимальный угол поворота моба при рандомном перемещении
    [SerializeField] public float distanceFollowing; // Дистанция при которой моб начинает следовать за мобом
    [SerializeField] public float distanceTeleport; // Дистанция при которой моб начинает телепортироваться за следуемым мобом
    [SerializeField] public string tagWallet;
    [SerializeField] public string tagFurniture;
    [SerializeField] public Transform point = null;
    [SerializeField] public float distancePoint = 0f;
    
    private Transform transformCache;
    private LineRenderer lineRenderer;
    private Rigidbody2D rigidbody2DCache;
    
    private VisibleTypical visibleTypical; // Класс отвечающий за видимость
    private FindAngry findAngry; // Класс отвечающий за поиск
    private MotorNow motorNow; // Класс отвечающий за мотор (передвижвения объектов)
    private MovedRandom movedRandom; // Класс отвечающий на рандомное передвижение
    private MovedFollowingObj movedFollowingObj; //Класс отвечающий за следование за объектом
    private Seeker seeker;

    private bool attack;
    private int id;
    private IPlayer player;
    void Start()
    {
        attack = true;
        
        transformCache = gameObject.transform;
        lineRenderer = GetComponent<LineRenderer>();
        rigidbody2DCache = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        id = GlobalID.GetID();
        
        //Надо будет сделать статичным
        motorNow = new MotorNow(transformCache, rigidbody2DCache, speed, speedRotate, target.transform, seeker);
        visibleTypical = new VisibleTypical(transformCache, lineRenderer, target, layerMask, angle, distance, tagMask);
        findAngry = new FindAngry(transformCache, motorNow, visibleTypical, distanceFind );
        movedRandom = new MovedRandom(transformCache, new Vector3[4], motorNow, minDelay, maxDelay, point, distancePoint);
        //movedFollowingObj = new MovedFollowingObj(transformCache, motorNow, distanceFollowing, distanceTeleport);

        player = target.GetComponent<IPlayer>();
        
    }
    
    void Update()
    {
        if (hp <= 0)
        {
            Death();
        }
    }

    public void SetMotor()
    {
        motorNow = new MotorNow(transformCache, rigidbody2DCache, speed, speedRotate, target.transform, seeker);
    }

    public void Attack()
    {
        if (DistanceVisible() < distanceDamage)
        {
            StartCoroutine(Attack());
        }
        else
        {
            motorNow.GoToTarget();
        }

        IEnumerator Attack()
        {
            if (attack)
            {
                player?.SetDamage(damage);
                attack = false;
                yield return new WaitForSeconds(attackFrequency);
                attack = true;
            }
        }
        
    }

    public void Death()
    {
        try
        {
            
            StartCoroutine(KillWait());

            IEnumerator KillWait()
            {
                SetFreeze(1f);
                yield return new WaitForSeconds(1f);
                Destroy(gameObject);
            }
        }
        catch (Exception e)
        {
            Destroy(gameObject);
        }
    }

    public bool IsVisible()
    {
        try
        {
            return visibleTypical.IsVisible();
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return false;
        }
    }

    public GameObject GetObjVisible()
    {
        try
        {
            return visibleTypical.GetObjVisible();
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return null;
        }
    }

    public float DistanceVisible()
    {
        try
        {
            return visibleTypical.DistanceVisible();
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return 100;
        }
    }

    public void Find()
    {
        try
        {
            findAngry.Find();
            StartCoroutine(EndFind());

            IEnumerator EndFind()
            {
                yield return new WaitForSeconds(timeFind);
                findAngry.EndFind();
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public bool SetDamage(float newDamage)
    {
        try
        {
            hp -= newDamage;
            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return false;
        }
    }

    public void Follow(Transform target)
    {
        try
        {
            movedFollowingObj.unit = target.transform;
            movedFollowingObj.Moved();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void Idle()
    {
        movedRandom.Moved();
    }
    
    public void Jerk(Vector3 forward, float velocity)
    {
        rigidbody2DCache.AddForce(forward * velocity, ForceMode2D.Impulse);
    }

    public int GetID()
    {
        return this.id;
    }

    public void SetFreeze(float time)
    {
        motorNow.SetFreeze(true);
        StartCoroutine(UnsetFreeze(time));
    }

    private IEnumerator UnsetFreeze(float time)
    {
        yield return new WaitForSeconds(time);
        motorNow.SetFreeze(false);
    }
    
    public void NoCollision(float time)
    {
        gameObject.layer = 12;
        StartCoroutine(UnsetCollision(time));
    }

    private IEnumerator UnsetCollision(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.layer = 9;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(tagWallet) || other.gameObject.CompareTag(tagFurniture))
        {
            movedRandom.HitWall();
        }
    }
}
