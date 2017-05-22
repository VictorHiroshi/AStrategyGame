using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script to handle general game information, such as score points.

public class GameManager : MonoBehaviour {

	[HideInInspector]public BoardManager boardScript;
	[HideInInspector]public PlayerController player1;
	[HideInInspector]public PlayerController player2;
	[HideInInspector]public PlayerController player3;
	[HideInInspector]public PlayerController player4;

	public CameraController m_camera;
	public static GameManager instance = null;
	public GameObject tileHighlightObject;
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

		DontDestroyOnLoad (gameObject);
		boardScript = GetComponent <BoardManager> ();
		InitializeGame ();	
	}
		
	void InitializeGame ()
	{
		AssignPlayers ();
		boardScript.SetupScene ();
		FocusCameraOn (player1);
	}

	private void AssignPlayers ()
	{
		int xMax = boardScript.columns;
		int zMax = boardScript.rows;

		player1 = new	PlayerController ();
		player1.controlledTiles = new List<string> ();
		player1.controlledTiles.Add (TileController.GetStringID (xMax -1, zMax-1));
		player1.creature = creature1;

		player2 = new	PlayerController ();
		player2.controlledTiles = new List<string> ();
		player2.controlledTiles.Add (TileController.GetStringID (xMax -1, 0));
		player2.creature = creature2;

		player3 = new	PlayerController ();
		player3.controlledTiles = new List<string> ();
		player3.controlledTiles.Add (TileController.GetStringID (0, zMax-1));
		player3.creature = creature3;

		player4 = new	PlayerController ();
		player4.controlledTiles = new List<string> ();
		player4.controlledTiles.Add (TileController.GetStringID (0, 0));
		player4.creature = creature4;
	}

	private void FocusCameraOn (PlayerController player)
	{
		string id = player.controlledTiles [0];
		Transform target = boardScript.getTile (id).spawnPoint;
		m_camera.MoveToTarget (target);
		Debug.Log ("trying FocusCamera " + target);
	}
}
