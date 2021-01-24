using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Assets.Core.Action;


namespace Assets.Action.Cast
{
    public class CastAction : MonoBehaviour, ICastAction
    {
        [SerializeField] float fireAccuracy; // Кучность стрельбы
        [SerializeField] float strongBulletMax; // Сила выстрела Макс
        [SerializeField] private float strongBulletMin;
        [SerializeField] float stepChangeStrongBullet; // шаг изменения силы пули
        [SerializeField] private float timeStepChangeStrongBullet; // Время изменения силы пули
        [SerializeField] float shutterSpeed; //Скорость затвора
        [SerializeField] float timeDestruction; //Время уничтожения пули
        [SerializeField] GameObject bull; // пуля
        [SerializeField] private bool takePhysicsParent; // Учитывать физику родителя?
        public Slider slider;
        public KeyCode key;

		private bool permission;
		private float time;
		private Transform transformCache; // кешируем трансформ
		private Animator animatorCache;
		private Rigidbody2D rigibody2DCache;
		private float strongBullet;


		void Start()
		{
			permission = true;
			transformCache = transform;
			rigibody2DCache = transform.parent.gameObject.GetComponent<Rigidbody2D>();
			animatorCache = gameObject.GetComponent<Animator>();
			time = 0f;
			strongBullet = strongBulletMin;
		}

		public void AccumulationCast()
		{
			time -= Time.deltaTime;
			if (time <= 0f)
			{
				if (strongBullet < strongBulletMax)
				{
					strongBullet += stepChangeStrongBullet;
					slider.value = strongBullet / strongBulletMax;
				}
				time = timeStepChangeStrongBullet;
			}
		}

		public void Cast()
        {
			StartCoroutine(ShotsAsync());
        }

		private void ResetShot()
		{
			strongBullet = strongBulletMin;
		}

	

		public void Shot()
		{
			slider.value = 0;

			GameObject spawn = Instantiate(bull, transformCache.position, transformCache.rotation);
			float angle = Random.Range(-fireAccuracy, fireAccuracy);
			var right = transformCache.right;
			var x = right.x * Mathf.Cos(angle) - right.y * Mathf.Sin(angle);
			var y = right.y * Mathf.Cos(angle) - right.x * Mathf.Sin(angle);
			Vector2 rotateSpawn = new Vector2(x, y);

			spawn.GetComponent<Rigidbody2D>().velocity = takePhysicsParent ?
				strongBullet * rotateSpawn + rigibody2DCache.velocity :
				strongBullet * rotateSpawn;
			StartCoroutine(DestructionBullet(spawn));
			ResetShot();
			
		}

		private IEnumerator DestructionBullet(GameObject obj)
		{
			yield return new WaitForSeconds(timeDestruction);
			Destroy(obj);
		}

		private IEnumerator ShotsAsync()
		{
			Shot();
			permission = false;
			yield return new WaitForSeconds(shutterSpeed);
			permission = true;
		}
	}
}
