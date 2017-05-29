using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ButtonType {NextTurn, Move, Duplicate, LightExploit, HeavyExploit, Attack, Convert, Oppress, Defend, None};

public class PanelController : MonoBehaviour {

	public Text turnText;
	public Text playerText;
	public Text coinsCount;
	public Text descriptionText;
	public Button nextTurnButton;
	public Button moveButton;
	public Button duplicateButton;
	public Button lightExploitButton;
	public Button heavyExploitButton;
	public Button attackButton;
	public Button convertButton;
	public Button oppressButton;
	public Button defendButton;

	private ButtonHighlight nextTurn;
	private ButtonHighlight move;
	private ButtonHighlight duplicate;
	private ButtonHighlight lightExploit;
	private ButtonHighlight heavyExploit;
	private ButtonHighlight attack;
	private ButtonHighlight convert;
	private ButtonHighlight oppress;
	private ButtonHighlight defend;
	private ButtonType selectedButton = ButtonType.None;

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
		if(nextTurn.IsHighligted () && selectedButton != ButtonType.NextTurn)
		{
			descriptionText.text = Descriptions.NEXT_TURN_BUTTON;
			selectedButton = ButtonType.NextTurn;
		}
		else if(move.IsHighligted () && selectedButton != ButtonType.Move)
		{
			descriptionText.text = Descriptions.MOVE;
			selectedButton = ButtonType.Move;
		}
		else if(duplicate.IsHighligted () && selectedButton != ButtonType.Duplicate)
		{
			descriptionText.text = Descriptions.DUPLICATE;
			selectedButton = ButtonType.Duplicate;
		}
		else if(lightExploit.IsHighligted () && selectedButton != ButtonType.LightExploit)
		{
			descriptionText.text = Descriptions.LIGHT_EXPLOIT;
			selectedButton = ButtonType.LightExploit;
		}
		else if(heavyExploit.IsHighligted () && selectedButton != ButtonType.HeavyExploit)
		{
			descriptionText.text = Descriptions.HEAVY_EXPLOIT;
			selectedButton = ButtonType.HeavyExploit;
		}
		else if(attack.IsHighligted () && selectedButton != ButtonType.Attack)
		{
			descriptionText.text = Descriptions.ATTACK;
			selectedButton = ButtonType.Attack;
		}
		else if(convert.IsHighligted () && selectedButton != ButtonType.Convert)
		{
			descriptionText.text = Descriptions.CONVERT;
			selectedButton = ButtonType.Convert;
		}
		else if(oppress.IsHighligted () && selectedButton != ButtonType.Oppress)
		{
			descriptionText.text = Descriptions.OPPRESS;
			selectedButton = ButtonType.Oppress;
		}
		else if(defend.IsHighligted () && selectedButton != ButtonType.Defend)
		{
			descriptionText.text = Descriptions.DEFEND;
			selectedButton = ButtonType.Defend;
		}
		else if(selectedButton != ButtonType.None)
		{
			descriptionText.text = Descriptions.NO_DESCRIPTION;
			selectedButton = ButtonType.None;
		}
	}

	void Start()
	{
		nextTurn = nextTurnButton.GetComponent <ButtonHighlight> ();
		move = moveButton.GetComponent <ButtonHighlight> ();
		duplicate = duplicateButton.GetComponent <ButtonHighlight> ();
		lightExploit = lightExploitButton.GetComponent <ButtonHighlight> ();
		heavyExploit = heavyExploitButton.GetComponent <ButtonHighlight> ();
		attack = attackButton.GetComponent <ButtonHighlight> ();
		convert = convertButton.GetComponent <ButtonHighlight> ();
		oppress = oppressButton.GetComponent <ButtonHighlight> ();
		defend = defendButton.GetComponent <ButtonHighlight> ();
	}
}
