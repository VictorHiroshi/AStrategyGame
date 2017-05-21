using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script to handle general game information, such as score points.

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public GameObject tileHighlightObject;
	public BoardManager boardScript;
	public GameObject creature1;
	public GameObject creature2;
	public GameObject creature3;
	public GameObject creature4;


	void Awake () {
		// Defining this object as a singleton.
		if (instance == null)
			instance = this;
		else
			Destroy (this);

		if (creature1 == null || creature2 == null || creature3 == null || creature4 == null) {
			Debug.LogError ("Assign creatures assets first!");
			Debug.Break ();
		}

		DontDestroyOnLoad (gameObject);
		boardScript = GetComponent <BoardManager> ();
		InitGame ();
		
	}

	void InitGame ()
	{
		boardScript.SetupScene ();
	}
}
