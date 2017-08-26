using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DialogScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	private Image balloon;
	private Text dialogMessage;

	void Awake()
	{
		balloon = GetComponent <Image> ();
		dialogMessage = GetComponentInChildren <Text> ();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
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
}
