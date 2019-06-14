using CM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemoPlayer2 : MonoBehaviour
{

	//singleton stuff
	public static NemoPlayer2 _instance;

	public NemoController controller;
	public NEMO_GameEvents eventManager;

	public int[] readArray = new int[10];
	private int counter = 0;
	private int rawFullness;
	private int oldFullness;
	public int fulnessDelta;

	public enum States { breathingIn, breathingOut }
	public States breathState = States.breathingIn;

	public float forwardSpeed = 1f;
	public float backSpeed = 2f;
	public float speedMultiplier = 1f;

	private float speed = 2f;

	public bool backwardOrUp;

	private float diraction = 0;

	public float smoother = 0.8f;

	// the main value to be used by the game
	// a float between 0 and 1;
	public float fullness;

	// this is to calculate fullness
	private float fullnessLow = -200f;
	private float fullnessHigh = 200f;
	public float fullnessCalibrationRate = 1f;
	private float range;

	public Vector3 startPosition;       // Start position of the player

	public GameEvent BreathingInEvent;
	public GameEvent BreathingOutEvent;

	private bool _canMove = false;

	void Awake()
	{
		//singleton stuff
		if (NemoPlayer2._instance == null)
			NemoPlayer2._instance = this;
		else
			Destroy(gameObject);

		CM_Debug.AddCategory("NEMO Player");
	}

	// Use current transform as start position
	public void SetStartPosition()
	{
		CM_Debug.Log("NEMO Player", "Setting start position to " + transform.position);
		startPosition = transform.position;
	}

	// Reset the position from the player to the start position
	public void ResetPosition()
	{
		GoToPosition goToPosition = GetComponent<GoToPosition>();

		if (!goToPosition.IsMoving)
		{
			CM_Debug.Log("NEMO Player", "Moving to " + startPosition);
			goToPosition.Execute(startPosition);
			goToPosition.FinishedEvent += OnResetPositionFinished;
		}
	}

	// Reset the game after finishing moving to the start position
	private void OnResetPositionFinished()
	{
		eventManager.ResetGame();
		GetComponent<GoToPosition>().FinishedEvent -= OnResetPositionFinished;
	}

	// Update is called once per frame
	void Update()
	{
		// are they breathing in or out 
		rawFullness = controller.value;
		fulnessDelta = oldFullness - rawFullness;
		//print(fulnessDelta);
		readArray[counter] = fulnessDelta;
		counter++;

		if (counter >= readArray.Length)
			counter = 0;

		int total = 0;

		foreach (int i in readArray)
			total += i;

		if (total > 0)
		{
			if (breathState != States.breathingIn)
			{
				BreathingInEvent.Invoke();
				// breathing in maybe 
				breathState = States.breathingIn;
				//diraction = 1;
				speed = forwardSpeed * speedMultiplier;
			}
		}
		else
		{
			if (breathState != States.breathingOut)
			{
				BreathingOutEvent.Invoke();
				// breathing out maybe 
				breathState = States.breathingOut;
				//diraction = -1;
				speed = backSpeed * speedMultiplier;
			}
		}
		oldFullness = rawFullness;

		if (fulnessDelta != 0 && breathState == States.breathingIn)
		{
			diraction = Mathf.Lerp(diraction, 1, Time.deltaTime * smoother);
		}

		if (fulnessDelta != 0 && breathState == States.breathingOut)
		{
			diraction = Mathf.Lerp(diraction, -1, Time.deltaTime * smoother);
		}

		if (fulnessDelta == 0)
		{
			diraction = Mathf.Lerp(diraction, 0, Time.deltaTime * smoother);
		}

		// move the player
		if (_canMove)
		{
			if (backwardOrUp)
			{
				transform.Translate(Vector3.forward * speed * diraction * Time.deltaTime);

			}
			else
			{
				transform.Translate(Vector3.up * speed * diraction * Time.deltaTime);
			}
		}

		fullness = CalculateFullness();
	}

	float CalculateFullness()
	{
		//first calibrate the top and bottom range

		// tuck in the bottom range
		if (rawFullness < fullnessLow)
			fullnessLow = rawFullness;
		else
			fullnessLow += fullnessCalibrationRate * Time.deltaTime;

		// tuck in the top range
		if (rawFullness > fullnessHigh)
			fullnessHigh = rawFullness;
		else
			fullnessHigh -= fullnessCalibrationRate * Time.deltaTime;

		// whats the range bro?
		range = fullnessHigh - fullnessLow;


		return ((rawFullness - fullnessLow) / range);
	}

	public void SetMovable()
	{
		_canMove = true;
	}

	public void StopMove()
	{
		_canMove = false;
	}
}