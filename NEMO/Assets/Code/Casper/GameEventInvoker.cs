using CM.Essentials.Timing;
using System.Collections;
using UnityEngine;

public class GameEventInvoker : MonoBehaviour
{
	public GameEvent gameEventToInvoke;
	public bool delay;
	public TimeData delayTime;

	public void Execute()
	{
		if (delay)
		{
			StartCoroutine(Routine());
		}
		else
		{
			InvokeGameEvent();
		}
	}

	public void Execute(TimeData delayTime)
	{
		delay = true;
		this.delayTime = delayTime;
		Execute();
	}

	private IEnumerator Routine()
	{
		yield return new WaitForSeconds(delayTime.TotalSeconds);
		InvokeGameEvent();
	}

	private void InvokeGameEvent()
	{
		gameEventToInvoke.Invoke();
	}
}