using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadeWhenCloser : MonoBehaviour
{
    public GameObject objectToWatch;
    public float triggerDistance = 10f;
    public Color startColor = Color.black, newColor = new Color(1f, 1f, 1f, 0f);

    private float distance;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        if (objectToWatch == null)
        {
            print("forgot to put player in inspector");
        }

        sr = GetComponent<SpriteRenderer>();
        sr.color = startColor;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, objectToWatch.transform.position);

        if (distance < triggerDistance)
        {
            sr.color = Color.Lerp(GetComponent<SpriteRenderer>().color, newColor, 0.1f);
        }
        else
        {
            sr.color = Color.Lerp(GetComponent<SpriteRenderer>().color, startColor, 0.1f);
        }
    }
}
