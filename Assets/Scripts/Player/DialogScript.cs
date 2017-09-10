using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DialogScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public Image balloon;
	public Text dialogMessage;

	void Awake()
	{
		TurnBalloonVisibility (false);
	}
		

	public void OnPointerEnter(PointerEventData eventData)
	{
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
	}

	public void TurnBalloonVisibility (bool isVisible)
	{
		balloon.gameObject.SetActive (isVisible);
		dialogMessage.gameObject.SetActive (isVisible);
	}


	public void DisplayMessageForTime(string message, float deltaTime = 2.5f)
	{
		dialogMessage.text = message;
		TurnBalloonVisibility (true);
		StartCoroutine (Display (new WaitForSeconds (deltaTime)));
	}

	private IEnumerator Display(WaitForSeconds delay)
	{
		yield return delay;
		TurnBalloonVisibility (false);
	}
}
