using CM;
using CM.Essentials.Timing;
using System;
using UnityEngine;

public class TimeInterpolationFloat : TimeInterpolation<float>
{
	public InterpolationType interpolationType;

	protected override float GetCurrentValue()
	{
		float value = 0.0f;

		switch (interpolationType)
		{
			case InterpolationType.Lerp:
				value = Mathf.Lerp(startInterpolation, TargetInterpolation, currentTime / totalTime);
				break;
			case InterpolationType.SmoothStep:
				value = Mathf.SmoothStep(startInterpolation, TargetInterpolation, currentTime / totalTime);
				break;
		}

		return value;
	}

	public static TimeInterpolationFloat InterpolateTo(GameObject addComponentAt, float startInterpolation, float targetInterpolation, TimeData time, Action callback)
	{
		TimeInterpolationFloat timeInterpolation = addComponentAt.AddComponent<TimeInterpolationFloat>();
		timeInterpolation.InterpolateTo(startInterpolation, targetInterpolation, time, callback, true);

		return timeInterpolation;
	}

	public static TimeInterpolationFloat InterpolateTo(GameObject addComponentAt, float startInterpolation, float targetInterpolation, TimeData time, Action callback, InterpolationType interpolationType)
	{
		TimeInterpolationFloat timeInterpolation = addComponentAt.AddComponent<TimeInterpolationFloat>();
		timeInterpolation.interpolationType = interpolationType;
		timeInterpolation.InterpolateTo(startInterpolation, targetInterpolation, time, callback, true);

		return timeInterpolation;
	}
}