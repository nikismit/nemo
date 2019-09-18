using CM;
using UnityEngine;

public class SpeedMultiplierCheck : MonoBehaviour
{
    public int calculations = 10;
    public float multiplier = 2;
    public float maxSpeedMultiplier = 3;
    public float minBeltStrengthToActivateMultiplier = 0.9f;
    public float minBeltStrengthToActivateUI = 0.98f;

    [Header("Events")]
    public GameEvent BadBreathingMultiplierEvent;
    public GameEvent BadBreathingUIEvent;

    private float _averageMinValue = -1;
    private float _averageMaxValue = -1;

    private float[] _minValueList = new float[10];
    private int _minValueListIndex = 0;
    private float[] _maxValueList = new float[10];
    private int _maxValueListIndex = 0;

    private float NewDiffernce;

    private void Awake()
    {
        CM_Debug.AddCategory("SpeedMultiplierManager");
    }

    private void Start()
    {
        _minValueList = new float[calculations];
        _maxValueList = new float[calculations];
    }

    private void MultiplierCheck()
    {
        if (_averageMinValue != -1 && _averageMaxValue != -1)
        {
            float difference = _averageMaxValue - _averageMinValue;
            difference = 1 - difference;

            NewDiffernce = difference;

            CM_Debug.Log("SpeedMultiplierManager", "Average max value: " + _averageMaxValue);
            CM_Debug.Log("SpeedMultiplierManager", "Average min value: " + _averageMinValue);
            CM_Debug.Log("SpeedMultiplierManager", "difference between average min and average max value: " + difference);

            // Doing great
            if (difference < minBeltStrengthToActivateMultiplier)
            {
                CM_Debug.Log("SpeedMultiplierManager", "Breathing normally");
                NemoPlayer2._instance.speedMultiplier = 1;
                return;
            }

            // Doing so bad that I need a UI to show me how to breath correctly through my belly
            if (difference > minBeltStrengthToActivateUI)
            {
                CM_Debug.Log("SpeedMultiplierManager", "Breathing incorrectly, need a UI");
                BadBreathingUIEvent.Invoke();
            }

            // Doing bad, need a multiplier
            if (difference > minBeltStrengthToActivateMultiplier)
            {
                CM_Debug.Log("SpeedMultiplierManager", "Breathing incorrectly, need a multiplier");
                BadBreathingMultiplierEvent.Invoke();
                NemoPlayer2._instance.speedMultiplier = Mathf.Clamp(difference * multiplier, 1, maxSpeedMultiplier);
            }
        }
    }

    public void BreathingIn()
    {
        _maxValueList[_maxValueListIndex] = NemoPlayer2._instance.fullness;
        CM_Debug.Log("SpeedMultiplierManager", "Current max value: " + NemoPlayer2._instance.fullness);

        _maxValueListIndex++;

        if (_maxValueListIndex >= _maxValueList.Length)
        {
            _maxValueListIndex = 0;
            _averageMaxValue = GetAverage(_maxValueList);
            
        }
    }

    public void BreathingOut()
    {
        _minValueList[_minValueListIndex] = NemoPlayer2._instance.fullness;
        CM_Debug.Log("SpeedMultiplierManager", "Current min value: " + NemoPlayer2._instance.fullness);

        _minValueListIndex++;

        if (_minValueListIndex >= _minValueList.Length)
        {
            _minValueListIndex = 0;
            _averageMinValue = GetAverage(_minValueList);
            MultiplierCheck();
        }
    }

    public float GetAverage(float[] array)
    {
        float sum = 0;
        float average = 0;

        for (var i = 0; i < array.Length; i++)
        {
            sum += array[i];
        }

        average = sum / array.Length;

        return average;
    }
}
