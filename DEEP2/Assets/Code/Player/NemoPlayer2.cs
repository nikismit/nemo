﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemoPlayer2 : MonoBehaviour
{

    public NemoController controller;

    public int[] readArray = new int[10];
    private int counter = 0;
    private int fullness;
    private int oldFullness;
    public int fulnessDelta;

    public enum States { breathingIn, breathingOut}
    public States breathState = States.breathingIn;

    public float forwardSpeed = 1f;
    public float backSpeed = 2f;

    private float speed = 2f;

    public bool backwardOrUp;

    private float diraction = 1;

    public float smoother = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // are they breathing in or out 
        fullness = controller.value;
        fulnessDelta = oldFullness - fullness;
        print(fulnessDelta);
        readArray[counter] = fulnessDelta;
        counter++;

        if (counter >= readArray.Length)
            counter = 0; 

        int total = 0;

        foreach (int i in readArray)
            total += i;

        if( total > 0 )
        {
            // breathing in maybe 
            breathState = States.breathingIn;
            //diraction = 1;
          
            speed = forwardSpeed;
        }
        else
        {
            // breathing out maybe 
            breathState = States.breathingOut;
            //diraction = -1;
            speed = backSpeed;
        }
        oldFullness = fullness;

        if (fulnessDelta != 0 && breathState == States.breathingIn)
        {
            diraction = Mathf.Lerp(diraction,1, Time.deltaTime * smoother);
        }

        if (fulnessDelta != 0 && breathState == States.breathingOut)
        {
            diraction = Mathf.Lerp(diraction, -1, Time.deltaTime * smoother);
        }

        if (fulnessDelta == 0 )
        {
            diraction = Mathf.Lerp(diraction, 0, Time.deltaTime * smoother);
        }
        // move the player 
        if (backwardOrUp)
        {
            
             transform.Translate(Vector3.forward * speed * diraction * Time.deltaTime);
            
        }
        else
        {
            transform.Translate(Vector3.up * speed * diraction * Time.deltaTime);
        }
       
        
    }
}
