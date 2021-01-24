using System;
using UnityEngine;
using Assets.Core.Action;

public class GunNews: MonoBehaviour, IGunNews
{
	[SerializeField] int countAmmo; // Количество патронов в магазине
	[SerializeField] int countMagazine; // Количество магазинов
	[SerializeField] bool ammoEndless; // Бесконечные ли патроны ?
	[SerializeField] string weaponName;

	//private переменные
	private int ammo;
	private int magazine;
	private IFireAction fireAction;


	void Start()
	{
		ammo = countAmmo;
		magazine = countMagazine;
		fireAction = GetComponent<IFireAction>();
	}

	void Update()
	{
		if (Input.GetKey(KeyCode.Mouse0))
		{
			Fire();
		}
	}

	public int GetAmmo()
	{
		return ammo;
	}

	public int GetMagazine()
	{
		return magazine;
	}

	public bool SetAmmo(int ammo, int magazine)
	{
		try
		{
			this.ammo = ammo;
			this.magazine = magazine;
			return true;
		}
		catch (Exception e)
		{
			return false;
		}
	}

	private bool UpdateMagazine()
	{
		if (magazine > -1)
		{
			ammo += countAmmo;
			magazine--;
			return true;
		}

		return false;
	}

	private void FlightPatron(int count)
	{
		ammo -= count;
		while (ammo <= 0)
		{
			bool result = UpdateMagazine();
			if (result == false)
			{
				ammo = 0;
				break;
			}
		}
	}

	public void Fire()
	{
		if (ammo * magazine > 0 || ammoEndless)
		{
			FlightPatron(1);
			fireAction.Fire();
		}
	}
}

