using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MovePlayer : MonoBehaviour {
	[SerializeField] float speed;
	[SerializeField] GameObject cell;
	[SerializeField] float strong;
	[SerializeField] string tagFurniture;
	public bool freeze;
	private Rigidbody2D _rigidbody2D;

	private Rigidbody2D _rigidbody2DFurniture;
	private bool isEditFurniture;
	private GameObject gameObjectFurniture;
	private Vector3 posFurniture;
	private ActionPlayer actionPlayer;

	private void Start()
	{
		_rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
		actionPlayer = gameObject.transform.GetComponentInChildren<ActionPlayer>();
	}

	// Update is called once per frame
	void Update () 
	{
		if (!freeze)
		{
			if (!isEditFurniture)
			{
				//положение мыши из экранных в мировые координаты
				Vector3 mousePosition = cell.transform.position;
				var angle = Vector2.Angle(Vector2.right,
					mousePosition - transform.position); //угол между вектором от объекта к мыше и осью х
				transform.eulerAngles =
					new Vector3(0f, 0f,
						transform.position.y < mousePosition.y ? angle : -angle); //немного магии на последок

				Move(_rigidbody2D, speed, false);
			}
			else
			{
				Debug.Log(posFurniture);
				transform.localPosition = posFurniture;
				Move(_rigidbody2DFurniture, strong, true);
			}

			if (Input.GetKeyDown(KeyCode.Q))
			{
				if (!isEditFurniture)
				{
					gameObjectFurniture = actionPlayer.GetSelectedFurniture();
					if (gameObjectFurniture != null)
					{
						_rigidbody2DFurniture = gameObjectFurniture.GetComponent<Rigidbody2D>();
						isEditFurniture = true;
						transform.parent = gameObjectFurniture.transform;
						posFurniture = transform.localPosition;
						_rigidbody2D.simulated = false;

						var angle = Vector2.Angle(Vector2.right,
							gameObjectFurniture.transform.position - transform.position);
						transform.eulerAngles = new Vector3(0f, 0f,
							transform.position.y < gameObjectFurniture.transform.position.y ? angle : -angle);
					}
				}
				else
				{
					isEditFurniture = false;
					transform.parent = null;
					_rigidbody2D.simulated = true;
					gameObjectFurniture = null;
					_rigidbody2DFurniture = null;
					//astarPath.Scan();
				}
			}
		}
	}

	void Move(Rigidbody2D moveRigibody, float veloSpeed, bool rotate)
	{
		float speed2 = veloSpeed;
		if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D) ||
		    (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)))
		{
			speed2 = speed2 * 71 / 100;
		}
		else if ((Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)) ||
		         (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)))
		{
			speed2 = speed2 * 71 / 100;
		}
		
		if (Input.GetKey(KeyCode.W))
		{
			moveRigibody.velocity += Vector2.up * (speed2 * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.A))
		{
			moveRigibody.velocity += Vector2.left * (speed2 * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.D))
		{
			moveRigibody.velocity += Vector2.right * (speed2 * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.S))
		{
			moveRigibody.velocity += Vector2.down * (speed2 * Time.deltaTime);
		}

		if (rotate)
		{
			if (Input.GetKey(KeyCode.RightArrow))
			{
				moveRigibody.rotation += veloSpeed * 10 * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.LeftArrow))
			{
				moveRigibody.rotation += veloSpeed * 10 * Time.deltaTime * -1;
			}
		}
	}
	

}

    

	

