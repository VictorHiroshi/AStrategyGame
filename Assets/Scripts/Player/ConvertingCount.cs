using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountingStruct
{
	public GameObject gameObject;
	public Renderer renderer;
}

public class ConvertingCount : MonoBehaviour {

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
			
			countingMeshes.Add (temporaryCountingStruct);

		}
		Debug.Log (countingMeshes.Count);
	}
	
	public void SetColors(Color newColor)
	{
		foreach (CountingStruct mesh in countingMeshes)
		{
			mesh.renderer.material.SetColor ("_Color", newColor);
		}
	}
}
