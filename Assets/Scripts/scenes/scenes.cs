using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scenes : MonoBehaviour
{
    //public dialog dialog;
    public string tagPlayer;
    private Rigidbody2D rigidbody2D;
    public float strong;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
       //if (dialog.GeIsCaneled())
       // {
       //     rigidbody2D.velocity = transform.right * (strong * Time.deltaTime);
       // }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(tagPlayer))
        {
            other.gameObject.GetComponent<Player>().SetDamage(10000f);
        }
    }
}
