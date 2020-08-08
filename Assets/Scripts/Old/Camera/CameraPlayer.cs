using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer : MonoBehaviour {

float _sizes;

bool _isVisible;
[SerializeField] Transform cell;
[SerializeField] Transform player;
[SerializeField] float distN;
float _distance;
[SerializeField] float speed;
Vector3 _cellPos;
Vector3 _playerPos;
[SerializeField] float strong;

	// Use this for initialization
	void Start ()
	{
		if (Camera.main != null) _sizes = Camera.main.fieldOfView;
	}
	
	// Update is called once per frame
	void Update () {
		_distance = Vector3.Distance(cell.position,player.position);
		if(_distance > distN)
		{
			Debug.Log("Plus");
			if (Camera.main != null)
				Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, _sizes * strong, speed);
		}
		else
		{
			Debug.Log("Minus");
			if (Camera.main != null)
				Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, _sizes, speed);
		}

		var transform1 = transform;
		var position = player.transform.position;
		transform1.position = new Vector3(position.x, position.y, transform1.position.z);
	}


}
