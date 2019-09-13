using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllSub : MonoBehaviour
{
    public DMXControll dmx;
    public AnimationCurve curve;
    public bool direction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (direction)
        {
            this.GetComponent<AudioSource>().volume = curve.Evaluate(dmx.relVal);
        }
        else
        {
            this.GetComponent<AudioSource>().volume = 1-curve.Evaluate(dmx.relVal);
        }
        
    }
}
