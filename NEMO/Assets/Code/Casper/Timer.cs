using CM.Essentials.Timing;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    public TimeData time;

    private IEnumerator co;

    public UnityEvent OnTimerFinished;

    public void Execute()
    {
        co = Routine();
        StartCoroutine(co);
    }

    public void Execute(TimeData time)
    {
        this.time = time;
        Execute();
    }

    private IEnumerator Routine()
    {
        // Debug.Log("counting down from " + time.TotalSeconds);

        yield return new WaitForSeconds(time.TotalSeconds);

        // Debug.Log("done counting from " + time.TotalSeconds + " going to invoke now");

        TimerFinished();
    }

    private void TimerFinished()
    {
        OnTimerFinished.Invoke();
    }

    // public void ResetTimer()
    // {
    //     StopCoroutine(co);
    //     Debug.Log("stopped coroutine " + gameObject.name + co.ToString() + " / / " + time.TotalSeconds);
    // }
}