using UnityEngine;
using Assets.Core.Action;
using Assets.Core.Logic;
using System.Collections;

namespace Assets.Action
{
    public class FireAction : MonoBehaviour, IFireAction
    {

		[SerializeField] float fireAccuracy; // Кучность стрельбы
		[SerializeField] float speedBullet; // Скорость пули
		[SerializeField] float shutterSpeed; //Скорость затвора
		[SerializeField] float timeDestruction; //Время уничтожения пули
		[SerializeField] GameObject bull; // пуля
		[SerializeField] private bool takePhysicsParent; // Учитывать физику родителя?

		//private переменные
		private bool permission;
		private Transform transformCache; // кешируем трансформ
		private Animator animatorCache;
		private Rigidbody2D rigibody2DCache;

        public void Start()
        {
			permission = true;
			transformCache = transform;
			rigibody2DCache = GetComponent<Rigidbody2D>();
			animatorCache = gameObject.GetComponent<Animator>();
		}

        public void Fire()
        {
			StartCoroutine(ShotsAsync());
		}

		private IEnumerator ShotsAsync()
		{
			Shot();
			permission = false;
			yield return new WaitForSeconds(shutterSpeed);
			permission = true;
		}

		private void Shot()
		{
			animatorCache.Play("fire");
			GameObject spawn = Instantiate(bull, transformCache.position, transformCache.rotation);
			float angle = Random.Range(-fireAccuracy, fireAccuracy);
			var right = transformCache.right;
			var x = right.x * Mathf.Cos(angle) - right.y * Mathf.Sin(angle);
			var y = right.y * Mathf.Cos(angle) - right.x * Mathf.Sin(angle);
			Vector2 rotateSpawn = new Vector2(x, y);

			spawn.GetComponent<Rigidbody2D>().velocity = takePhysicsParent ?
				speedBullet * rotateSpawn + rigibody2DCache.velocity :
				speedBullet * rotateSpawn;
			StartCoroutine(DestructionBullet(spawn));
		}

		private IEnumerator DestructionBullet(GameObject obj)
		{
			yield return new WaitForSeconds(timeDestruction);
			Destroy(obj);
		}

	}
}
