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

    public float moveSpeed;
    public float rotateAngle;
    public float rotateSpeed;

    private bool moving = false;

    public float distanceDeactivate = 10;

    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(objectToWatch.transform, true);

        ResetTextMeshProPosition();

        TMPT = GetComponent<TextMeshPro>();
        startColor = TMPT.color;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, objectToWatch.transform.position);

        if (moving)
        {
            transform.position += new Vector3(0, 0, moveSpeed);
            if (distance < triggerDistance && distance > closeTriggerDistance)
            {
                transform.Rotate(new Vector3(0, 0, rotateAngle) * (rotateSpeed * Time.deltaTime));
            }
        }

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

        if (distance > triggerDistance + distanceDeactivate && moving)
        {
            ResetTextMeshProPosition();
            gameObject.SetActive(false);
        }
    }

    public void ResetTextMeshProPosition()
    {
        moving = false;
        transform.position = new Vector3(transform.position.x, transform.position.y, objectToWatch.transform.position.z - 1);
        transform.rotation = Quaternion.identity;
    }

    public void StartMoving()
    {
        moving = true;
    }
}
