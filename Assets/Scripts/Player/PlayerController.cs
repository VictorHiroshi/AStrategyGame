using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class to control player informations.
public class PlayerController {
	public Color color;
	public List<string> controlledTiles;
	public GameObject creature;
	public int coinCount;
	public int playerNumber;

	public void Spend(int money)
	{
		coinCount -= money;
		GameManager.instance.panelControler.updateCoins (coinCount);
	}
}
