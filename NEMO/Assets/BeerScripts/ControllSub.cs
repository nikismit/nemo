using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllSub : MonoBehaviour
{
    public DMXControll dmx;
    public AnimationCurve curve;
    public bool direction;
    public NemoController controller;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (direction)
        {
            if(controller._isBeltConnected == true){
                this.GetComponent<AudioSource>().volume = curve.Evaluate(dmx.relVal);
            }
            
            if(controller._isBeltConnected == false){
                this.GetComponent<AudioSource>().volume = 0;
            }

        }
        else
        {
           if(controller._isBeltConnected == true){
                this.GetComponent<AudioSource>().volume = 1- curve.Evaluate(dmx.relVal);
            }

             if(controller._isBeltConnected == false){
                this.GetComponent<AudioSource>().volume = 0;
            }
        }
        
    }
}
