using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(BoxCollider))]
public class fadeWhenCloserUI : MonoBehaviour
{
    public GameObject objectToWatch;
    public float triggerDistance = 20f, closeTriggerDistance = 10f;
    public float fadeSpeed = 5f;

    private Color startColor, newColor = new Color(1f, 1f, 1f, 0f);
    private float distance;
    private TMP_Text TMPT;
    private bool goneThru;

    private BoxCollider BC;

    private Vector3 startPos;
    private Quaternion startRot;

    public float startMoveDistance = 5;
    public float moveSpeed = 5f;
    // private bool checkBool;

    public float rotateAngle;
    public float rotationSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;

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

        BC = GetComponent<BoxCollider>();
        BC.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, objectToWatch.transform.position);

        if (distance > triggerDistance || distance < closeTriggerDistance)
        {
            //fade out
            TMPT.color = Color.Lerp(GetComponent<TextMeshPro>().color, newColor, fadeSpeed / 100);
        }
        else
        {
            //fade in
            TMPT.color = Color.Lerp(GetComponent<TextMeshPro>().color, startColor, fadeSpeed / 100);
        }

        if (goneThru)
        {
            if (distance > startMoveDistance)
            {
                transform.position += new Vector3(0, 0, moveSpeed / 100);
                transform.Rotate(new Vector3(0, 0, rotateAngle) * (rotationSpeed * Time.deltaTime));
                // checkBool = true;
            }

            // if (distance < startMoveDistance && checkBool)
            // {
            //     transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, objectToWatch.transform.position.z + 1), 100);
            // }

            if (distance > triggerDistance + 5)
            {
                goneThru = false;
                gameObject.SetActive(false);
                ResetTextMeshProPosition();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        if (other.gameObject == objectToWatch)
        {
            goneThru = true;
        }
    }

    public void ResetTextMeshProPosition()
    {
        goneThru = false;
        transform.position = startPos;
        transform.rotation = startRot;
    }
}
