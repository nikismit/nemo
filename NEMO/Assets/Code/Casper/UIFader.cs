using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIFader : MonoBehaviour
{
	private Image _image;

	private bool _isFading = false;
	public float fadeSeconds = 0.0f;
	private float _fadeTimer = 0.0f;

	private Color _startColor;
	private Color _endColor;

	public UnityEvent onFadeStart;
	public UnityEvent onFadeEnd;

	private void Awake()
	{
		_image = GetComponent<Image>();
	}

	public void FadeTo(float seconds, Color finalColor)
	{
		if (_isFading)
			return;

		ResetVariables(finalColor);

		onFadeStart.Invoke();
		_isFading = true;
	}

	public void FadeTo(Color finalColor)
	{
		if (_isFading)
			return;

		ResetVariables(finalColor);

		onFadeStart.Invoke();
		_isFading = true;
	}


	public void FadeIn()
	{
		Color color = _image.color;
		color.a = 1;
		FadeTo(fadeSeconds, color);
	}

	public void FadeOut()
	{
		Color color = _image.color;
		color.a = 0;
		FadeTo(fadeSeconds, color);
	}

	public void StopFade()
	{
		_isFading = false;
	}

	private void ResetVariables(Color finalColor)
	{
		_fadeTimer = 0.0f;
		_startColor = _image.color;
		_endColor = finalColor;
	}

	private void Update()
	{
		if (_isFading)
		{
			_image.color = Color.Lerp(_startColor, _endColor, _fadeTimer / fadeSeconds);
			if (_fadeTimer < fadeSeconds)
			{
				_fadeTimer += Time.deltaTime;
			}
			else
			{
				onFadeEnd.Invoke();
				_isFading = false;
			}
		}
	}
}