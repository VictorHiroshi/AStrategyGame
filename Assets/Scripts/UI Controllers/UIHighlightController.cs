using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIHighlightController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	public bool isControlPanel = false;
	private bool highlighted = false;

	public void OnPointerEnter(PointerEventData eventData)
	{
		highlighted = true;
		if(isControlPanel)
		{
			GameManager.instance.m_camera.canMove = false;
			GameManager.instance.m_camera.canZoom = false;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		highlighted = false;
		if(isControlPanel)
		{
			GameManager.instance.m_camera.canMove = true;
			GameManager.instance.m_camera.canZoom = true;
		}
	}

	public bool IsHighligted()
	{
		return highlighted;
	}
}
