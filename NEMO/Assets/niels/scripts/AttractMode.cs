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

    void Start()
    {
        test = mutateShader.GetComponentsInChildren<GameEventListener>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!nemoController._isBeltConnected)
        {
            timer += Time.deltaTime;

            if (timer > AFKTimerMax)
            {
                StartAttractMode(true);
                timer = 0;
            }
        }
        else
        {
            StartAttractMode(false);
            timer = 0;
        }
    }

    void StartAttractMode(bool checker)
    {
        dMXControll.simulate = checker;

        if (checker)
        {
            
        }
        else
        {

        }
    }
}
