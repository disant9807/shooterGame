using System;
using UnityEngine;
using Assets.Core.Action;

public class Throws : MonoBehaviour, IThrows
{
	[SerializeField] int countAmmo; // Количество патронов в магазине
	[SerializeField] int countMagazine; // Количество магазинов
	[SerializeField] bool ammoEndless; // Бесконечные ли патроны ?
	[SerializeField] string weaponName;

	//private переменные
	private int ammo;
	private int magazine;
	private ICastAction castAction;


	void Start()
	{
		ammo = countAmmo;
		magazine = countMagazine;
		castAction = GetComponent<ICastAction>();
	}

	void Update()
	{
		if (Input.GetKey(KeyCode.Mouse0))
		{
			castAction.AccumulationCast();
		}
		if (Input.GetKeyUp(KeyCode.Mouse0))
		{
			Cast();
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

	public void Cast()
	{
		if (ammo * magazine > 0 || ammoEndless)
		{
			FlightPatron(1);
			castAction.Cast();
		}
	}
}

