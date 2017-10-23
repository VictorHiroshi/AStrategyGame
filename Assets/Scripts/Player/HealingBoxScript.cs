using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingBoxScript : MonoBehaviour {

	CreatureController parent;

	void Start()
	{
		parent = GetComponentInParent <CreatureController> ();
	}

	void OnMouseDown()
	{
		if (parent.belongsToPlayer.playerNumber == GameManager.instance.activePlayerIndex)
		{
			if(GameManager.instance.GetActivePlayer ().coinCount < ActionsManager.instance.healingCost)
			{
				GameManager.instance.panelControler.ShowMessage (3f, MessageType.NotEnoughtMoney);
				return;
			}
			StopAllCoroutines ();
			StartCoroutine (parent.Heal ());
		}
		else
		{
			GameManager.instance.panelControler.ShowMessage (3f, MessageType.NotYourCreature);
		}
	}

	void OnMouseEnter()
	{
		GameManager.instance.panelControler.ShowMessage (3f, MessageType.Healing);

		if (parent.belongsToPlayer.playerNumber == GameManager.instance.activePlayerIndex) 
		{
			StopAllCoroutines ();
			StartCoroutine (parent.dialogCanvas.DisplayMessageForTime ("Please, dude! Only one buck..."));
		}
	}
}
