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

	private float _difference = 0;
	private float _minValue = 0;
	private float _maxValue = 1;

	private void Update()
	{
		float value = 0;

		// Normal
		if (!invert)
		{
			value = Mathf.Lerp(0, maxBrightness, (_difference - NemoPlayer2._instance.fullness) / _maxValue);
		}
		// Inverted
		else
		{
			value = Mathf.Lerp(maxBrightness, 0, (_difference - NemoPlayer2._instance.fullness) / _maxValue);
		}

		Shader.SetGlobalFloat(NodeName, value);
	}

	public void SetMinValue()
	{
		_difference = NemoPlayer2._instance.fullness;
	}

	public void SetMaxValue()
	{
		_maxValue = _difference - NemoPlayer2._instance.fullness;
	}
}
