using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum HighlightType {NextTurn, Coins, Move, Duplicate, LightExploit, HeavyExploit, Attack, Convert, Oppress, Defend, None, Empty};
public enum MessageType {None, CantPerformAction, CreatureTooTired, SelectTileFirst, NotEnoughtMoney, NotYourCreature, NoEnemy, NoCreatureThere, NoStone, Healing, CreatureDefending};

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
	public Button cancelButton;

	[HideInInspector]public HighlightType selectedUI = HighlightType.None;
	[HideInInspector]public bool canChangePlayerText;

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
	private bool scrolling;

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

		HideCancelButton ();
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

	public void ShowMessage(float displayingTime, MessageType type)
	{
		switch(type)
		{
		case MessageType.CantPerformAction:
			StartCoroutine (ShowingMessage (displayingTime, Descriptions.UNAVALIABLE_ACTION));
			break;
		case MessageType.CreatureTooTired:
			StartCoroutine (ShowingMessage (displayingTime, Descriptions.TOO_TIRED_MAAN));
			break;
		case MessageType.SelectTileFirst:
			StartCoroutine (ShowingMessage (displayingTime, Descriptions.SELECT_A_TILE));
			break;
		case MessageType.NotEnoughtMoney:
			StartCoroutine (ShowingMessage (displayingTime, Descriptions.NO_MONEY));
			break;
		case MessageType.NotYourCreature:
			StartCoroutine (ShowingMessage (displayingTime, Descriptions.NOT_YOUR_CREATURE));
			break;
		case MessageType.NoCreatureThere:
			StartCoroutine (ShowingMessage (displayingTime, Descriptions.NO_CREATURE_THERE));
			break;
		case MessageType.NoEnemy:
			StartCoroutine (ShowingMessage (displayingTime, Descriptions.NO_ENEMY));
			break;
		case MessageType.NoStone:
			StartCoroutine (ShowingMessage (displayingTime, Descriptions.NO_STONE));
			break;
		case MessageType.CreatureDefending:
			StartCoroutine (ShowingMessage (displayingTime, Descriptions.CREATURE_DEFENDING));
			break;
		case MessageType.Healing:
			StartCoroutine (ShowingMessage (displayingTime, Descriptions.HEALING));
			break;
		}
	}

	public void ShowCancelButton()
	{
		cancelButton.gameObject.SetActive (true);
	}

	public void HideCancelButton()
	{
		cancelButton.gameObject.SetActive (false);
	}

	public void UpdateTileMessage(TileController tile)
	{
		selectedUI = HighlightType.Empty;
		descriptionPanelScrollbar.value = 1;
		string message = "";
		if(tile.creature!=null)
		{
			message += "This tile belongs to player " + (tile.creature.belongsToPlayer.playerNumber+1)+"!\n";

			if(tile.creature.influencedByPlayer != null)
			{
				message += "This creature is under the influence of player " + (tile.creature.influencedByPlayer.playerNumber+1) + "!\n";
			}

			if(tile.creature.oppressedByPlayer != null)
			{
				message += "This creature is controlled by oppression of player " + (tile.creature.oppressedByPlayer.playerNumber+1) + "!\n";
			}
		}
		else
		{
			message += "Nobody owns this tile yet!\n";
		}

		if(tile.resource!=null)
		{
			message += "Resource: Stones\n\n";
			message += Descriptions.RESOURCE_STONE;
		}
		else{
			message += "Resource: None\n";
		}
		descriptionText.text = message;
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

	public void DisableAllButtons()
	{
		nextTurnButton.interactable=false;
		moveButton.interactable=false;
		duplicateButton.interactable=false;
		lightExploitButton.interactable=false;
		heavyExploitButton.interactable=false;
		attackButton.interactable=false;
		convertButton.interactable=false;
		oppressButton.interactable=false;
		defendButton.interactable=false;
	}
	public void EnableAllButtons()
	{
		nextTurnButton.interactable=true;
		moveButton.interactable=true;
		duplicateButton.interactable=true;
		lightExploitButton.interactable=true;
		heavyExploitButton.interactable=true;
		attackButton.interactable=true;
		convertButton.interactable=true;
		oppressButton.interactable=true;
		defendButton.interactable=true;
	}

	private IEnumerator ShowingMessage(float time, string message)
	{
		canChangePlayerText = false;
		descriptionText.text = message;
		yield return new WaitForSeconds (time);
		canChangePlayerText = true;
		selectedUI = HighlightType.None;
	}
}
