using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script to handle general game information, such as score points and turn information.

public class GameManager : MonoBehaviour {

	[HideInInspector]public BoardManager boardScript;
	/*[HideInInspector]*/
public PlayerController[] player;

	public int coinsPerStone = 2;
	public int coinsPerTurn = 4;
	public PanelController panelControler;
	public CameraController m_camera;
	public static GameManager instance = null;
	public GameObject tileHighlightObject;
	public GameObject [] creature;
	public Color[] playersColors;

	[HideInInspector]public int activePlayerIndex;
	[HideInInspector]public List<TileController> highlightedTiles;
	[HideInInspector]public List<CreatureController> tiredCreatures;

	private int actualTurn;

	void Awake () {
		// Defining this object as a singleton.
		if (instance == null)
			instance = this;
		else
			Destroy (this);

		boardScript = GetComponent <BoardManager> ();
		InitializeGame ();	

		highlightedTiles = new List<TileController> ();
		tiredCreatures = new List<CreatureController> ();
	}
		
	void InitializeGame ()
	{
		activePlayerIndex = 3;
		actualTurn = 0;
		AssignPlayers ();
		boardScript.SetupScene ();
		NextTurn ();
		panelControler.selectedUI = HighlightType.None;
	}

	public void NextTurn()
	{
		activePlayerIndex += 1;
		activePlayerIndex = activePlayerIndex % player.Length;

		if(activePlayerIndex == 0)
		{
			TurnChangingIncome ();
			actualTurn += 1;
			panelControler.ChangeTurnText (actualTurn);
		}

		panelControler.ChangeActivePlayer ("Player " + (activePlayerIndex + 1));
		panelControler.updateCoins (player [activePlayerIndex].coinCount);
		FocusCameraOn (player [activePlayerIndex]);
		ClearTiredCreaturesList ();
		ClearSelections ();
	}

	public void ClearSelections()
	{
		foreach(TileController tile in highlightedTiles)
		{
			tile.Unselect ();
		}
		highlightedTiles.Clear ();
	}

	private void AssignPlayers ()
	{
		int xMax = boardScript.columns;
		int zMax = boardScript.rows;

		player = new PlayerController[creature.Length];
		
		for (int i = 0; i < player.Length; i++) {
			player [i] = new PlayerController ();
			player [i].controlledTiles = new List<string> ();
			player [i].creature = creature [i];
			player [i].coinCount = 0;
			player [i].controlledStones = 0;
			player [i].playerNumber = i;
			player [i].color = playersColors[i];
		}

		player[0].controlledTiles.Add (TileController.GetStringID (xMax -1, zMax-1));
		player[1].controlledTiles.Add (TileController.GetStringID (xMax -1, 0));
		player[2].controlledTiles.Add (TileController.GetStringID (0, zMax-1));
		player[3].controlledTiles.Add (TileController.GetStringID (0, 0));
	}

	private void FocusCameraOn (PlayerController player)
	{
		string id = player.controlledTiles [0];
		Transform target = boardScript.getTile (id).spawnPoint;
		m_camera.MoveToTarget (target);
	}

	private void TurnChangingIncome ()
	{
		// TODO: Give turn changing money for all players.
		for (int i = 0; i < player.Length; i++) {
			player [i].coinCount += coinsPerTurn;
			player [i].coinCount += (coinsPerStone * player [i].controlledStones);
		}

	}

	private void ClearTiredCreaturesList()
	{
		foreach(CreatureController creature in tiredCreatures)
		{
			creature.isTired = false;
			creature.moved = false;
		}
		tiredCreatures.Clear ();
	}
}
