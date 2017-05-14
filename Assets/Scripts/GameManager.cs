using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script to handle general game information, such as score points.

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public GameObject tileHighlightObject;
	public BoardManager boardScript;

	void Awake () {
		// Defining this object as a singleton.
		if (instance == null)
			instance = this;
		else
			Destroy (this);

		DontDestroyOnLoad (gameObject);
		boardScript = GetComponent <BoardManager> ();
		InitGame ();
		
	}

	void InitGame ()
	{
		boardScript.SetupScene ();
	}
}
