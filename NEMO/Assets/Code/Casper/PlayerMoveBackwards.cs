using UnityEngine;

public class PlayerMoveBackwards : MonoBehaviour
{
	public float speed = 2f;
	public float speedIncreaseFactor = 1.1f;

	private float _currentSpeed = 0;

	private void Start()
	{
		_currentSpeed = speed;
	}

	private void Update()
	{
		transform.position += Vector3.back * _currentSpeed * Time.deltaTime;
		_currentSpeed *= speedIncreaseFactor;
	}

	private void OnEnable()
	{
		_currentSpeed = speed;
	}
}