using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMeshProCollision : MonoBehaviour
{
    public GameObject textMeshProInstance;
    public GameObject player;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            //            Debug.Log("starting movement on " + textMeshProInstance);
            textMeshProInstance.GetComponent<fadeWhenCloserUI>().StartMoving();
        }
    }
}
