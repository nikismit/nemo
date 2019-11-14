using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractMode : MonoBehaviour
{
    public NemoController nemoController;
    public DMXControll dMXControll;
    public float AFKTimerMax;

    public float timer = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!nemoController._isBeltConnected)
        {
            timer += Time.deltaTime;

            if (timer > AFKTimerMax)
            {
                dMXControll.simulate = true;
                timer = 0;
            }
        }
        else
        {
            dMXControll.simulate = false;
            timer = 0;
        }
    }
}
