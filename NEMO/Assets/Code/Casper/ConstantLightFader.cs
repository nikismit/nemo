using UnityEngine;

namespace CM.Essentials
{
	[RequireComponent(typeof(Light))]
	public class ConstantLightFader : LightFader
	{
		private void Start()
		{
			FadeIn();
		}

		protected override void OnFadeInFinish()
		{
			base.OnFadeInFinish();

			FadeOut();
		}

		protected override void OnFadeOutFinish()
		{
			base.OnFadeOutFinish();

			FadeIn();
		}
	}
}