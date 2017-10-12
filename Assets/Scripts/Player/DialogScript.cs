using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DialogScript : MonoBehaviour {

	public Image balloon;
	public Text dialogMessage;

	private BoxCollider boxCollider;

	void Awake()
	{
		boxCollider = GetComponent <BoxCollider> ();
		TurnBalloonVisibility (false);
	}
		

/*	public void OnPointerEnter(PointerEventData eventData)
	{
		Debug.Log ("pointerEnter");
		// TODO: Check why is not being called.
		Color temp = balloon.color;
		temp.a = temp.a/4;
		balloon.color = temp;

		temp = dialogMessage.color;
		temp.a = temp.a/4;
		dialogMessage.color = temp;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		Color temp = balloon.color;
		temp.a = temp.a*4;
		balloon.color = temp;

		temp = dialogMessage.color;
		temp.a = temp.a*4;
		dialogMessage.color = temp;
	}*/

	public void OnMouseEnter()
	{
		Color temp = balloon.color;
		temp.a = temp.a/4;
		balloon.color = temp;

		temp = dialogMessage.color;
		temp.a = temp.a/4;
		dialogMessage.color = temp;
	}

	public void OnMouseExit()
	{
		Color temp = balloon.color;
		temp.a = temp.a*4;
		balloon.color = temp;

		temp = dialogMessage.color;
		temp.a = temp.a*4;
		dialogMessage.color = temp;

	}

	public void TurnBalloonVisibility (bool isVisible)
	{
		boxCollider.enabled = isVisible;

		balloon.gameObject.SetActive (isVisible);
		dialogMessage.gameObject.SetActive (isVisible);

		Color temp = balloon.color;
		temp.a = 1f;
		balloon.color = temp;

		temp = dialogMessage.color;
		temp.a = 1f;
		dialogMessage.color = temp;
	}


	public IEnumerator DisplayMessageForTime(string message, float deltaTime = 2.5f)
	{
		TurnBalloonVisibility (true);
		dialogMessage.text = message;
		yield return new WaitForSeconds (deltaTime);
		TurnBalloonVisibility (false);
	}
}
