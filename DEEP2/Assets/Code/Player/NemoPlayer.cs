using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemoPlayer : MonoBehaviour
{

    public NemoController controller;

    public int[] readArray = new int[10];
    private int counter = 0;
    private int fullness;
    private int oldFullness;
    private int fulnessDelta;

    public enum States { breathingIn, breathingOut}
    public States breathState = States.breathingIn;

    public float forwardSpeed = 1f;
    public float backSpeed = 2f;

    private float speed = 2f;
    
    

    private int diraction = 1;

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
            diraction = 1;
            speed = forwardSpeed;
        }
        else
        {
            // breathing out maybe 
            breathState = States.breathingOut;
            diraction = -1;
            speed = backSpeed;
        }
        oldFullness = fullness;

        // move the player 
        transform.Translate(Vector3.forward * speed * diraction * Time.deltaTime);

    }
}
