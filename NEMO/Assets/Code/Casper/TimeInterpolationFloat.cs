using CM;
using CM.Essentials.Timing;
using System;
using UnityEngine;

public class TimeInterpolationFloat : TimeInterpolation<float>
{
	protected override float GetCurrentValue()
	{
		return Mathf.Lerp(startInterpolation, TargetInterpolation, currentTime / totalTime);
	}

	public static TimeInterpolationFloat InterpolateTo(GameObject addComponentAt, float startInterpolation, float targetInterpolation, TimeData time, Action callback)
	{
		TimeInterpolationFloat timeInterpolation = addComponentAt.AddComponent<TimeInterpolationFloat>();
		timeInterpolation.InterpolateTo(startInterpolation, targetInterpolation, time, callback, true);

		return timeInterpolation;
	}
}