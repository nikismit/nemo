using CM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class NemoPlayer2 : MonoBehaviour
{

    //singleton stuff
    public static NemoPlayer2 _instance;

    public NemoController controller;
    public NEMO_GameEvents eventManager;

    //BAS edit below
    //Change the number of ints in readArray to change the number of frames used to calculate fullnessDifference
    public int[] readArray = new int[10];

    //fullnessDifference shows the difference in fullness over an amount of frames (ie time)
    //short breaths register as a low fullnessDifference, long breaths as a big fullnessDifference (this can be a negative or positive number)
    public float fullnessDifference;

    //fullnessDiffThreshold: How big does the fullness difference need to be to be counted as a deep enough breath to be counted
    public float fullnessDiffThreshold = 20f;


    private int counter = 0;
    private int rawFullness;
    private int oldFullness;
    public int fulnessDelta;

    public enum States { breathingIn, breathingOut }
    public States breathState = States.breathingIn;

    public float forwardSpeed = 1f;
    public float backSpeed = 2f;
    public float speedMultiplier = 1f;

    private float speed = 2f;

    public bool backwardOrUp;

    private float diraction = 0;

    public float smoother = 0.8f;

    // the main value to be used by the game
    // a float between 0 and 1;
    public float fullness;

    // this is to calculate fullness
    private float fullnessLow = -200f;
    private float fullnessHigh = 200f;
    public float fullnessCalibrationRate = 1f;
    private float range;

    public Vector3 startPosition;       // Start position of the player

    public GameEvent BreathingInEvent;
    public GameEvent BreathingOutEvent;

    private bool _canMove = false;

    //Bas edit
    //variables to control the white border fade in and out
    public GameObject whiteBorder;
    private Color whiteBorderColor;
    private float whiteBorderAlpha = 0;
    public float whiteBorderAlphaMax = 0.65f;
    private float whiteBorderAlphaMin = 0.0f;
    private float whiteBorderSmoothIn = 0.25f;
    private float whiteBorderSmoothOut = 2f;

    void Awake()
    {
        //singleton stuff
        if (NemoPlayer2._instance == null)
            NemoPlayer2._instance = this;
        else
            Destroy(gameObject);

        CM_Debug.AddCategory("NEMO Player");
    }

    // Use current transform as start position
    public void SetStartPosition()
    {
        CM_Debug.Log("NEMO Player", "Setting start position to " + transform.position);
        startPosition = transform.position;
    }

    // Reset the position from the player to the start position
    public void ResetPosition()
    {
        GoToPosition goToPosition = GetComponent<GoToPosition>();

        if (!goToPosition.IsMoving)
        {
            CM_Debug.Log("NEMO Player", "Moving to " + startPosition);
            goToPosition.Execute(startPosition);
            goToPosition.FinishedEvent += OnResetPositionFinished;
        }
    }

    // Reset the game after finishing moving to the start position
    private void OnResetPositionFinished()
    {
        eventManager.ResetGame();
        GetComponent<GoToPosition>().FinishedEvent -= OnResetPositionFinished;
    }

    // Update is called once per frame
    void Update()
    {
        // are they breathing in or out 
        rawFullness = controller.value;
        fulnessDelta = oldFullness - rawFullness;
        //print(fulnessDelta);
        readArray[counter] = fulnessDelta;
        counter++;

        if (counter >= readArray.Length)
            counter = 0;

        int total = 0;

        foreach (int i in readArray)
            total += i;

        if (total > 0)
        {
            if (breathState != States.breathingIn)
            {
                BreathingInEvent.Invoke();
                // breathing in maybe 
                breathState = States.breathingIn;
                //diraction = 1;

                //BAS edit below
                //BAS: Not sure what this does, commenting it out doesn't seem to affect the movement
                //speed = forwardSpeed * speedMultiplier;
            }
        }
        else
        {
            if (breathState != States.breathingOut)
            {
                BreathingOutEvent.Invoke();
                // breathing out maybe 
                breathState = States.breathingOut;
                //diraction = -1;
                speed = backSpeed * speedMultiplier;
            }
        }
        oldFullness = rawFullness;


        //BAS EDIT BELOW
        //Stop changes in position being caused by bad breathing

        //Only long, deep breaths change position
        //This is regulated by fullnessDiffThreshold
        //fullnessDifference checks how much difference there is in fullness over a set time
        //slow breaths have big difference (either negative of positive)
        //short breaths have small difference, and get ignored

        //check total fullness difference (which is a combination of breathing differences between N number of frames)
        // if too little change do nothing?
        fullnessDifference = 0;
        for (int i = 0; i < readArray.Length; i++)
        {
            fullnessDifference += readArray[i];
        }

        if (fullnessDifference > fullnessDiffThreshold || fullnessDifference < -fullnessDiffThreshold)
        {
            if (fulnessDelta != 0 && breathState == States.breathingIn)
            {

                //BAS edit below
                //Bas: When breathing in, movement smoothes to standstill (0). When breathing out it smoothes to backwards movement (-1)
                //diraction = Mathf.Lerp(diraction, 1, Time.deltaTime * smoother);
                diraction = Mathf.Lerp(diraction, 0.1f, Time.deltaTime * smoother);

                //BAS edit
                //Changes the opacity of white border based on fullness
                whiteBorderAlpha = Mathf.Lerp(whiteBorderAlpha, whiteBorderAlphaMax, Time.deltaTime * whiteBorderSmoothIn);
                whiteBorderColor = new Color(1, 1, 1, whiteBorderAlpha);
                whiteBorder.GetComponent<Image>().color = whiteBorderColor;

            }

            if (fulnessDelta != 0 && breathState == States.breathingOut)
            {
                diraction = Mathf.Lerp(diraction, -1, Time.deltaTime * smoother);

                //BAS edit
                //Changes the opacity of white border based on breathing
                whiteBorderAlpha = Mathf.Lerp(whiteBorderAlpha, whiteBorderAlphaMin, Time.deltaTime * whiteBorderSmoothOut);
                whiteBorderColor = new Color(1, 1, 1, whiteBorderAlpha);
                whiteBorder.GetComponent<Image>().color = whiteBorderColor;
            }

            if (fulnessDelta == 0)
            {
                diraction = Mathf.Lerp(diraction, 0, Time.deltaTime * smoother);

            }
        }
        // BAS EDIT
        //if change over time is too small (meaning breath not deep enough): ignore, and just smooth
        else
        {
            diraction = Mathf.Lerp(diraction, 0, Time.deltaTime * smoother);
        }

        // move the player
        if (_canMove)
        {
            if (backwardOrUp)
            {
                transform.Translate(Vector3.forward * speed * diraction * Time.deltaTime);

            }
            else
            {
                transform.Translate(Vector3.up * speed * diraction * Time.deltaTime);
            }
        }

        fullness = CalculateFullness();
    }

    float CalculateFullness()
    {



        //first calibrate the top and bottom range

        // tuck in the bottom range
        if (rawFullness < fullnessLow)
            fullnessLow = rawFullness;
        else
            fullnessLow += fullnessCalibrationRate * Time.deltaTime;

        // tuck in the top range
        if (rawFullness > fullnessHigh)
            fullnessHigh = rawFullness;
        else
            fullnessHigh -= fullnessCalibrationRate * Time.deltaTime;

        // whats the range?
        range = fullnessHigh - fullnessLow;


        return ((rawFullness - fullnessLow) / range);
    }

    public void SetMovable()
    {
        _canMove = true;
    }

    public void StopMove()
    {
        _canMove = false;
    }
}