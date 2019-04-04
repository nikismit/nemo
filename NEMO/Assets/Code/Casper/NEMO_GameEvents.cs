using UnityEngine;

public class NEMO_GameEvents : MonoBehaviour
{
	public GameEvent StartTutorialEvent;
	public GameEvent StartGameEvent;
	public GameEvent EndGameEvent;
	public GameEvent ResetGameEvent;

	[SerializeField]
	private bool _debug;

	public enum GameStates { WaitingForPlayer, Tutorial, Game, EndingGame };

	// Current GameState
	private GameStates _gameState = GameStates.WaitingForPlayer;
	public GameStates GameState { get => _gameState; }

	public void StartGame()
	{
		if (_gameState != GameStates.Game-1)
			return;

		_gameState = GameStates.Game;
		StartGameEvent.Invoke();

		DebugMessage("START GAME EVENT");
	}

	public void StartTutorial()
	{
		if (_gameState != GameStates.Tutorial-1)
			return;

		_gameState = GameStates.Tutorial;
		StartTutorialEvent.Invoke();

		DebugMessage("START TUTORIAL EVENT");

		StartGame();
	}

	public void ResetGame()
	{
		_gameState = GameStates.WaitingForPlayer;
		ResetGameEvent.Invoke();

		DebugMessage("RESET GAME EVENT");
	}

	public void EndGame()
	{
		if (_gameState != GameStates.EndingGame - 1)
			return;

		_gameState = GameStates.EndingGame;
		EndGameEvent.Invoke();

		DebugMessage("END GAME EVENT");
	}

	public void DebugMessage(string message)
	{
		if (_debug)
			Debug.Log(message);
	}
}