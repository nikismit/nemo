using CM.Essentials.Timing;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
	public TimeData time;

	public UnityEvent OnTimerFinished;

	public void Execute()
	{
		StartCoroutine(Routine());
	}

	public void Execute(TimeData time)
	{
		this.time = time;
		Execute();
	}

	private IEnumerator Routine()
	{
		yield return new WaitForSeconds(time.TotalSeconds);
		TimerFinished();
	}

	private void TimerFinished()
	{
		OnTimerFinished.Invoke();
	}
}