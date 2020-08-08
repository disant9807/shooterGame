using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {
	[SerializeField] GameObject Point;

	private Transform thisTransform;
	private Transform pointTransform;
	
	void Start ()
	{
		thisTransform = transform;
		pointTransform = Point.transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
		thisTransform.position = pointTransform.position;
	}
}
