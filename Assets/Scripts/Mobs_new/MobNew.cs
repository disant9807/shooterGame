using UnityEngine;
using Assets.Core.Action;
using Assets.Core.Common;
using System.Collections;

public class MobNew: MonoBehaviour, IMobNew
{
    [SerializeField]
    public float hp;

    private IFindAction findAction;
    private IMovedAction movedAction;
    private Rigidbody2D rigidbody2DCache;
    private Collider2D collider2DCache;

    private bool isAttack;
    private bool isSeeTarget;
    private bool isFreeze;

    public void Start()
    {
        findAction = GetComponent<IFindAction>();
        movedAction = GetComponent<IMovedAction>();
        rigidbody2DCache = GetComponent<Rigidbody2D>();
        collider2DCache = GetComponent<Collider2D>();

        isFreeze = false;
        isAttack = false;
        isSeeTarget = false;
    }

    public void Update()
    {
        if (hp < 1)
        {
            Kill();
        }
        if (!isFreeze)
        {
            if (!isAttack && !isSeeTarget)
            {
                movedAction.RandomMove();
                isSeeTarget = findAction.CheckTarget();
            }
            else if (isAttack && !isSeeTarget)
            {
                findAction.Find();
            }
            else if (isSeeTarget)
            {
                movedAction.AttackMove();
            }
        }
    }

    public void Damage(float damageValue)
    {
        hp -= damageValue;
    }

    public void Kill()
    {
        hp = 0;
        Destroy(gameObject);
    }

    public void Freeze(float time)
    {
        StartCoroutine(FreezeAsync(time));
    }

    private IEnumerator FreezeAsync(float time)
    {
        isFreeze = true;
        yield return new WaitForSeconds(time);
        isFreeze = false;
    }

    public void UnFreeze()
    {
        isFreeze = false;
    }

    public void Jerk(Vector3 forward, float velocity)
    {
        rigidbody2DCache.AddForce(forward * velocity, ForceMode2D.Impulse);
    }

    public int GetID()
    {
        return GlobalIDHelper.GetID();
    }

    public void OffCollision(float time)
    {
        StartCoroutine(OffCollisionAsync(time));
    }

    private IEnumerator OffCollisionAsync(float time)
    {
        collider2DCache.isTrigger = true;
        yield return new WaitForSeconds(time);
        collider2DCache.isTrigger = false;
    }

    public void OnCollision()
    {
        collider2DCache.isTrigger = false;
    }
}

