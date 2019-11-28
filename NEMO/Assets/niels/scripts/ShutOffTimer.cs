using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutOffTimer : MonoBehaviour
{
    public NEMO_GameEvents nemo_GameEvents;
    public float maxTimer = 300;
    public float timer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (nemo_GameEvents.GameState.ToString() == "Game")
        {
            timer += Time.deltaTime;

            if (timer >= maxTimer)
            {
                nemo_GameEvents.EndCutscene();
                timer = 0;
            }
        }
    }
}
