using UnityEngine;

public class GameEventInvoker : MonoBehaviour
{
	public GameEvent gameEventToInvoke;

	public void Execute()
	{
		gameEventToInvoke.Invoke();
	}
}