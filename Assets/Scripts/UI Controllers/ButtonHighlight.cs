using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonHighlight : MonoBehaviour {
	private bool highlighted = false;

	void OnPointerEnter(PointerEventData eventData)
	{
		Debug.Log ("ButtonHighlight");
		highlighted = true;
	}

	void OnMouseExit()
	{
		highlighted = false;
	}

	public bool IsHighligted()
	{
		return highlighted;
	}
}
