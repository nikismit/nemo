using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSimulate : MonoBehaviour
{
    public NemoController nemoController;

    public bool simulatePlayer;

    private float timer = 0;
    public float timerMax = 5;
    private bool toggle = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (simulatePlayer)
        {
            timer += Time.deltaTime;
            if (timer > timerMax)
            {
                timer = 0;
                if (toggle)
                {
                    toggle = false;
                }
                else
                {
                    toggle = true;
                }
            }

            if (toggle)
            {
                nemoController.value++;
            }
            else
            {
                nemoController.value--;
            }
        }
        else
        {
            timer = 0;
        }
    }
}
