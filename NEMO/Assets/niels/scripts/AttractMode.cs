using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractMode : MonoBehaviour
{
    public NemoController nemoController;
    public DMXControll dMXControll;
    public ObjectScaler objectScaler;
    public GameObject mutateShader;

    public GameEventListener[] test;

    public float AFKTimerMax;
    public float timer = 0;

    public bool active;
    public bool scaleGrowing;

    public int once;

    void Start()
    {
        test = mutateShader.GetComponentsInChildren<GameEventListener>();
    }

    // Update is called once per frame
    void Update()
    {
        scaleGrowing = objectScaler.IsGrowing;

        if (!nemoController._isBeltConnected)
        {
            if (!active)
            {
                timer += Time.deltaTime;

                if (timer > AFKTimerMax)
                {
                    timer = 0;
                    active = true;
                }
            }
        }
        else
        {
            timer = 0;
            active = false;
        }

        if (active)
        {
            if (objectScaler.IsGrowing)
            {
                if (once < 1)
                {
                    test[0].Response.Invoke();
                    once++;
                }

                dMXControll.simulate = true; //moet fade in zijn, edit later een fucntie in dmxcontroller
            }
            else
            {
                if (once >= 1)
                {
                    test[1].Response.Invoke();
                    once--;
                }

                dMXControll.simulate = false; //moet fade out zijn, edit later een fucntie in dmxcontroller
            }
        }
        else
        {
            dMXControll.simulate = false;
        }
    }
}
