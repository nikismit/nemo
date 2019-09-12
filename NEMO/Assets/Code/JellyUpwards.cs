using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyUpwards : MonoBehaviour
{
    [Header("Jelly Selection")]
    //public GameObject[] JellyClumps;
    public Transform[] Jellies;

    [Header("Values")]
    public float minRandomMovUp = .1f, maxRandomMovUp = .5f, minShiver = -.5f, maxShiver = .5f;

    public bool AutoUp = true;

    private bool GoUp;

    private float[] upFactor;
    private float[] shiverFactor;
    private Vector3[] startPos;

    private int Amount;

    void Start()
    {
        Jellies = GetComponentsInChildren<Transform>();

        Amount = Jellies.Length;

        upFactor = new float[Amount];
        shiverFactor = new float[Amount];
        startPos = new Vector3[Amount];

        for (int i = 0; i < Jellies.Length; i++)
        {
            CalculateUp();
            CalculateShiver();
            //startPos[i] = JellyClumps[i].transform.position;
            startPos[i] = Jellies[i].transform.position;
        }
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.J))
        // {
        //     if (!GoUp)
        //     {
        //         GoUp = true;
        //     }
        //     else if (GoUp)
        //     {
        //         GoUp = false;
        //     }
        // }

        // if (Input.GetKeyDown(KeyCode.R))
        // {
        //     Reset();
        // }

        if (GoUp || AutoUp)
        {
            JellyGoUp();
            JellyMoveHor();
        }
    }

    void JellyGoUp()
    {
        for (int i = 0; i < Jellies.Length; i++)
        {
            //JellyClumps[i].transform.position += Vector3.up * Time.deltaTime * upFactor[i];
            Jellies[i].transform.position += Vector3.up * Time.deltaTime * upFactor[i];
        }
    }

    void JellyMoveHor()
    {
        for (int i = 0; i < Jellies.Length; i++)
        {
            //JellyClumps[i].transform.position += Vector3.right * Time.deltaTime * shiverFactor[i];
            Jellies[i].transform.position += Vector3.right * Time.deltaTime * shiverFactor[i];
        }
    }

    // void Reset()
    // {
    //     for (int i = 0; i < Jellies.Length; i++)
    //     {
    //         //JellyClumps[i].transform.position = startPos[i];
    //         Jellies[i].transform.position = startPos[i];
    //     }
    //     CalculateUp();
    //     CalculateShiver();
    //     print("reset");
    // }

    void CalculateUp()
    {
        for (int i = 0; i < Jellies.Length; i++)
        {
            upFactor[i] = Random.Range(minRandomMovUp, maxRandomMovUp);
        }
        //print("calculated upwards momentum");
    }

    void CalculateShiver()
    {
        for (int i = 0; i < Jellies.Length; i++)
        {
            shiverFactor[i] = Random.Range(minShiver, maxShiver);
        }
        //print("calculated shiver momentum");
    }
}
