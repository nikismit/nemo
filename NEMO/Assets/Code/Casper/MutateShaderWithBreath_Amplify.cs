///////////////////////////////////
// HELLO!
// 
// This is a script to link a shader of an object to the players breath
//
// 1 - create your shader. 
//          I recomend doing it with a slider node to begin with
//          make sure the range of the slider is from 0 - 1 to create the effect you desire
//          this is for testing, you will be replacing it in step 2
// 2 - add a Value Node and give it a unique name
//          the Unique Name is very important!!!!
// 3 - add this script to the same object as the shader
// 4 - in Unity, replace the NODE NAME variable name of your Node from step 2
//          maske sure it matches exactly!!!!
// 5 - ENJOY!       
///////////////////////////////////

using UnityEngine;

public class MutateShaderWithBreath_Amplify : MonoBehaviour
{
	public string NodeName = "globalBreathValue";
	public float maxBrightness = 5;
	public bool invert = false;
	public int calculations = 10;

	private float _averageDifference = 0;
	private float _minValue = 0;
	private float _averageMaxValue = 1;

	private float[] _differenceList = new float[10];
	private int _differenceListIndex = 0;
	private float[] _maxValueList = new float[10];
	private int _maxValueListIndex = 0;

	private void Start()
	{
		_differenceList = new float[calculations];
		_maxValueList = new float[calculations];
	}

	private void Update()
	{
		float value = 0;

		// Normal
		if (!invert)
		{
			value = Mathf.Lerp(0, maxBrightness, (_averageDifference - NemoPlayer2._instance.fullness) / _averageMaxValue);
		}
		// Inverted
		else
		{
			value = Mathf.Lerp(maxBrightness, 0, (_averageDifference - NemoPlayer2._instance.fullness) / _averageMaxValue);
		}

		Shader.SetGlobalFloat(NodeName, value);
	}

	public void SetMinValue()
	{
		_differenceList[_differenceListIndex] = NemoPlayer2._instance.fullness;

		_differenceListIndex++;

		if (_differenceListIndex >= _differenceList.Length)
		{
			_differenceListIndex = 0;
			_averageDifference = GetAverage(_differenceList);
		}
	}

	public void SetMaxValue()
	{
		_maxValueList[_maxValueListIndex] = _averageDifference - NemoPlayer2._instance.fullness;

		_maxValueListIndex++;

		if (_maxValueListIndex >= _maxValueList.Length)
		{
			_maxValueListIndex = 0;
			_averageMaxValue = GetAverage(_maxValueList);
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
