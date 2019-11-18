using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallBreaths : MonoBehaviour
{
    public NemoController nemoController;

    public int changeInt;
    public bool test;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (test)
        {
            nemoController.arduinoTest = true;
            StartCoroutine("start");
        }
        else
        {
            nemoController.arduinoTest = false;
        }
    }

    public IEnumerator start()
    {
        nemoController.value += changeInt;
        yield return new WaitForSeconds(1);
        nemoController.value -= changeInt;
        yield return new WaitForSeconds(1);
    }
}
