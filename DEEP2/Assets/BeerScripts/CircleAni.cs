using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAni : MonoBehaviour
{
    public float height;
    public int avatarPoints;
    public Analyser dataAnalyser;
    public GameObject preFab;
    public GameObject[] body;
    public float scaler;
    public float XDistance;
    public int pointDistance;
    public float lerper;

    public float distance;
    public float rotAdd;

    public float smoother;
    public float distanceCalc;
    public float standDistance;
    public float baseRotation;
    public float rotationAdder;

    public bool inout;

    // Start is called before the first frame update
    void Start()
    {
        body = new GameObject[avatarPoints];

        for (int i = 0; i < avatarPoints; i++)
        {
            GameObject sphere = Instantiate(preFab);
            sphere.transform.parent = this.transform;

            
            body[i] = sphere;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (dataAnalyser.breathLowPass[0]!=0) {
        float maxArr = Helpers.maxValueRange(dataAnalyser.breathLowPass, 128);
        float minArr = Helpers.minValueRange(dataAnalyser.breathLowPass, 128);
            float multi = 0;
            if (inout)
            {
                multi = Helpers.scale(minArr, maxArr, 0, 1, dataAnalyser.breathLowPass[0]);
            }
            else
            {
                multi = Helpers.scale(minArr, maxArr, 1, 0, dataAnalyser.breathLowPass[0]);
            }
        
        this.transform.Rotate(0, baseRotation +(multi* rotationAdder), 0, Space.Self);

        distanceCalc = standDistance + Mathf.Lerp(distanceCalc, multi * distance, Time.deltaTime * smoother); 
            for (int i = 0; i < avatarPoints; i++)
            {
                //float calcScale = Helpers.scale(minArr, maxArr, 0, 1, dataAnalyser.breathLowPass[i * pointDistance]);
                //body[i].transform.localScale = new Vector3(1 + (calcScale * scaler), 1 + (calcScale * scaler), 1 + (calcScale * scaler));
               // body[i].transform.transform.Rotate(calcScale, calcScale, calcScale, Space.Self); ;
                //body[i].transform.localPosition = Vector3.Lerp(body[i].transform.localPosition, new Vector3(0 - (XDistance * i), Helpers.scale(minArr, maxArr, height, -height, dataAnalyser.breathLowPass[i * pointDistance]), 0), lerper * Time.deltaTime);

                float nx;
                float nz;

                float sinAngle = Mathf.Sin(2 * Mathf.PI * i * rotAdd);
                float cosAngle = Mathf.Cos(2 * Mathf.PI * i * rotAdd);
                // Calculate new points
                Vector3 pos1 = new Vector3();
                pos1.x = distanceCalc;
                pos1.z = distanceCalc;
                nx = pos1.x * cosAngle - pos1.z * sinAngle;
                nz = pos1.x * sinAngle + pos1.z * cosAngle;
                pos1.x = nx;
                pos1.z = nz;

                body[i].transform.localPosition = pos1;

            }

        }

    }
}
