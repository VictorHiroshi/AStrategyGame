using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

// Script to handle general game information, such as score points and turn information.

public class GameManager : MonoBehaviour {

	public bool neverTiredCreatures = false;
	public int maxHealth = 10;
	public int coinsPerStone = 2;
	public int coinsPerTurn = 4;
	public PanelController panelControler;
	public CameraController m_camera;
	public static GameManager instance = null;
	public GameObject tileHighlightObject;
	public GameObject [] creature;
	public Color[] playersColors;

	[HideInInspector]public BoardManager boardScript;
	/*[HideInInspector]*/
public PlayerController[] player;
	[HideInInspector]public int activePlayerIndex;
	[HideInInspector]public List<TileController> highlightedTiles;
	[HideInInspector]public List<CreatureController> tiredCreatures;
	[HideInInspector]public List<CreatureController> oppressedCreatures;

	private int actualTurn;
	private bool [] playerIsActive;

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
		SceneManager.LoadScene ("MainMenu");
	}

	public void NextTurn()
	{
		//TODO: Verify winning condition.

		activePlayerIndex += 1;
		activePlayerIndex = activePlayerIndex % player.Length;

		if (activePlayerIndex == 0) {
			CountDownOppressingTurns();
			if(GameOver())
			{
				return;
			}
			TurnChangingIncome ();
			actualTurn += 1;
			panelControler.ChangeTurnText (actualTurn);
		}

		if(!playerIsActive[activePlayerIndex])
		{
			NextTurn ();
			return;
		}

		if(player[activePlayerIndex].CheckIfLost ())
		{
			playerIsActive [activePlayerIndex] = false;
			NextTurn ();
			return;
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
		int size = boardScript.boardSize;

		player = new PlayerController[creature.Length];
		playerIsActive = new bool[creature.Length];
		
		for (int i = 0; i < player.Length; i++) {
			player [i] = new PlayerController ();
			player [i].controlledCreatures = new List<CreatureController> ();
			player [i].oppressedCreatures = new List<CreatureController> ();
			player [i].attemptingToConvert = new List<CreatureController> ();
			player [i].coinCount = 0;
			player [i].controlledStones = 0;
			player [i].playerNumber = i;
			player [i].color = playersColors[i];
			playerIsActive [i] = true;

			ListOfInitialTile list = boardScript.intialTilesForPlayer [i];

			foreach(InitialTile tileCoord in list.initialTiles)
			{
				player [i].InstantiateCreature (boardScript.getTile (tileCoord.coordX, tileCoord.coordY));
			}
		}

		/*player[0].InstantiateCreature (boardScript.getTile(size -1, size-1));
		player[1].InstantiateCreature (boardScript.getTile(size -1, 0));
		player[2].InstantiateCreature (boardScript.getTile(0, size-1));
		player[3].InstantiateCreature (boardScript.getTile(0, 0));*/


	}

	private void FocusCameraOn (PlayerController player)
	{
		TileController tile;

		if(player.controlledCreatures.Count>0)
			tile = player.controlledCreatures [0].occupiedTile;
		else
			tile = player.oppressedCreatures[0].occupiedTile;

		Transform target;

		if (tile == null)
			target = transform;
		else 
			target = tile.spawnPoint;
		
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

				creature.belongsToPlayer.GetBackOppressedCreature (creature);
				creature.oppressedByPlayer.controlledCreatures.Remove (creature);

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

	private bool GameOver()
	{
		int playerCount = 0;
		PlayerController winner = player[0];

		for(int i = 0; i<player.Length; i++)
		{
			if (playerIsActive[i]) 
			{
				playerCount++;
				winner = player [i];
			}
		}

		if (playerCount > 1)
			return false;

		StartCoroutine (FinishGame(winner));
		return true;
	}

	private IEnumerator FinishGame(PlayerController winner)
	{
		ActionsManager.instance.actualState = ActionState.GameOver;
		panelControler.GameOverMessage (winner);
		yield return null;
	}
}
