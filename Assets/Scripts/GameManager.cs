using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script to handle general game information, such as score points.

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public GameObject tileHighlightObject;
	public BoardManager boardScript;
	public GameObject player1;
	public GameObject player2;
	public GameObject player3;
	public GameObject player4;


	void Awake () {
		// Defining this object as a singleton.
		if (instance == null)
			instance = this;
		else
			Destroy (this);

		if (player1 == null || player2 == null || player3 == null || player4 == null) {
			Debug.LogError ("Assign players assets first!");
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
