using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountingStruct
{
	public GameObject gameObject;
	public Renderer renderer;
}

public class OppressCount : MonoBehaviour {

	private int countingOppressTurns = 0;
	private CreatureController creature;
	private List<CountingStruct> countingMeshes;

	// Use this for initialization
	void Awake () {
		
		countingMeshes = new List<CountingStruct> ();

		CountingStruct temporaryCountingStruct;

		foreach (Transform child in transform)
		{
			temporaryCountingStruct = new CountingStruct ();

			temporaryCountingStruct.gameObject = child.gameObject;
			if (temporaryCountingStruct.gameObject == null)
				Debug.LogError ("No game object in children");

			temporaryCountingStruct.renderer = child.GetComponentInParent <Renderer> ();
			if (temporaryCountingStruct.renderer == null)
				Debug.LogError ("No renderer in children");

			temporaryCountingStruct.gameObject.SetActive (false);

			countingMeshes.Add (temporaryCountingStruct);

		}

		creature = GetComponentInParent <CreatureController> ();
		if(creature==null)
		{
			Debug.LogError ("Oppress count can't reach it's creature controller.");
		}

	}

	public void CountDown()
	{
		countingOppressTurns--;

		if (countingOppressTurns < 0)
			return;
		
		countingMeshes [countingOppressTurns].gameObject.SetActive (false);
		
	}

	public bool HasTurnsLeft()
	{
		return (countingOppressTurns > 0);
	}

	public void Oppress(Color newColor)
	{

		foreach (CountingStruct mesh in countingMeshes)
		{
			mesh.gameObject.SetActive (true);
		}

		countingOppressTurns = countingMeshes.Count;
		SetColors (newColor);
	}

	public void Unoppress()
	{
		foreach (CountingStruct mesh in countingMeshes)
		{
			mesh.gameObject.SetActive (false);
		}

		countingOppressTurns = 0;
	}

	private void SetColors(Color newColor)
	{
		foreach (CountingStruct mesh in countingMeshes)
		{
			mesh.renderer.material.SetColor ("_Color", newColor);
		}
	}
}
