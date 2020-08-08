using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class Gun : MonoBehaviour, IGun {
	//public переменные
	[FormerlySerializedAs("cuchnost")] [SerializeField] float fireAccuracy; // Кучность стрельбы
	[FormerlySerializedAs("speedBull")] [SerializeField] float speedBullet; // Скорость пули
	[FormerlySerializedAs("speedAction")] [SerializeField] float shutterSpeed; //Скорость затвора
	[FormerlySerializedAs("timeDestroy")] [SerializeField] float timeDestruction; //Время уничтожения пули
	[FormerlySerializedAs("avtomat")] [SerializeField] bool automatic; // Автоматическая стрелбьа или пистолетная
	[SerializeField] GameObject bull; // пуля
	[SerializeField] int countAmmo; // Количество патронов в магазине
	[SerializeField] int countMagazine; // Количество магазинов
	[SerializeField] bool ammoEndless; // Бесконечные ли патроны ?
	[SerializeField] string weaponName;
	[SerializeField] private bool takePhysicsParent; // Учитывать физику родителя?

	//private переменные
	private bool _permission;
	private float _triggerPull;
	private float _time;
	private Transform _transform; // кешируем трансформ
	private int _ammo;
	private int _magazine;
	private Animator animator;
	private Rigidbody2D _rigidbody2D;


    void Start () {
 		_permission = true;
        _transform = transform;       
        _ammo = countAmmo;
        _magazine = countMagazine;
        _rigidbody2D = transform.parent.gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
    }
	
	void Update () {
		if (Input.GetKey(KeyCode.Mouse0))
		{
			if (automatic && _permission)
			{
				StartCoroutine(ShutterRelease());
			} 
			else if (!automatic)
			{
				Fire();
			}
		}
	}

	public int GetAmmo()
	{
		return _ammo;
	}

	public int GetMagazine()
	{
		return _magazine;
	}

	public bool SetAmmo(int ammo, int magazine)
	{
		try
		{
			_ammo = ammo;
			_magazine = magazine;
			return true;
		}
		catch (Exception e)
		{
			return false;
		}
	}

	private bool UpdateMagazine()
	{
		if (_magazine > -1)
		{
			_ammo += countAmmo;
			_magazine-- ;
			return true;
		}

		return false;
	}
	
	private void FlightPatron ( int count)
	{
		_ammo -= count;
		while (_ammo <= 0)
		{
			bool result = UpdateMagazine();
			if (result == false)
			{
				_ammo = 0;
				break;
			}
		}
	}

	public void Fire ()
	{
		if (_ammo * _magazine > 0 || ammoEndless)
		{
			animator.Play("fire");
			FlightPatron(1);
			GameObject spawn = Instantiate(bull, _transform.position, _transform.rotation);
			float angle = Random.Range(-fireAccuracy, fireAccuracy);
			var right = _transform.right;
			var x = right.x * Mathf.Cos(angle) - right.y * Mathf.Sin(angle);
			var y = right.y * Mathf.Cos(angle) - right.x * Mathf.Sin(angle);
			Vector2 rotateSpawn = new Vector2(x, y);
			
			spawn.GetComponent<Rigidbody2D>().velocity = takePhysicsParent ? 
				speedBullet * rotateSpawn + _rigidbody2D.velocity : 
				speedBullet * rotateSpawn;
			StartCoroutine(DestructionBullet(spawn));
		}
	}
	
	private IEnumerator DestructionBullet (GameObject obj)
	{
		yield return new WaitForSeconds(timeDestruction);
			Destroy(obj);
	}

	private IEnumerator ShutterRelease ()
	{
		Fire();
		_permission = false;
		 yield return new WaitForSeconds(shutterSpeed);
		_permission = true;
	}
}
