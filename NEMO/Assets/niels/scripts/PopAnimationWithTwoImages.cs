using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopAnimationWithTwoImages : MonoBehaviour
{
    private Transform[] AllImages;
    private bool In;
    public float FadeSpeed = .1f;
    public float TimeTillNext = 3f;
    private float tempTimer;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        AllImages = GetComponentsInChildren<Transform>();
        animator = GetComponentInChildren<Animator>();

        for (int i = 1; i < AllImages.Length; i++)
        {

        }
        tempTimer = TimeTillNext;
        animator.SetBool("FirstBop", true);
    }

    // Update is called once per frame
    void Update()
    {
        tempTimer -= Time.deltaTime;

        if (tempTimer < 0)
        {
            if (In)
            {
                In = false;
            }
            else
            {
                In = true;
            }
            tempTimer = TimeTillNext;
        }

        if (In)
        {
            ShowFirst();
        }
        else
        {
            ShowSecond();
        }
    }

    void ShowFirst()
    {
        AllImages[1].gameObject.SetActive(true);
        AllImages[2].gameObject.SetActive(false);
    }

    void ShowSecond()
    {
        AllImages[1].gameObject.SetActive(false);
        AllImages[2].gameObject.SetActive(true);
    }

    public void ResetValues()
    {
        if (AllImages != null)
        {
            for (int i = 0; i < AllImages.Length; i++)
            {

            }
            In = true;
            tempTimer = TimeTillNext;
        }
    }
}
