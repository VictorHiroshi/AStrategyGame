using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIHighlightController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	private bool highlighted = false;

	public void OnPointerEnter(PointerEventData eventData)
	{
		highlighted = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		highlighted = false;
	}

	public bool IsHighligted()
	{
		return highlighted;
	}
}
