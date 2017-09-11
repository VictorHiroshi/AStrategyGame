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
			StartCoroutine (parent.Heal ());
			parent.dialogCanvas.DisplayMessageForTime ("Thanks, ma'am!");
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
			parent.dialogCanvas.DisplayMessageForTime ("Please, dude! Only one buck...");
		}
	}
}
