using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class disableOnButtonPress : MonoBehaviour
{
    public KeyCode input = KeyCode.C;
    public Image gameobject;

    private bool isActive;

    void Start()
    {
        isActive = true;
        gameobject = GetComponent<Image>();
        gameobject.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(input))
        {
            if (isActive)
            {
                gameobject.enabled = false;
                isActive = false;
            }
            else
            {
                gameobject.enabled = true;
                isActive = true;
            }
        }
    }
}