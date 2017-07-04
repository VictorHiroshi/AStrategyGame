using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Resource : MonoBehaviour {
	public int resourceCount;


	public void Awake ()
	{
		int maxValue = GameManager.instance.boardScript.resources.resourceFadigue;
		resourceCount = Random.Range (1, maxValue);
	}
}

