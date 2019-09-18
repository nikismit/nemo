using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllSub : MonoBehaviour
{
    public DMXControll dmx;
    public AnimationCurve curve;
    public bool direction;
    public NemoController controller;

    public bool trigger;

    public bool triggered;
    public float triggerLength;
    public float triggerTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(  triggered == true){
            if(triggerTime <= triggerLength){
                triggerTime+= Time.deltaTime;
                this.GetComponent<AudioSource>().volume = curve.Evaluate(triggerTime/triggerLength); 
            }
        }

        if(controller._isBeltConnected == false){
            this.GetComponent<AudioSource>().volume = 0;
            
        }

        if (direction)
        {   
             
            if(trigger == false){
                if(controller._isBeltConnected == true){
                    this.GetComponent<AudioSource>().volume = curve.Evaluate(dmx.relVal);
                }
            }

            if(trigger ==true){
                if(controller.breathState == NemoController.States.breathingIn && triggered == false){
                    triggered = true;
                }

                if(controller.breathState == NemoController.States.breathingOut && triggered == true){
                    triggered = false;
                    triggerTime =0;
                }
            }
        }
        else
        {
            if(trigger == false){
            if(controller._isBeltConnected == true){
                    this.GetComponent<AudioSource>().volume = 1- curve.Evaluate(dmx.relVal);
                }
            }

             if(trigger ==true){
                if(controller.breathState == NemoController.States.breathingOut && triggered == false){
                    triggered = true;
                }
                 if(controller.breathState == NemoController.States.breathingIn && triggered == true){
                    triggered = false;
                    triggerTime =0;
                }
            }
        }
        if(controller.normalBreathing == false){
                  this.GetComponent<AudioSource>().volume = 0;
        }
    }
}
