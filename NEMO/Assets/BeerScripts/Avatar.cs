using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviour
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

    // Use this for initialization
    void Start()
    {

        body = new GameObject[avatarPoints];

        for (int i = 0; i < avatarPoints; i++)
        {
            GameObject sphere = Instantiate(preFab);
            sphere.transform.parent = this.transform;
            sphere.transform.localScale = new Vector3(1 - (i * scaler), 1 - (i * scaler), 1 - (i * scaler));
            sphere.transform.localPosition = new Vector3(0 - (XDistance * i), 0, 0);
            body[i] = sphere;
        }
    }

    // Update is called once per frame
    void Update()
    {

        float maxArr = Helpers.maxValueRange(dataAnalyser.breathLowPass, dataAnalyser.breathLowPass.Length);
        float minArr = Helpers.minValueRange(dataAnalyser.breathLowPass, dataAnalyser.breathLowPass.Length);
        if (maxArr != minArr)
        {

            for (int i = 0; i < avatarPoints; i++)
            {
                float calcScale = Helpers.scale(minArr, maxArr, 0, 1, dataAnalyser.breathLowPass[i * pointDistance]);
                body[i].transform.localScale = new Vector3(1+(calcScale * scaler),1+ (calcScale * scaler), 1+(calcScale * scaler));
                body[i].transform.transform.Rotate(calcScale, calcScale, calcScale, Space.Self); ;
                body[i].transform.localPosition = Vector3.Lerp(body[i].transform.localPosition, new Vector3(0 - (XDistance * i), Helpers.scale(minArr, maxArr, height, -height, dataAnalyser.breathLowPass[i * pointDistance]), 0), lerper * Time.deltaTime);

            }
        }
    }
}
