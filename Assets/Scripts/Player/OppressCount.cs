using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountingStruct
{
	public GameObject gameObject;
	public Renderer renderer;
}

public class OppressCount : MonoBehaviour {

	public int countingOppressTurns = 0;

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

	}
	
	public void SetColors(Color newColor)
	{
		foreach (CountingStruct mesh in countingMeshes)
		{
			mesh.renderer.material.SetColor ("_Color", newColor);
		}
	}

	public void CountDown()
	{
		if (countingOppressTurns == 0)
		{
			Oppress ();
			return;
		}

		countingOppressTurns--;
		countingMeshes [countingOppressTurns].gameObject.SetActive (false);
	}

	public void Oppress()
	{

		foreach (CountingStruct mesh in countingMeshes)
		{
			mesh.gameObject.SetActive (true);
		}

		countingOppressTurns = countingMeshes.Count;
		Color tempColor = Random.ColorHSV ();
		SetColors (tempColor);
	}
}
