﻿using CM;
using UnityEngine;

public class NEMO_GameEvents : MonoBehaviour
{
	public GameEvent StartTutorialEvent;
	public GameEvent StartGameEvent;
	public GameEvent EndGameEvent;
	public GameEvent ResetGameEvent;

	public enum GameStates { WaitingForPlayer, Tutorial, Game, EndingGame };

	// Current GameState
	private GameStates _gameState = GameStates.WaitingForPlayer;
	public GameStates GameState { get => _gameState; }

	private void Awake()
	{
		CM_Debug.AddCategory("NEMO GameEvents");
	}

	public void StartGame()
	{
		if (_gameState != GameStates.Game-1)
			return;

		_gameState = GameStates.Game;
		StartGameEvent.Invoke();

		CM_Debug.Log("NEMO GameEvents", "START GAME EVENT");
	}

	public void StartTutorial()
	{
		if (_gameState != GameStates.Tutorial-1)
			return;

		_gameState = GameStates.Tutorial;
		StartTutorialEvent.Invoke();

		CM_Debug.Log("NEMO GameEvents", "START TUTORIAL EVENT");
	}

	public void ResetGame()
	{
		_gameState = GameStates.WaitingForPlayer;
		ResetGameEvent.Invoke();

		CM_Debug.Log("NEMO GameEvents", "RESET GAME EVENT");
	}

	public void EndGame()
	{
		if (_gameState != GameStates.EndingGame - 1)
			return;

		_gameState = GameStates.EndingGame;
		EndGameEvent.Invoke();

		CM_Debug.Log("NEMO GameEvents", "END GAME EVENT");
	}
}