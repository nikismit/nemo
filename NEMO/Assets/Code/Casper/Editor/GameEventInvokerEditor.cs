using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameEventInvoker))]
public class GameEventInvokerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		GameEventInvoker gameEventInvoker = (GameEventInvoker)target;
		if (GUILayout.Button("Execute"))
		{
			gameEventInvoker.Execute();
		}
	}
}