using CM.Essentials.Timing;
using System.Collections;
using UnityEngine;

public class CorrectedBreathingEventManager : MonoBehaviour
{
	public TimeData breathingDetectionDelay;

	[Header("Corrected Breathing Events")]
	public GameEvent CorrectedBreathingInEvent;
	public GameEvent CorrectedBreathingOutEvent;

	private bool _isBreathingIn = false;

	public void BreathingIn()
	{
		_isBreathingIn = true;
		StartCoroutine(Routine(breathingDetectionDelay, _isBreathingIn));
	}

	public void BreathingOut()
	{
		_isBreathingIn = false;
		StartCoroutine(Routine(breathingDetectionDelay, _isBreathingIn));
	}

	private IEnumerator Routine(TimeData time, bool isBreathingIn)
	{
		bool valueChanged = false;

		while (time.TotalSeconds > 0)
		{
			if (_isBreathingIn != isBreathingIn)
			{
				valueChanged = true;
				yield break;
			}

			time -= Time.deltaTime;
			yield return null;
		}

		if (!valueChanged)
		{
			CorrectedBreathing(isBreathingIn);
		}
	}

	private void CorrectedBreathing(bool isBreathingIn)
	{
		if (isBreathingIn)
		{
			CorrectedBreathingInEvent.Invoke();
		}
		else
		{
			CorrectedBreathingOutEvent.Invoke();
		}
	}
}