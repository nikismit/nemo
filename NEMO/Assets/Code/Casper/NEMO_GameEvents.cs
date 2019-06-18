using CM;
using UnityEngine;

public class NEMO_GameEvents : MonoBehaviour
{
	public GameEvent FirstStartGameEvent;
	public GameEvent StartTutorialEvent;
	public GameEvent StartGameEvent;
	public GameEvent EndGameEvent;
	public GameEvent ResetGameEvent;
	public GameEvent EndCutsceneEvent;
	public GameEvent EndCutsceneFinishedEvent;

	public enum GameStates { WaitingForPlayer, Tutorial, Game, EndingGame, EndingCutscene };

	// Current GameState
	private GameStates _gameState = GameStates.WaitingForPlayer;
	public GameStates GameState { get => _gameState; }

	private void Awake()
	{
		CM_Debug.AddCategory("NEMO GameEvents");
	}

	private void Start()
	{
		FirstStartGameEvent.Invoke();
		CM_Debug.Log("NEMO GameEvents", "FIRST START GAME EVENT");
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
		//if (_gameState != GameStates.EndingGame - 1)
			//return;

		_gameState = GameStates.EndingGame;
		EndGameEvent.Invoke();

		CM_Debug.Log("NEMO GameEvents", "END GAME EVENT");
	}

	public void EndCutscene()
	{
		if (_gameState != GameStates.Game)
			return;

		_gameState = GameStates.EndingCutscene;
		EndCutsceneEvent.Invoke();

		CM_Debug.Log("NEMO GameEvents", "END CUTSCENE EVENT");
	}

	public void EndCutsceneFinished()
	{
		if (_gameState != GameStates.EndingCutscene)
			return;

		_gameState = GameStates.EndingGame;
		EndCutsceneFinishedEvent.Invoke();

		CM_Debug.Log("NEMO GameEvents", "END CUTSCENE FINISHED EVENT");
	}
}