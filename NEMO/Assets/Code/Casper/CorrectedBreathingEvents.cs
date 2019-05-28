using CM.Essentials.Timing;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CorrectedBreathingEvents : MonoBehaviour
{
	public TimeData timeBeforeNextCheck;

	public UnityEvent OnInhalingCorrected;
	public UnityEvent OnExhalingCorrected;

	private ObjectScaler _objectScaler;
	private bool _isRoutineRunning = false;

	public void ExecuteInhaling()
	{
		if (!_isRoutineRunning)
		{
			OnInhalingCorrected.Invoke();
			StartCoroutine(Routine());
		}
	}

	public void ExecuteExhaling()
	{
		if (!_isRoutineRunning)
		{
			OnExhalingCorrected.Invoke();
			StartCoroutine(Routine());
		}
	}

	private IEnumerator Routine()
	{
		_isRoutineRunning = true;
		yield return new WaitForSeconds(timeBeforeNextCheck.TotalSeconds);
		_isRoutineRunning = false;
	}
}