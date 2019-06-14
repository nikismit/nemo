using CM;
using UnityEngine;

public class SpeedMultiplierCheck : MonoBehaviour
{
	public int calculations = 10;
	public float multiplier = 2;
	public float maxSpeedMultiplier = 3;
	public float minBeltStrengthToActivate = 0.2f;

	private float _averageMinValue = -1;
	private float _averageMaxValue = -1;

	private float[] _minValueList = new float[10];
	private int _minValueListIndex = 0;
	private float[] _maxValueList = new float[10];
	private int _maxValueListIndex = 0;

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

			CM_Debug.Log("SpeedMultiplierManager", "Average max value: " + _averageMaxValue);
			CM_Debug.Log("SpeedMultiplierManager", "Average min value: " + _averageMinValue);
			CM_Debug.Log("SpeedMultiplierManager", "difference between average min and average max value: " + difference);

			// Doing great
			if (difference < minBeltStrengthToActivate)
			{
				NemoPlayer2._instance.speedMultiplier = 1;
				return;
			}

			// Doing bad, need a multiplier
			NemoPlayer2._instance.speedMultiplier = Mathf.Clamp(difference * multiplier, 1, maxSpeedMultiplier);
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
