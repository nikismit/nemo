using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeBetweenTwoImages : MonoBehaviour
{
    private Image[] AllImages;
    private Color AlphaUp = new Color(1, 1, 1, 1), AlphaZero = new Color(1, 1, 1, 0);
    private bool In;
    public float FadeSpeed = .1f;
    public bool autoFade = true;
    public float TimeTillNext = 3f;
    private float tempTimer;

    // Start is called before the first frame update
    void Start()
    {
        AllImages = GetComponentsInChildren<Image>();

        for (int i = 1; i < AllImages.Length; i++)
        {
            AllImages[i].color = AlphaZero;
        }
        tempTimer = TimeTillNext;
    }

    // Update is called once per frame
    void Update()
    {
        if (In)
        {
            FadeFirstInArray();
        }
        else
        {
            FadeSecondInArray();
        }

        if (autoFade)
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
        }
    }

    public void ResetValues()
    {
        In = true;
        tempTimer = TimeTillNext;
    }

    void FadeFirstInArray()
    {
        AllImages[0].color = Color.Lerp(AllImages[0].color, AlphaUp, FadeSpeed);
        AllImages[1].color = Color.Lerp(AllImages[1].color, AlphaZero, FadeSpeed);
    }

    void FadeSecondInArray()
    {
        AllImages[0].color = Color.Lerp(AllImages[0].color, AlphaZero, FadeSpeed);
        AllImages[1].color = Color.Lerp(AllImages[1].color, AlphaUp, FadeSpeed);
    }
}
