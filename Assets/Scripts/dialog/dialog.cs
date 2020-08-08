using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class dialog : MonoBehaviour
{
    public string tag; // Тег объекта на который реагирует диалог
    public bool repeat; // Повторять ли диалог ?
    public bool automatic;
    public KeyCode key; // Кнопка активации
    public KeyCode keyPass;
    public RPGTalk talk;
    public MovePlayer player;
    public bool freeze;
    
   // public GameObject spriteButton; // Кнопка, которая отображается при приблежении чувака
    public GameObject canvas; // плитка разговора
    public GameObject canvasText;
    public GameObject canvasMission;
    public dialog previous;
    public dialog following;
    // Start is called before the first frame update
    private bool permission = true;
    private bool visible = false;
    private bool isCaneled = false;
    void Start()
    {
        if (canvasMission != null)
        {
            canvasMission.SetActive(true);
        }

        if (talk != null)
        {
            talk.OnEndTalk += CanelTalk;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (visible && permission)
        {
            if (Input.GetKey(key) || automatic)
            {
                permission = false;
                if (canvasText != null)
                    canvasText.SetActive(false);
                if (canvas != null)
                {
                    canvas.SetActive(true);
                }

                if (talk != null)
                {
                    talk.NewTalk();
                    if (freeze)
                    {
                        player.freeze = true;
                    }
                }
            }
        }
        if (Input.GetKey(keyPass))
        {
            if (talk.isPlaying)
            {
                talk.EndTalk();
            }
        }

        if (previous != null && permission && canvasMission != null)
        {
            if (!previous.isCaneled)
            {
                canvasMission.SetActive(false);
            }
            else
            {
                canvasMission.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(tag))
        {
            if (permission)
            {
                if ((previous != null && previous.isCaneled) || previous == null)
                {
                    if ((following != null && !following.isCaneled) || following == null)
                    {
                        visible = true;
                        if (canvasText != null) 
                            canvasText.SetActive(true);
                    }
                }
            }
        }
    }

    private void CanelTalk()
    {
        if (freeze)
        {
            player.freeze = false;
        }
        if (repeat)
        {
            permission = true;
            visible = false;
        }

        if (canvasMission != null)
        {
            canvasMission.SetActive(false);
        }

        if (canvas != null)
        {
            canvas.SetActive(false);
        }

        isCaneled = true;
    }

    public bool GeIsCaneled()
    {
        return isCaneled;
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(tag))
        {
            visible = false;
            if (canvasText != null)
                canvasText.SetActive(false);
        }

        if (talk == null)
        {
            CanelTalk();
        }
    }
}
