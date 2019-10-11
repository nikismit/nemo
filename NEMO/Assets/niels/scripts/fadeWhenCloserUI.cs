using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class fadeWhenCloserUI : MonoBehaviour
{
    public GameObject objectToWatch;
    public float triggerDistance = 20f, closeTriggerDistance = 10f;
    public float fadeSpeed = 5f;

    private Color startColor, newColor = new Color(1f, 1f, 1f, 0f);
    private float distance;
    private TMP_Text TMPT;
    private bool goneThru;

    // Start is called before the first frame update
    void Start()
    {
        goneThru = false;

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

        if (distance > triggerDistance && !goneThru || distance < closeTriggerDistance && !goneThru)
        {
            TMPT.color = Color.Lerp(GetComponent<TextMeshPro>().color, newColor, fadeSpeed / 100);
        }
        else
        {
            TMPT.color = Color.Lerp(GetComponent<TextMeshPro>().color, startColor, fadeSpeed / 100);
        }

        if (TMPT.color.a == 0 && !goneThru)
        {
            goneThru = true;
            gameObject.SetActive(false);
        }
    }
}
