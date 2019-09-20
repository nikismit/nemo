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

    public float breathFreq =0f;
    public  float inTimer = 0f;
    public float outTimer = 0f;

public float treshGoodBreath = 1f;

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

            if(breathLowPass[0] > breathLowPass[5]){
                if(controller.breathState != NemoController.States.breathingIn){
                    breathFreq = (inTimer +outTimer) /2.0f;
                    controller.breathInTimer = inTimer;
                    inTimer =0;
                    
                }
                inTimer += calcUpdate;
                controller.breathState = NemoController.States.breathingIn;
            }
            if(breathLowPass[0] < breathLowPass[5]){
                 if(controller.breathState != NemoController.States.breathingOut){
                    breathFreq = (inTimer +outTimer) /2.0f;
                    controller.breathOutTimer = outTimer;
                    outTimer =0;
                   
                }
                outTimer += calcUpdate;
                controller.breathState = NemoController.States.breathingOut;
            }

            if(breathFreq >= treshGoodBreath && breathFreq <= 6){
                controller.normalBreathing = true;
            }else{
                  controller.normalBreathing = false;
            }
        }

    }
}
