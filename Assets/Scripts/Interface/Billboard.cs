using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Quaternion _rotation;
    private Vector3  _position;
    
    void Start()
    {
        _rotation = transform.rotation;
    }
    void Update()
    { 
        transform.rotation = _rotation;
    }
}
