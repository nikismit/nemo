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

	private IEnumerator WaitForReset(float seconds)
	{
		_routineRunning = true;
		_previousValue = nemoController.value;
		while (_routineRunning)
		{
			yield return new WaitForSeconds(seconds);

			if (nemoController.value == _previousValue)
			{
				eventManager.EndGame();
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