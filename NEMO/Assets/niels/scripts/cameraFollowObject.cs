using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollowObject : MonoBehaviour
{
    public GameObject objectToFollow;
    private bool followToggle = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (objectToFollow != null && followToggle)
        {
            transform.position = new Vector3(transform.position.x, objectToFollow.transform.position.y, transform.position.z);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (followToggle)
            {
                followToggle = false;
            }
            else
            {
                followToggle = true;
            }
        }
    }
}
