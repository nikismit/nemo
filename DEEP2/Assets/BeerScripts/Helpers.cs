using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helpers : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    // t: current time, b: beginning value, c: change in value, d: duration
    // cubic easing in/out - acceleration until halfway, then deceleration
    public static float easeInOutCubic(float t, float b, float c, float d)
    {
        if ((t /= d / 2) < 1) return c / 2 * t * t * t + b;
        return c / 2 * ((t -= 2) * t * t + 2) + b;
    }

    public static float easeInCubic(float t, float b, float c, float d)
    {
        return c * (t /= d) * t * t + b;
    }

    // cubic easing out - decelerating to zero velocity
    public static float easeOutCubic(float t, float b, float c, float d)
    {
        return c * ((t = t / d - 1) * t * t + 1) + b;
    }

    public static float easeInSine(float t, float b, float c, float d)
    {
        return -c * Mathf.Cos(t / d * (Mathf.PI / 2)) + c + b;
    }

    // sinusoidal easing out - decelerating to zero velocity
    public static float easeOutSine(float t, float b, float c, float d)
    {
        return c * Mathf.Sin(t / d * (Mathf.PI / 2)) + b;
    }

    // sinusoidal easing in/out - accelerating until halfway, then decelerating
    public static float easeInOutSine(float t, float b, float c, float d)
    {
        return -c / 2 * (Mathf.Cos(Mathf.PI * t / d) - 1) + b;
    }

    public static float linearTween(float t, float b, float c, float d)
    {
        return c * t / d + b;
    }

    public static float scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }

    public static float scaleRel(float OldMin, float OldMax, float OldValue)
    {

        float OldRange = (OldMax - OldMin);
        float NewValue = ((OldValue - OldMin)/ OldRange);

        return (NewValue);
    }

    public static float maxValue(float[] arrayLink)
    {
        float mxm = arrayLink[0];
        for (int i = 0; i < arrayLink.Length; i++)
        {
            if (arrayLink[i] > mxm)
            {
                mxm = arrayLink[i];
            }
        }
        return mxm;
    }

    public static float maxValuePos(float[] arrayLink)
    {
        float mxm = arrayLink[0];
        int pos = 0;
        for (int i = 0; i < arrayLink.Length; i++)
        {
            if (arrayLink[i] > mxm)
            {
                mxm = arrayLink[i];
                pos = i;
            }
        }
        return pos;
    }

    public static int maxValuePosRange(float[] arrayLink, int x, int y)
    {
        float mxm = arrayLink[0];
        int pos = 0;
        for (int i = x; i < y; i++)
        {
            if (arrayLink[i] > mxm)
            {
                mxm = arrayLink[i];
                pos = i;
            }
        }
        return pos;
    }

    public static float minValue(float[] arrayLink)
    {
        float mim = arrayLink[0];
        for (int i = 0; i < arrayLink.Length; i++)
        {
            if (arrayLink[i] < mim)
            {
                mim = arrayLink[i];
            }
        }
        return mim;
    }

    public static float maxValueRange(float[] arrayLink, int range)
    {
        float mxm = arrayLink[0];
        for (int i = 0; i < range; i++)
        {
            if (arrayLink[i] > mxm)
            {
                mxm = arrayLink[i];
            }
        }
        return mxm;
    }

    public static float minValueRange(float[] arrayLink, int range)
    {
        float mim = arrayLink[0];
        for (int i = 0; i < range; i++)
        {

            if (arrayLink[i] < mim )
            {
                mim = arrayLink[i];
            }
        }
        return mim;
    }


    public static float lpf(float target, float current, float smooth)
    {
        float returnVal = current;
        if (target != 0)
        {
            returnVal += (target - current) / smooth;
        }
        

        return returnVal;
    }

    public static void sortArrayMove(float[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            array[i] = array[i - 1];
        }
    }

}
