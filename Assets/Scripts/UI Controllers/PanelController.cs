using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum HighlightType {NextTurn, Coins, Move, Duplicate, LightExploit, HeavyExploit, Attack, Convert, Oppress, Defend, None, Empty};

public class PanelController : MonoBehaviour {

	public Text turnText;
	public Text playerText;
	public Text coinsCount;
	public Text descriptionText;
	public Scrollbar descriptionPanelScrollbar;
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

	[HideInInspector]public HighlightType selectedUI = HighlightType.None;

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
	private bool canChangePlayerText;
	private bool scrolling;

	public void ChangeActivePlayer(string player)
	{
		playerText.text = player;

	}

	public void updateCoins(int totalCoins)
	{
		coinsCount.text = "" + totalCoins;
	}

	public void ChangeTurnText(int turn)
	{
		turnText.text = "Turn: " + turn;
	}

	public void CantPerformActionMessage(float displayingTime)
	{
		StartCoroutine (ShowMessage (displayingTime, Descriptions.UNAVALIABLE_ACTION));
	}

	void Awake()
	{
		canChangePlayerText = true;
		scrolling = false;
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
		
	void Update () {
		if(Input.GetMouseButtonDown (0))
		{
			scrolling = true;
		}
		if(Input.GetMouseButtonUp (0))
		{
			scrolling = false;
		}
		
		if (canChangePlayerText && !scrolling)
		{
			UpdateButtons ();
		}
	}

	//Verifies if any button is highlighted to update the description text.
	private void UpdateButtons ()
	{
		if(nextTurn.IsHighligted () && selectedUI != HighlightType.NextTurn)
		{
			descriptionText.text = Descriptions.NEXT_TURN_BUTTON;
			selectedUI = HighlightType.NextTurn;
			descriptionPanelScrollbar.value = 1;
		}
		else if(coins.IsHighligted () && selectedUI != HighlightType.Coins)
		{
			descriptionText.text = Descriptions.COINS;
			selectedUI = HighlightType.Coins;
			descriptionPanelScrollbar.value = 1;
		}
		else if(move.IsHighligted () && selectedUI != HighlightType.Move)
		{
			descriptionText.text = Descriptions.MOVE;
			selectedUI = HighlightType.Move;
			descriptionPanelScrollbar.value = 1;
		}
		else if(duplicate.IsHighligted () && selectedUI != HighlightType.Duplicate)
		{
			descriptionText.text = Descriptions.DUPLICATE;
			selectedUI = HighlightType.Duplicate;
			descriptionPanelScrollbar.value = 1;
		}
		else if(lightExploit.IsHighligted () && selectedUI != HighlightType.LightExploit)
		{
			descriptionText.text = Descriptions.LIGHT_EXPLOIT;
			selectedUI = HighlightType.LightExploit;
			descriptionPanelScrollbar.value = 1;
		}
		else if(heavyExploit.IsHighligted () && selectedUI != HighlightType.HeavyExploit)
		{
			descriptionText.text = Descriptions.HEAVY_EXPLOIT;
			selectedUI = HighlightType.HeavyExploit;
			descriptionPanelScrollbar.value = 1;
		}
		else if(attack.IsHighligted () && selectedUI != HighlightType.Attack)
		{
			descriptionText.text = Descriptions.ATTACK;
			selectedUI = HighlightType.Attack;
			descriptionPanelScrollbar.value = 1;
		}
		else if(convert.IsHighligted () && selectedUI != HighlightType.Convert)
		{
			descriptionText.text = Descriptions.CONVERT;
			selectedUI = HighlightType.Convert;
			descriptionPanelScrollbar.value = 1;
		}
		else if(oppress.IsHighligted () && selectedUI != HighlightType.Oppress)
		{
			descriptionText.text = Descriptions.OPPRESS;
			selectedUI = HighlightType.Oppress;
			descriptionPanelScrollbar.value = 1;
		}
		else if(defend.IsHighligted () && selectedUI != HighlightType.Defend)
		{
			descriptionText.text = Descriptions.DEFEND;
			selectedUI = HighlightType.Defend;
			descriptionPanelScrollbar.value = 1;
		}
		else if(selectedUI == HighlightType.None)
		{
			descriptionText.text = Descriptions.NO_DESCRIPTION;
			selectedUI = HighlightType.Empty;
		}
	}

	private IEnumerator ShowMessage(float time, string message)
	{
		canChangePlayerText = false;
		descriptionText.text = message;
		yield return new WaitForSeconds (time);
		canChangePlayerText = true;
		selectedUI = HighlightType.None;
	}
}
