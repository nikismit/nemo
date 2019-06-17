using CM;
using System.Collections.Generic;
using UnityEngine;

public class WorldOptimization : MonoBehaviour
{
	public List<Transform> activeGameObjectsOnStart = new List<Transform>();
	public bool loadWorldOnStart = true;

	private void Start()
	{
		CM_Debug.AddCategory("Optimization");

		if (loadWorldOnStart)
			LoadWorld();
	}

	public void LoadWorld()
	{
		CM_Debug.Log("Optimization", "Loading world start...");

		TurnWorldOff();

		CM_Debug.Log("Optimization", "Turning activeGameObjectsOnStart on");
		for (int i = 0; i < activeGameObjectsOnStart.Count; i++)
		{
			Activate(activeGameObjectsOnStart[i]);
			SetActiveAllChildren(activeGameObjectsOnStart[i], true);
		}

		CM_Debug.Log("Optimization", "Loading world finished!");
	}

	public void TurnWorldOn()
	{
		CM_Debug.Log("Optimization", "Turning world on");

		SetActiveAllChildren(transform, true);
	}

	public void TurnWorldOff()
	{
		CM_Debug.Log("Optimization", "Turning world off");

		SetActiveAllChildren(transform, false);
	}

	public void Activate(Transform transform)
	{
		SetActiveAllChildren(transform, true);

		while (transform.name != this.transform.name)
		{
			transform.gameObject.SetActive(true);
			transform = transform.parent;
		}
	}

	public void Deactivate(Transform transform)
	{
		SetActiveAllChildren(transform, false);
	}

	private void SetActiveAllChildren(Transform transform, bool value)
	{
		foreach (Transform child in transform)
		{
			child.gameObject.SetActive(value);

			SetActiveAllChildren(child, value);
		}
	}
}