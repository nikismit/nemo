using CM.Essentials.Timing;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioFader : MonoBehaviour
{
	public TimeData time;
	[Range(0, 1)]
	public float maxVolume = 1;

	private AudioSource _audioSource;

	private TimeInterpolationFloat _timeInterpolationFloat;

	private void Awake()
	{
		_audioSource = GetComponent<AudioSource>();
	}

	public void FadeIn()
	{
		_audioSource.Play();
		_timeInterpolationFloat = TimeInterpolationFloat.InterpolateTo(gameObject, _audioSource.volume, maxVolume, time, null);
	}

	public void FadeOut()
	{
		_timeInterpolationFloat = TimeInterpolationFloat.InterpolateTo(gameObject, _audioSource.volume, 0, time, StopAudio);
	}

	private void Update()
	{
		if (_timeInterpolationFloat)
		{
			_audioSource.volume = _timeInterpolationFloat.Value;
		}
	}

	private void StopAudio()
	{
		_audioSource.Stop();
	}
}