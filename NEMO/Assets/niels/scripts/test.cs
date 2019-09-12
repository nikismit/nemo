using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CM;

public class test : MonoBehaviour
{
    public string CM_Category = "niels_test";
    private int CM_testint = 1;
    public bool isActive;

    [Header("test events")]
    public GameEvent testEvent;

    private void Awake()
    {
        CM_Debug.AddCategory(CM_Category);
    }

    // Start is called before the first frame update
    void Start()
    {
        isActive = gameObject.GetComponent<AudioListener>().enabled;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            CM_Debug.Log(CM_Category, " test " + CM_testint);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            CM_Debug.Log(CM_Category, " Invoke event test ");
            testEvent.Invoke();
        }
    }

    public void TestEventToggle()
    {
        if (isActive)
        {
            gameObject.GetComponent<AudioSource>().enabled = false;
            isActive = false;
        }
        else if (!isActive)
        {
            gameObject.GetComponent<AudioSource>().enabled = true;
            isActive = true;
        }
    }
}
