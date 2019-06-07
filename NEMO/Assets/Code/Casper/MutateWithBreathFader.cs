using UnityEngine;

namespace CM.Essentials
{
	public class MutateWithBreathFader : FloatFader<MutateWithBreathFader>
	{
		public string nodeName = "globalBreathValue";

		protected override float GetComponentValue()
		{
			return Shader.GetGlobalFloat(nodeName);
		}

		protected override void SetComponentValue(float value)
		{
			Shader.SetGlobalFloat(nodeName, value);
		}
	}
}