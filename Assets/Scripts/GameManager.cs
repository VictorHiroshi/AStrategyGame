using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

// Script to handle general game information, such as score points and turn information.

public class GameManager : MonoBehaviour {

	[HideInInspector]public BoardManager boardScript;
	[HideInInspector] public PlayerController[] player;

	public int maxHealth = 10;
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
	[HideInInspector]public List<CreatureController> oppressedCreatures;

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
		oppressedCreatures = new List<CreatureController> ();
	}
		
	void InitializeGame ()
	{
		activePlayerIndex = 3;
		actualTurn = 0;
		boardScript.SetupScene ();
		panelControler.selectedUI = HighlightType.None;
		AssignPlayers ();
		NextTurn ();
	}

	public void RestartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void BackToMenu()
	{
		if(!SceneManager.GetSceneByName ("MainMenu").IsValid ())
			SceneManager.LoadScene ("MainMenu", LoadSceneMode.Additive);
		
		SceneManager.SetActiveScene (SceneManager.GetSceneByName ("MainMenu"));
	}

	public void NextTurn()
	{
		//TODO: Verify winning condition.
		do {
			activePlayerIndex += 1;
			activePlayerIndex = activePlayerIndex % player.Length;

			if (activePlayerIndex == 0) {
				CountDownOppressingTurns();
				TurnChangingIncome ();
				actualTurn += 1;
				panelControler.ChangeTurnText (actualTurn);
			}
		} while(player [activePlayerIndex].controlledTiles.Count <= 0);

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
		int size = boardScript.boardSize;

		player = new PlayerController[creature.Length];
		
		for (int i = 0; i < player.Length; i++) {
			player [i] = new PlayerController ();
			player [i].controlledTiles = new List<TileController> ();
			player [i].coinCount = 0;
			player [i].controlledStones = 0;
			player [i].playerNumber = i;
			player [i].color = playersColors[i];
		}

		player[0].controlledTiles.Add (boardScript.getTile(size -1, size-1));
		player[1].controlledTiles.Add (boardScript.getTile(size -1, 0));
		player[2].controlledTiles.Add (boardScript.getTile(0, size-1));
		player[3].controlledTiles.Add (boardScript.getTile(0, 0));
	}

	private void FocusCameraOn (PlayerController player)
	{
		TileController tile = player.controlledTiles [0];
		Transform target = tile.spawnPoint;
		m_camera.MoveToTarget (target);
	}

	private void TurnChangingIncome ()
	{
		for (int i = 0; i < player.Length; i++) {
			player [i].coinCount += coinsPerTurn;
			player [i].coinCount += (coinsPerStone * player [i].controlledStones);
		}

	}

	private void CountDownOppressingTurns()
	{
		List<CreatureController> toRemove = new List<CreatureController> ();
		foreach(CreatureController creature in oppressedCreatures)
		{
			creature.oppressScript.CountDown ();
			if(!creature.oppressScript.HasTurnsLeft ())
			{
				toRemove.Add (creature);
				creature.oppressedByPlayer = null;
			}
		}

		foreach(CreatureController creature in toRemove)
		{
			oppressedCreatures.Remove (creature);
		}
	}

	private void ClearTiredCreaturesList()
	{
		foreach(CreatureController creature in tiredCreatures)
		{
			creature.isTired = false;
			creature.moved = false;
			creature.animatorController.SetTrigger ("IsIdle");
		}
		tiredCreatures.Clear ();
	}
}
