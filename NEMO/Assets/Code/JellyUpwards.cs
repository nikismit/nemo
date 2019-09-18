using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyUpwards : MonoBehaviour
{
    [Header("Jelly Selection")]
    public Transform[] Jellies;

    [Header("Values")]
    public float minRandomMovUp = .1f, maxRandomMovUp = .2f, minShiver = -.1f, maxShiver = .1f, rotationSpeed = .5f;

    public bool AutoUp = true;

    private bool GoUp;

    private float[] upFactor;
    private float[] shiverFactor;
    private float[] rotationFactor;
    private Vector3[] startPos;

    private int Amount;

    void Start()
    {
        Jellies = GetComponentsInChildren<Transform>();

        Amount = Jellies.Length;

        upFactor = new float[Amount];
        shiverFactor = new float[Amount];
        rotationFactor = new float[Amount];
        startPos = new Vector3[Amount];

        for (int i = 0; i < Jellies.Length; i++)
        {
            CalculateUp();
            CalculateShiver();
            CalculateRotation();
            startPos[i] = Jellies[i].transform.position;
        }
    }

    void Update()
    {
        if (GoUp || AutoUp)
        {
            JellyGoUp();
            JellyMoveHor();
            JellyRotate();
        }
    }

    void JellyGoUp()
    {
        for (int i = 1; i < Jellies.Length; i++)
        {
            Jellies[i].transform.position += Vector3.up * Time.deltaTime * upFactor[i];
        }
    }

    void JellyMoveHor()
    {
        for (int i = 1; i < Jellies.Length; i++)
        {
            Jellies[i].transform.position += Vector3.right * Time.deltaTime * shiverFactor[i];
        }
    }

    void JellyRotate()
    {
        for (int i = 1; i < Jellies.Length; i++)
        {
            Jellies[i].transform.Rotate(new Vector3(0, 0, rotationFactor[i]) * rotationSpeed);
        }
    }

    void CalculateUp()
    {
        for (int i = 1; i < Jellies.Length; i++)
        {
            upFactor[i] = Random.Range(minRandomMovUp, maxRandomMovUp);
        }
    }

    void CalculateShiver()
    {
        for (int i = 1; i < Jellies.Length; i++)
        {
            shiverFactor[i] = Random.Range(minShiver, maxShiver);
        }
    }

    void CalculateRotation()
    {
        for (int i = 1; i < Jellies.Length; i++)
        {
            rotationFactor[i] = Random.Range(-1f, 1f);
        }
    }
}
