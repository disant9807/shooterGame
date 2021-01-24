using UnityEngine;

public class BulletNew : MonoBehaviour, IBulletNew 
{
    public float damage; // урон пули
    public string tagMobs; // Тег моба
    public string tagFloor;
    public float powerGarbage;

    private Vector3 velocityCache;

    private void Start()
    {
        velocityCache = GetComponent<Rigidbody2D>().velocity;
    }

    public float GetDamage()
    {
        return damage;
    }

    public float SetDamage(float _damage)
    {
        damage = _damage;
        return damage;
    }

    public void Damage()
    {
        // Пуля наносит урон только при соприкосновении
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject objTrigger = other.gameObject;
        if (objTrigger.CompareTag(tagMobs))
        {
            IMobNew mobs = objTrigger.GetComponent<IMobNew>();
            mobs.Damage(damage);
            mobs.Jerk(velocityCache, powerGarbage);
            mobs.Freeze(0.5f);
            mobs.OffCollision(1.5f);
            Destroy(gameObject);
        }

        if (objTrigger.CompareTag(tagFloor))
        {
            Destroy(gameObject);
        }
    }
}

