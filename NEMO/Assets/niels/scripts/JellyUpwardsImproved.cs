using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyUpwardsImproved : MonoBehaviour
{
    [Header("Jelly Selection")]
    public Transform[] Jellies;

    [Header("Values")]
    public float minRandomMovUp = .1f, maxRandomMovUp = .2f, minShiver = -.1f, maxShiver = .1f, rotationSpeed = .5f;

    public bool AutoUp = true;

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
            Calculate(i);
            startPos[i] = Jellies[i].transform.position;
        }
    }

    void FixedUpdate()
    {
        if (AutoUp)
        {
            JellyMove();
        }
    }

    void JellyMove()
    {
        for (int i = 1; i < Jellies.Length; i++)
        {
            Jellies[i].transform.position += Vector3.up * Time.deltaTime * upFactor[i];
            Jellies[i].transform.position += Vector3.right * Time.deltaTime * shiverFactor[i];
            Jellies[i].transform.Rotate(new Vector3(0, 0, rotationFactor[i]) * rotationSpeed);
        }
    }

    void Calculate(int i)
    {
        upFactor[i] = Random.Range(minRandomMovUp, maxRandomMovUp);
        shiverFactor[i] = Random.Range(minShiver, maxShiver);
        rotationFactor[i] = Random.Range(-1f, 1f);
    }
}
