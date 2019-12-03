using System.Collections;
using UnityEngine;

public class ResetPlayerPositionRoutine : MonoBehaviour
{
    public NemoController nemoController;
    public NEMO_GameEvents eventManager;
    public GoToPosition goToPosition;
    public NemoPlayer2 nemoPlayer2;
    public float seconds = 10f;

    private bool _routineRunning = false;

    private int _previousValue;

    public float[] storeVals = new float[5];
    public int checkSame;

    private IEnumerator WaitForReset(float seconds)
    {
        _routineRunning = true;
        _previousValue = nemoController.value;
        while (_routineRunning)
        {
            yield return new WaitForSeconds(seconds);

            checkSame = 0;
            for (int i = storeVals.Length - 1; i > 0; i--)
            {
                if (storeVals[i] == nemoController.value)
                {
                    checkSame += 1;
                };
                storeVals[i] = storeVals[i - 1];

            }
            storeVals[0] = nemoController.value;

            /*if (nemoController.value == _previousValue)
			{
				eventManager.EndGame();
				_routineRunning = false;
			}*/
            if (checkSame >= storeVals.Length - 1)
            {
                eventManager.EndCutscene();
                _routineRunning = false;
            }
            _previousValue = nemoController.value;
        }
    }

    public void StartRoutine()
    {
        StopAllCoroutines();
        StartCoroutine(WaitForReset(seconds));
    }
}