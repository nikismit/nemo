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
    public int overlapTime;

    public bool belt;
    private bool player;

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
                StartCoroutine("AutoStart");
            }
        }
    }

    private IEnumerator AutoStart()
    {
        yield return new WaitForSeconds(overlapTime);
        BeltConnect.Invoke();
        yield return new WaitForSeconds(overlapTime);
        TutorialFinished.Invoke();
        yield return new WaitForSeconds(overlapTime);
        playerSimulate.simulatePlayer = true;
        StopCoroutine("AutoStart");
    }
}
