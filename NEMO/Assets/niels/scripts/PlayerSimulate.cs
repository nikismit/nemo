using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSimulate : MonoBehaviour
{
    public NemoController nemoController;

    public bool simulatePlayer;
    public bool test;
    private float timer = 0;
    public float timerMaxIn = 5;
    public float timerMaxOut = 5;
    private bool toggle = false;
    public bool slowIncrease;
    private float t;
    public float smoothTime = 3;
    private bool a;

    // Update is called once per frame
    void Update()
    {
        if (simulatePlayer)
        {
            nemoController.arduinoTest = true;

            timer += Time.deltaTime;
            if (timer > timerMaxIn && !a)
            {
                a = true;
                timer = 0;
                if (toggle)
                {
                    toggle = false;
                }
                else
                {
                    toggle = true;
                }
            }
            if (timer > timerMaxOut && a)
            {
                a = false;
                timer = 0;
                if (toggle)
                {
                    toggle = false;
                }
                else
                {
                    toggle = true;
                }
            }

            if (toggle && !slowIncrease)
            {
                nemoController.value++;
            }
            else if (!slowIncrease)
            {
                nemoController.value--;
            }


            else if (toggle && slowIncrease)
            {
                In();
            }
            else if (!toggle && slowIncrease)
            {
                Out();
            }
        }
        else
        {
            timer = 0;
        }

        if (test)
        {
            nemoController.arduinoTest = true;
        }
    }

    private void In()
    {
        t += Time.deltaTime;
        if (t > .1)
        {
            t = 0;
            nemoController.value++;
        }
    }

    private void Out()
    {
        t += Time.deltaTime;
        if (t > .1)
        {
            t = 0;
            nemoController.value--;
        }
    }
}
