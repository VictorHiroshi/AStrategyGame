using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum HighlightType {NextTurn, Coins, Move, Duplicate, LightExploit, HeavyExploit, Attack, Convert, Oppress, Defend, None};

public class PanelController : MonoBehaviour {

	public Text turnText;
	public Text playerText;
	public Text coinsCount;
	public Text descriptionText;
	public GameObject coinsObject;
	public Button nextTurnButton;
	public Button moveButton;
	public Button duplicateButton;
	public Button lightExploitButton;
	public Button heavyExploitButton;
	public Button attackButton;
	public Button convertButton;
	public Button oppressButton;
	public Button defendButton;

	private UIHighlightController nextTurn;
	private UIHighlightController move;
	private UIHighlightController duplicate;
	private UIHighlightController lightExploit;
	private UIHighlightController heavyExploit;
	private UIHighlightController attack;
	private UIHighlightController convert;
	private UIHighlightController oppress;
	private UIHighlightController defend;
	private UIHighlightController coins;
	private HighlightType selectedUI = HighlightType.None;

	public void ChangeActivePlayer(string player, int totalCoins)
	{
		playerText.text = player;
		coinsCount.text = "" + totalCoins;
	}

	public void ChangeTurnText(int turn)
	{
		turnText.text = "Turn: " + turn;
	}

	// Update is called once per frame
	void Update () {
		UpdateButtons ();
	}


	//Verifies if any button is highlighted to update the description text.
	private void UpdateButtons ()
	{
		if(nextTurn.IsHighligted () && selectedUI != HighlightType.NextTurn)
		{
			Debug.Log ("updating next turn message");
			descriptionText.text = Descriptions.NEXT_TURN_BUTTON;
			selectedUI = HighlightType.NextTurn;
		}
		else if(coins.IsHighligted () && selectedUI != HighlightType.Coins)
		{
			descriptionText.text = Descriptions.COINS;
			selectedUI = HighlightType.Coins;
		}
		else if(move.IsHighligted () && selectedUI != HighlightType.Move)
		{
			descriptionText.text = Descriptions.MOVE;
			selectedUI = HighlightType.Move;
		}
		else if(duplicate.IsHighligted () && selectedUI != HighlightType.Duplicate)
		{
			descriptionText.text = Descriptions.DUPLICATE;
			selectedUI = HighlightType.Duplicate;
		}
		else if(lightExploit.IsHighligted () && selectedUI != HighlightType.LightExploit)
		{
			descriptionText.text = Descriptions.LIGHT_EXPLOIT;
			selectedUI = HighlightType.LightExploit;
		}
		else if(heavyExploit.IsHighligted () && selectedUI != HighlightType.HeavyExploit)
		{
			descriptionText.text = Descriptions.HEAVY_EXPLOIT;
			selectedUI = HighlightType.HeavyExploit;
		}
		else if(attack.IsHighligted () && selectedUI != HighlightType.Attack)
		{
			descriptionText.text = Descriptions.ATTACK;
			selectedUI = HighlightType.Attack;
		}
		else if(convert.IsHighligted () && selectedUI != HighlightType.Convert)
		{
			descriptionText.text = Descriptions.CONVERT;
			selectedUI = HighlightType.Convert;
		}
		else if(oppress.IsHighligted () && selectedUI != HighlightType.Oppress)
		{
			descriptionText.text = Descriptions.OPPRESS;
			selectedUI = HighlightType.Oppress;
		}
		else if(defend.IsHighligted () && selectedUI != HighlightType.Defend)
		{
			descriptionText.text = Descriptions.DEFEND;
			selectedUI = HighlightType.Defend;
		}
		/*else if(selectedUI != HighlightType.None)
		{
			descriptionText.text = Descriptions.NO_DESCRIPTION;
			selectedUI = HighlightType.None;
		}*/
	}

	void Start()
	{
		nextTurn = nextTurnButton.GetComponent <UIHighlightController> ();
		coins = coinsObject.GetComponent <UIHighlightController> ();
		move = moveButton.GetComponent <UIHighlightController> ();
		duplicate = duplicateButton.GetComponent <UIHighlightController> ();
		lightExploit = lightExploitButton.GetComponent <UIHighlightController> ();
		heavyExploit = heavyExploitButton.GetComponent <UIHighlightController> ();
		attack = attackButton.GetComponent <UIHighlightController> ();
		convert = convertButton.GetComponent <UIHighlightController> ();
		oppress = oppressButton.GetComponent <UIHighlightController> ();
		defend = defendButton.GetComponent <UIHighlightController> ();
	}
}
