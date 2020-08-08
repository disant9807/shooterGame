using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class GunGrenadeLaunch : MonoBehaviour, IGun {
	//public переменные
	[FormerlySerializedAs("cuchnost")] [SerializeField] float fireAccuracy; // Кучность стрельбы
	[SerializeField] float strongBulletMax; // Сила выстрела Макс
	[SerializeField] private float strongBulletMin;
	[SerializeField] float stepChangeStrongBullet; // шаг изменения силы пули
	[SerializeField] private float timeStepChangeStrongBullet; // Время изменения силы пули
	[FormerlySerializedAs("speedAction")] [SerializeField] float shutterSpeed; //Скорость затвора
	[FormerlySerializedAs("timeDestroy")] [SerializeField] float timeDestruction; //Время уничтожения пули
	[FormerlySerializedAs("avtomat")] [SerializeField] bool automatic; // Автоматическая стрелбьа или пистолетная
	[SerializeField] GameObject bull; // пуля
	[SerializeField] int countAmmo; // Количество патронов в магазине
	[SerializeField] int countMagazine; // Количество магазинов
	[SerializeField] bool ammoEndless; // Бесконечные ли патроны ?
	[SerializeField] string weaponName;
	[SerializeField] private bool takePhysicsParent; // Учитывать физику родителя?
	public Slider slider;
	public KeyCode key;
	
	//private переменные
	private bool _permission;
	private float _triggerPull;
	private float _time;
	private Transform _transform; // кешируем трансформ
	private int _ammo;
	private int _magazine;
	private Animator animator;
	private Rigidbody2D _rigidbody2D;
	private float _strongBullet;


	void Start () {
 		_permission = true;
        _transform = transform;       
        _ammo = countAmmo;
        _magazine = countMagazine;
        _rigidbody2D = transform.parent.gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        _time = 0f;
        _strongBullet = strongBulletMin;
	}
	
	void Update () {
		if (Input.GetKey(key))
		{
			PreparingShoot();
		}
		if (Input.GetKeyUp(key))
		{
			Fire();
		}
	}

	private void PreparingShoot()
	{
		_time -= Time.deltaTime;
		if (_time <= 0f)
		{
			if (_strongBullet < strongBulletMax)
			{
				_strongBullet += stepChangeStrongBullet;
				slider.value = _strongBullet / strongBulletMax;
			}
			_time = timeStepChangeStrongBullet;
		}
	}

	private void ResetShot()
	{
		_strongBullet = strongBulletMin;
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
		slider.value = 0;
		if (_ammo * _magazine > 0 || ammoEndless)
		{
			FlightPatron(1);
			GameObject spawn = Instantiate(bull, _transform.position, _transform.rotation);
			float angle = Random.Range(-fireAccuracy, fireAccuracy);
			var right = _transform.right;
			var x = right.x * Mathf.Cos(angle) - right.y * Mathf.Sin(angle);
			var y = right.y * Mathf.Cos(angle) - right.x * Mathf.Sin(angle);
			Vector2 rotateSpawn = new Vector2(x, y);
			
			spawn.GetComponent<Rigidbody2D>().velocity = takePhysicsParent ? 
				_strongBullet * rotateSpawn + _rigidbody2D.velocity : 
				_strongBullet * rotateSpawn;
			StartCoroutine(DestructionBullet(spawn));
			ResetShot();
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
