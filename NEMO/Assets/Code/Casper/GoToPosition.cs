using UnityEngine;

public class GoToPosition : MonoBehaviour
{
	private Vector3 _goToPosition;
	private Vector3 _startPosition;

	private bool _isMoving = false;
	public bool IsMoving { get => _isMoving; }

	[SerializeField]
	private float _seconds = 5f;
	private float _timer = 0.0f;

	public delegate void FinishedHandler();
	public event FinishedHandler FinishedEvent;

	private void Start()
	{
		_startPosition = transform.position;
	}

	private void Update()
	{
		if (IsMoving)
		{
			transform.position = Vector3.Lerp(_startPosition, _goToPosition, _timer / _seconds);
			if (_timer < _seconds)
			{
				_timer += Time.deltaTime;
			}
			else
			{
				FinishedEvent?.Invoke();
				_isMoving = false;
				_timer = 0.0f;
			}
		}
	}

	public void Execute(Vector3 goToPosition)
	{
		_isMoving = true;
		_startPosition = transform.position;
		_goToPosition = goToPosition;
	}
}
