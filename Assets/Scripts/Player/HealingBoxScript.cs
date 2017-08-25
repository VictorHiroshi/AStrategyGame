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
		StartCoroutine (parent.Heal ());
	}

	void OnMouseOver()
	{
		// TODO: Make creature plead for healing.

	}
}
