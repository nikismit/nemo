using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Analyser : MonoBehaviour
{
    public float calcUpdate;
    public float calcUpdateTimer;

    public NemoController controller;

    public float[] breathLowPass = new float[1024];
    public float lowPass = 5.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        calcUpdateTimer += Time.deltaTime;

        if (calcUpdateTimer >= calcUpdate)
        {
            calcUpdateTimer = 0;
            Helpers.sortArrayMove(breathLowPass);
            breathLowPass[0] = Helpers.lpf(controller.value, breathLowPass[0], lowPass);
        }

    }
}
