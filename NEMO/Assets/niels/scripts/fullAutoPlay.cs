using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fullAutoPlay : MonoBehaviour
{
    public GameEvent BeltConnect;
    public GameEvent BeltDisconnect;
    public GameEvent TutorialFinished;

    public KeyCode beltKey = KeyCode.B;
    public KeyCode TutorialKey = KeyCode.T;
    public KeyCode simulateKey = KeyCode.S;

    public PlayerSimulate playerSimulate;
    public NEMO_GameEvents gameEvents;
    public string gameState;

    public bool auto;
    public bool breathingDuringTutorial;
    public int overlapTime;

    public bool belt;
    private bool player;

    private int i;

    // Update is called once per frame
    void Update()
    {
        gameState = gameEvents.GameState.ToString();

        if (!auto)
        {
            if (Input.GetKeyDown(beltKey))
            {
                if (!belt)
                {
                    BeltConnect.Invoke();
                    belt = true;
                }
                else
                {
                    BeltDisconnect.Invoke();
                    belt = false;
                }
            }

            if (Input.GetKeyDown(TutorialKey))
            {
                TutorialFinished.Invoke();
            }

            if (Input.GetKeyDown(simulateKey))
            {
                if (!player)
                {
                    playerSimulate.simulatePlayer = true;
                    player = true;
                }
                else
                {
                    playerSimulate.simulatePlayer = false;
                    player = false;
                }
            }
        }
        else
        {
            if (gameState == "WaitingForPlayer")
            {
                if (i == 0)
                {
                    i++;
                    StartCoroutine("AutoStart");
                }
            }
        }
    }

    private IEnumerator AutoStart()
    {
        if (belt)
        {
            yield return new WaitForSeconds(overlapTime);
            BeltDisconnect.Invoke();
            belt = false;
            playerSimulate.simulatePlayer = false;
            player = false;
        }
        yield return new WaitForSeconds(overlapTime);
        BeltConnect.Invoke();
        belt = true;
        if (breathingDuringTutorial)
        {
            playerSimulate.simulatePlayer = true;
            player = true;
        }
        else
        {
            yield return new WaitForSeconds(overlapTime);
            TutorialFinished.Invoke();
            playerSimulate.simulatePlayer = true;
            player = true;
        }

        StopCoroutine("AutoStart");
        i = 0;
    }
}
