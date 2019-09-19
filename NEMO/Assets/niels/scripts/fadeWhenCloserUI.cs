using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class fadeWhenCloserUI : MonoBehaviour
{
    public GameObject objectToWatch;
    public float triggerDistance = 20f, closeTriggerDistance = 10f;

    private Color startColor, newColor = new Color(1f, 1f, 1f, 0f);
    private float distance;
    private TMP_Text TMPT;

    // Start is called before the first frame update
    void Start()
    {
        if (objectToWatch == null)
        {
            objectToWatch = GameObject.Find("************************Player");
            if (objectToWatch == null)
            {
                Debug.Log("Can't find the searched object on " + gameObject.name + " script: " + this.name);
            }
        }

        TMPT = GetComponent<TextMeshPro>();
        startColor = TMPT.color;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, objectToWatch.transform.position);

        if (distance > triggerDistance || distance < closeTriggerDistance)
        {
            TMPT.color = Color.Lerp(GetComponent<TextMeshPro>().color, newColor, 0.1f);
        }
        else
        {
            TMPT.color = Color.Lerp(GetComponent<TextMeshPro>().color, startColor, 0.1f);
        }
    }
}
