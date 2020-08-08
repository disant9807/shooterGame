using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckKeys : MonoBehaviour
{
    public GameObject keysUI;

    public KeyCode key;
    // Start is called before the first frame update
    void Start()
    {
        keysUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            keysUI.SetActive(true);
        }

        if (Input.GetKeyUp(key))
        {
            keysUI.SetActive(false);
        }
    }
}
