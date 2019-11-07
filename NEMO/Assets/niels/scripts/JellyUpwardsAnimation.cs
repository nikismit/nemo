using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyUpwardsAnimation : MonoBehaviour
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
    private Animator[] animator;

    private int Amount;

    void Start()
    {
        Jellies = GetComponentsInChildren<Transform>();

        Amount = Jellies.Length;

        upFactor = new float[Amount];
        shiverFactor = new float[Amount];
        rotationFactor = new float[Amount];
        startPos = new Vector3[Amount];
        animator = new Animator[Amount];

        for (int i = 1; i < Jellies.Length; i++)
        {
            Calculate(i);
            startPos[i] = Jellies[i].transform.position;
            animator[i] = Jellies[i].GetComponent<Animator>();
            JellyMove(i);
        }
    }

    void JellyMove(int i)
    {
        if (rotationFactor[i] > 0)
        {
            animator[i].SetBool("rotate", true);
        }
        else
        {
            animator[i].SetBool("rotate", false);
        }

        if (shiverFactor[i] > 0)
        {
            animator[i].SetBool("shiver", true);
        }
        else
        {
            animator[i].SetBool("shiver", false);
        }
        if (upFactor[i] < .15f)
        {
            animator[i].SetInteger("up", 0);
        }
        else if (upFactor[i] < .175f)
        {
            animator[i].SetInteger("up", 1);
        }
        else
        {
            animator[i].SetInteger("up", 2);
        }
    }

    void Calculate(int i)
    {
        upFactor[i] = Random.Range(minRandomMovUp, maxRandomMovUp);
        shiverFactor[i] = Random.Range(minShiver, maxShiver);
        rotationFactor[i] = Random.Range(-1f, 1f);
    }
}
