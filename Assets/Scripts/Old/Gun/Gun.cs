using System.Collections;
using UnityEngine;



public class GunOld : MonoBehaviour {
	[SerializeField] float speedBull; // скорость полета пули
	[SerializeField] float speedAction; // скорость затвора
	[SerializeField] float timeDestroy; // время до уничтожения
	[SerializeField] float accuracy; // точность
	[SerializeField] bool automatic; // автоматический режим стрельбы или одиночный
	[SerializeField] Vector3 aim; // прицел
	[SerializeField] GameObject bullet; // пуля

	private bool permission; // права доступа на стрельбу
	private KeyCode shotButton; // кнопка выстрела
	private Quaternion gunMuzzle;

	void Start () {
 		permission = true;
		shotButton = KeyCode.Mouse0;
		gunMuzzle = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(shotButton))
		{
			if (automatic)
            {
				if (permission)
				{
					StartCoroutine(ShotAutomatic());
				}
            }
			else
            {
				Fire();
			}
		}
	}
	private void Fire () // выстрел
	{
		GameObject spawn = Instantiate(bullet, transform.position, transform.rotation);
		spawn.transform.LookAt(aim);
		spawn.GetComponent<Rigidbody2D>().velocity = spawn.transform.forward * speedBull;
		spawn.transform.rotation = new Quaternion(gunMuzzle.x, gunMuzzle.y, gunMuzzle.z += Random.Range(-accuracy, accuracy), gunMuzzle.w);
		StartCoroutine(ShotВestruction(spawn));

	}
	private IEnumerator ShotВestruction (GameObject obj)
	{
		yield return new WaitForSeconds(timeDestroy);
		Destroy(obj);
	}

	private IEnumerator ShotAutomatic ()
	{
		permission = false;
		Fire();
		yield return new WaitForSeconds(speedAction);
		permission = true;
	}
}
