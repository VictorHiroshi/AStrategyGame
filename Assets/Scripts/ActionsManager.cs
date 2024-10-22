using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionState {WaitingActionCall, PerformingAction, WaitingActionFinish, GameOver};
public enum ActionType {Move, Duplicate, LExploit, HExploit, Attack, Convert, Oppress, Defend, None};

public class ActionsManager : MonoBehaviour {

	public static ActionsManager instance;

	public int movingCost = 1;
	public int duplicateCost = 2;
	public int exploitCost = 1;
	public int attackCost = 2;
	public int defenseCost = 1;
	public int defenseExtraCost = 3;
	public int convertingCost = 2;
	public int oppressingCost = 3;
	public int healingCost = 1;
	public int defendingDamage = 5;
	public int lightExploitProfit = 3;
	public int heavyExploitProfit = 6;

	[HideInInspector]public ActionState actualState;

	private TileController selectedTile;
	private TileController targetTile;
	private List<GameObject> neighbours;
	private ActionType performing = ActionType.None;
	private IEnumerator actualStateCoroutine;

	void Awake () {
		// Defining this object as a singleton.
		if (instance == null)
			instance = this;
		else
			Destroy (this);

		actualState = ActionState.WaitingActionCall;
	}

	public void FinishAction()
	{
		GameManager.instance.panelControler.selectedUI = HighlightType.None;
		GameManager.instance.panelControler.canChangePlayerText = true;
		actualState = ActionState.WaitingActionCall;
	}

	public void SetTargetTile(TileController target)
	{
		targetTile = target;
	}

	public void CancelAction()
	{
		StopCoroutine (actualStateCoroutine);

		performing = ActionType.None;
		actualState = ActionState.WaitingActionCall;

		targetTile = null;

		GameManager.instance.ClearSelections ();
		GameManager.instance.panelControler.canChangePlayerText = true;
		GameManager.instance.panelControler.HideCancelButton ();

		selectedTile.Select ();

		GameManager.instance.panelControler.EnableAllButtons ();
	}

	public void ActionMove()
	{
		if(CanPerformAction (actionCost: movingCost, halfAction: true, needNeighbours: true))
		{
			performing = ActionType.Move;
			selectedTile.Unselect ();
			GameManager.instance.panelControler.canChangePlayerText = false;
			actualStateCoroutine = PerformingActionState (false);
			StartCoroutine (actualStateCoroutine);
		}
	}

	public void ActionDuplicate()
	{
		if(CanPerformAction (actionCost: duplicateCost, halfAction: false,  needNeighbours: true))
		{
			performing = ActionType.Duplicate;
			selectedTile.Unselect ();
			GameManager.instance.panelControler.canChangePlayerText = false;
			actualStateCoroutine = PerformingActionState (false);
			StartCoroutine (actualStateCoroutine);
		}
	}

	public void ActionLightExploit()
	{
		performing = ActionType.LExploit;
		if(CanPerformAction (actionCost: exploitCost, halfAction: false,  needNeighbours: false))
		{
			if(selectedTile.resource==null)
			{
				GameManager.instance.panelControler.ShowMessage (3f, MessageType.NoStone);
			}
			else
			{
				StartCoroutine (WaitingActionFinishState ());
				selectedTile.creature.animatorController.SetTrigger ("Explores");
				GameManager.instance.player [GameManager.instance.activePlayerIndex].Receive (lightExploitProfit);
			}
		}
		performing = ActionType.None;
	}

	public void ActionHeavyExploit()
	{
		performing = ActionType.HExploit;
		if(CanPerformAction (actionCost: exploitCost, halfAction: false,  needNeighbours: false))
		{
			if(selectedTile.resource==null)
			{
				GameManager.instance.panelControler.ShowMessage (3f, MessageType.NoStone);
			}
			else
			{
				StartCoroutine (WaitingActionFinishState ());
				selectedTile.creature.animatorController.SetTrigger ("Explores");
				GameManager.instance.player [GameManager.instance.activePlayerIndex].Receive (heavyExploitProfit);
				selectedTile.CheckIfResourceExhausted ();
			}
		}
		performing = ActionType.None;
	}

	public void ActionAttack()
	{
		if(CanPerformAction (actionCost: attackCost, halfAction: false,  needNeighbours: true))
		{
			performing = ActionType.Attack;
			selectedTile.Unselect ();
			GameManager.instance.panelControler.canChangePlayerText = false;
			actualStateCoroutine = PerformingActionState (true);
			StartCoroutine (actualStateCoroutine);
		}
	}

	public void ActionDefense()
	{
		performing = ActionType.Defend;

		if(CanPerformAction (actionCost: defenseCost, halfAction: true, needNeighbours: false))
		{
			selectedTile.Unselect ();

			if(selectedTile.creature.IsDefending ())
			{
				selectedTile.creature.TurnDefense (false);
			}
			else{
				selectedTile.creature.TurnDefense (true);
			}

			//Spend Money.
			int cost = defenseCost;
			if (CreatureIsDefending ())
				cost += defenseExtraCost;
			GameManager.instance.GetActivePlayer ().Spend (cost);
		}

		performing = ActionType.None;
	}

	public void ActionConvert()
	{
		if(CanPerformAction (actionCost: convertingCost, halfAction: false, needNeighbours: true))
		{
			performing = ActionType.Convert;
			selectedTile.Unselect ();
			GameManager.instance.panelControler.canChangePlayerText = false;
			actualStateCoroutine = PerformingActionState (true);
			StartCoroutine (actualStateCoroutine);
		}
	}

	public void ActionOppress()
	{
		if(CanPerformAction (actionCost: oppressingCost, halfAction: false, needNeighbours: true))
		{
			performing = ActionType.Oppress;
			selectedTile.Unselect ();
			GameManager.instance.panelControler.canChangePlayerText = false;
			actualStateCoroutine = PerformingActionState (true);
			StartCoroutine (actualStateCoroutine);
		}
	}

	private void MoveCreature()
	{
		// Spend money and move the creature.
		int cost = movingCost;
		if (CreatureIsDefending ())
			cost += defenseExtraCost;
		GameManager.instance.GetActivePlayer ().Spend (cost);

		StartCoroutine (selectedTile.creature.MoveToTarget (targetTile));
		selectedTile = targetTile;
	}

	private void DuplicateCreature()
	{
		CreatureController newCreature;

		// Spend money and duplicate the creature.
		int cost = duplicateCost;
		if (CreatureIsDefending ())
			cost += defenseExtraCost;
		GameManager.instance.GetActivePlayer ().Spend (cost);

		newCreature = selectedTile.creature.DuplicateToTarget (targetTile);
		StartCoroutine (newCreature.MoveToTarget (targetTile));

	}

	private void Attack ()
	{
		// Spend money.
		int cost = attackCost;
		if (CreatureIsDefending ())
			cost += defenseExtraCost;
		GameManager.instance.GetActivePlayer ().Spend (cost);

		StartCoroutine (selectedTile.creature.Attack (selectedTile, targetTile));
		selectedTile = targetTile;

	}

	private void ConvertCreature ()
	{
		// Spend money.
		int cost = convertingCost;
		if (CreatureIsDefending ())
			cost += defenseExtraCost;
		GameManager.instance.GetActivePlayer ().Spend (cost);

		StartCoroutine (selectedTile.creature.Convert (selectedTile, targetTile));
	}

	private void OppressCreature ()
	{
		// Spend money.
		int cost = oppressingCost;
		if (CreatureIsDefending ())
			cost += defenseExtraCost;
		GameManager.instance.GetActivePlayer ().Spend (cost);

		StartCoroutine (selectedTile.creature.Oppress (selectedTile, targetTile));
	}

	private IEnumerator PerformingActionState(bool creatureTarget)
	{
		GameManager.instance.panelControler.ShowCancelButton ();
		actualState = ActionState.PerformingAction;

		GameManager.instance.panelControler.DisableAllButtons ();

		if(!HighlightNeighbours (creatureTarget))
		{
			if(performing == ActionType.Attack || performing == ActionType.Convert || performing == ActionType.Oppress)
				GameManager.instance.panelControler.ShowMessage (3f, MessageType.NoEnemy);
			else
				GameManager.instance.panelControler.ShowMessage (3f, MessageType.CantPerformAction);

			GameManager.instance.panelControler.HideCancelButton ();
			GameManager.instance.panelControler.EnableAllButtons ();

			performing = ActionType.None;
			actualState = ActionState.WaitingActionCall;
		}
		else
		{
			while (targetTile == null) {
				yield return null;
			}

			GameManager.instance.panelControler.HideCancelButton ();

			switch (performing) {
			case ActionType.Move:
				MoveCreature ();
				break;
			case ActionType.Duplicate:
				DuplicateCreature ();
				break;
			case ActionType.Attack:
				Attack ();
				break;
			case ActionType.Convert:
				ConvertCreature ();
				break;
			case ActionType.Oppress:
				OppressCreature ();
				break;
			}

			StartCoroutine (WaitingActionFinishState ());
		}
	}

	private IEnumerator WaitingActionFinishState()
	{
		actualState = ActionState.WaitingActionFinish;

		GameManager.instance.ClearSelections ();

		while (actualState != ActionState.WaitingActionCall)
		{
			yield return null;
		}

		targetTile = null;
		performing = ActionType.None;


		GameManager.instance.panelControler.EnableAllButtons ();
		if(selectedTile.creature != null)
			selectedTile.Select ();
	}


	private bool HighlightNeighbours(bool withCreatues)
	{
		TileController neighbourTileController;
		bool hasHighlight = false;
		MessageType message = MessageType.CantPerformAction;

		foreach(GameObject neighbour in neighbours)
		{
			neighbourTileController = neighbour.GetComponent <TileController> ();

			if(neighbourTileController!=null)
			{
				if (withCreatues && neighbourTileController.creature!=null)
				{
					if(CreatureIsOppressed ())
					{
						if(neighbourTileController.creature.belongsToPlayer.playerNumber != selectedTile.creature.belongsToPlayer.playerNumber
							&& selectedTile.creature.oppressedByPlayer.playerNumber == GameManager.instance.activePlayerIndex
							&& neighbourTileController.creature.belongsToPlayer.playerNumber != GameManager.instance.activePlayerIndex)
						{
							neighbourTileController.Highlight ();
							hasHighlight = true;
						}
					}
					else
					{
						if(neighbourTileController.creature.belongsToPlayer.playerNumber == GameManager.instance.activePlayerIndex)
						{
							if (performing == ActionType.Convert) {
								if (neighbourTileController.creature.influencedByPlayer != null) {

									neighbourTileController.Highlight ();
									hasHighlight = true;
								}
							} 
							else if (performing == ActionType.Oppress) 
							{
								if (neighbourTileController.creature.oppressedByPlayer != null) 
								{
									neighbourTileController.Highlight ();
									hasHighlight = true;
								}
							}
						}
						else{
							neighbourTileController.Highlight ();
							hasHighlight = true;
						}
					}
				}
				else if(!withCreatues && neighbourTileController.creature==null)
				{
					if(CreatureIsOppressed ())
					{
						if(CreatureIsOppressedByActivePlayer(ref message) && performing!= ActionType.Duplicate)
						{
							neighbourTileController.Highlight ();
							hasHighlight = true;
						}
					} 
					else
					{
						neighbourTileController.Highlight ();
						hasHighlight = true;
					}
				}
			}
		}

		return hasHighlight;
	}

	private bool CanPerformAction(int actionCost, bool halfAction, bool needNeighbours)
	{
		MessageType message = MessageType.None;
		bool canPerformAction = HasSelectedTile (ref message) && HasEnoughtMoney (actionCost, ref message);

		if(canPerformAction)
		// Has a selected tile and enought money.
		{
			canPerformAction = HasSelectedCreature (ref message);

			if(canPerformAction)
			// Has a creature in selected tile.
			{
				if(CreatureIsOppressed ())
				{
					canPerformAction = CreatureIsOppressedByActivePlayer (ref message);
				}
				else
				{
					canPerformAction = CreatureBelongsToActivePlayer (ref message);
				}

				if (canPerformAction) 
				// Creature can be controlled by active player.
				{
					canPerformAction = !CreatureIsTired (halfAction, ref message);
				}

				if(canPerformAction && needNeighbours)
				// Creature is not tired.
				{
					canPerformAction = CreatureHasNeighbours (ref message);
				}
			}
		}
		if(!canPerformAction)
		{
			GameManager.instance.panelControler.ShowMessage (3f, message);
		}



		return canPerformAction;
	}

	private bool HasSelectedTile(ref MessageType message)
	{
		GameObject tile;
		bool check = GameManager.instance.boardScript.SelectedTile (out tile);

		if (!check)
		{
			if(message == MessageType.None)
			{
				message = MessageType.SelectTileFirst;
			}
			return false;
		}
		selectedTile = tile.GetComponent <TileController> ();

		return true;
	}

	private bool HasEnoughtMoney(int actionCost, ref MessageType message)
	{
		if(CreatureIsDefending ())
		{
			actionCost += defenseExtraCost;
		}

		bool check = GameManager.instance.GetActivePlayer ().coinCount >= actionCost;

		if(!check && message == MessageType.None)
		{
			message = MessageType.NotEnoughtMoney;
		}

		return check;
	}

	private bool HasSelectedCreature(ref MessageType message)
	{
		bool check = selectedTile.creature != null;

		if(!check && message == MessageType.None)
		{
			message = MessageType.NoCreatureThere;
		}

		return check;
	}

	private bool CreatureBelongsToActivePlayer(ref MessageType message)
	{
		bool check = selectedTile.creature.belongsToPlayer.playerNumber == GameManager.instance.activePlayerIndex;

		if(!check && message == MessageType.None)
		{
			message = MessageType.NotYourCreature;
		}

		return check;
	}

	private bool CreatureIsOppressed()
	{
		return selectedTile.creature.oppressedByPlayer != null;
	}

	private bool CreatureIsOppressedByActivePlayer(ref MessageType message)
	{
		bool check = selectedTile.creature.oppressedByPlayer.playerNumber == GameManager.instance.activePlayerIndex;

		if(!check && message == MessageType.None)
		{
			message = MessageType.NotYourCreature;
		}

		return check;
	}

	private bool CreatureIsTired(bool halfAction, ref MessageType message)
	{
		bool check = false;

		if (selectedTile.creature.isTired)
			check = true;
		
		if (!check && !halfAction)
			check = selectedTile.creature.moved;

		if(check && message == MessageType.None)
		{
			message = MessageType.CreatureTooTired;
		}

		return check;
	}

	private bool CreatureHasNeighbours(ref MessageType message)
	{
		bool check = GameManager.instance.boardScript.hasNeighbours (selectedTile.xIndex, selectedTile.zIndex, out neighbours);

		if(!check && message == MessageType.None)
		{
			message = MessageType.CantPerformAction;
		}

		return check;
	}

	private bool CreatureIsDefending()
	{
		bool check = selectedTile.creature.IsDefending ();

		if (performing == ActionType.Defend)
			check = false;
			
		return check;
	}
}
