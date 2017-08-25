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
		Debug.Log ("Clicked Healing Box!");
		parent.animatorController.SetTrigger ("Moves");
	}

	void OnMouseOver()
	{
		// TODO: Make creature plead for healing.
	}
}
