using UnityEngine;

namespace CM.Essentials
{
	[RequireComponent(typeof(Light))]
	public class LightFader : FloatFader<Light>
	{
		protected override float GetComponentValue()
		{
			return component.intensity;
		}

		protected override void SetComponentValue(float value)
		{
			component.intensity = value;
		}
	}
}