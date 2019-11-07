using CM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectScaler))]
public class TutorialGrowCheck : MonoBehaviour
{
    [Header("Threshold in seconds")]
    public float range = 0.5f;
    public int counterToReach = 4;

    [Header("Game Events")]
    public GameEvent CorrectEvent;
    public GameEvent WrongEvent;
    public GameEvent FinishedEvent;
    public GameEvent TutorialExhalingEvent;
    public GameEvent TutorialInhalingEvent;

    private ObjectScaler _objectScaler;
    private bool _isRoutineRunning = false;
    private bool _isGrowRoutine = false;
    private int _counter = 0;

    private void Awake()
    {
        _objectScaler = GetComponent<ObjectScaler>();

        CM_Debug.AddCategory("NEMO Tutorial");
    }

    public void ExecuteInhaling()
    {
        if (!_isGrowRoutine)
        {
            _counter = 0;
            InvokeWrongEvent();
            CM_Debug.Log("NEMO Tutorial", "COUNTER: " + _counter);
            return;
        }

        /*niels edit
            why check if the routine is running, you alreadyhave eveything you need with the grow routine (growing or not growing)
        */

        // if (!_isRoutineRunning)
        // {
        //     _counter = 0;
        //     InvokeWrongEvent();
        //     CM_Debug.Log("NEMO Tutorial", "COUNTER: " + _counter);
        //     return;
        // }

        _counter++;
        InvokeCorrectEvent();
        CM_Debug.Log("NEMO Tutorial", "COUNTER: " + _counter);

        InvokeFinishedEvent();
    }

    public void ExecuteExhaling()
    {
        if (_isGrowRoutine)
        {
            _counter = 0;
            InvokeWrongEvent();
            CM_Debug.Log("NEMO Tutorial", "COUNTER: " + _counter);
            return;
        }

        /*niels edit           
            why check if the routine is running, you alreadyhave eveything you need with the grow routine (growing or not growing)
        */

        // if (!_isRoutineRunning)
        // {
        //     _counter = 0;
        //     InvokeWrongEvent();
        //     CM_Debug.Log("NEMO Tutorial", "COUNTER: " + _counter);
        //     return;
        // }

        _counter++;
        InvokeCorrectEvent();
        CM_Debug.Log("NEMO Tutorial", "COUNTER: " + _counter);

        InvokeFinishedEvent();
    }

    public void Reset()
    {
        _counter = 0;
    }

    private IEnumerator Routine(bool isGrowing)
    {
        _isRoutineRunning = true;
        _isGrowRoutine = isGrowing;
        yield return new WaitForSeconds(range * 2);
        _isRoutineRunning = false;
    }

    private void Update()
    {
        // Growing
        if (_objectScaler.IsGrowing)
        {
            if (_objectScaler.CurrentGrowTime >= _objectScaler.inhalingSeconds - range)
            {
                if (!_isRoutineRunning)
                {
                    StartCoroutine(Routine(false));
                    TutorialExhalingEvent.Invoke();
                }
            }
        }
        // Not growing
        else
        {
            if (_objectScaler.CurrentGrowTime >= _objectScaler.exhalingSeconds - range)
            {
                if (!_isRoutineRunning)
                {
                    StartCoroutine(Routine(true));
                    TutorialInhalingEvent.Invoke();
                }
            }
        }
    }

    private void InvokeFinishedEvent()
    {
        if (_counter >= counterToReach)
            FinishedEvent.Invoke();
    }

    private void InvokeCorrectEvent()
    {
        CorrectEvent.Invoke();
    }

    private void InvokeWrongEvent()
    {
        WrongEvent.Invoke();
    }

    void OnDisable()
    {
        _isRoutineRunning = false;
        _isGrowRoutine = false;
    }
}