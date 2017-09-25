//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class to control player informations.
[System.Serializable]
public class PlayerController : ScriptableObject{
	public Color color;
	public List<string> controlledTiles;
	//public GameObject creature;
	public int coinCount;
	public int playerNumber;
	public int controlledStones;

	public void Spend(int money)
	{
		coinCount -= money;
		GameManager.instance.panelControler.updateCoins (coinCount);
	}

	public void Receive(int money)
	{
		coinCount += money;
		GameManager.instance.panelControler.updateCoins (coinCount);
	}

	public void ControllNewTile(TileController newTile)
	{
		controlledTiles.Add (TileController.GetStringID (newTile.xIndex, newTile.zIndex));
		if(newTile.resource!=null)
		{
			controlledStones++;
		}
	}

	public void LeaveTile(TileController oldTile)
	{
		controlledTiles.Remove (TileController.GetStringID (oldTile.xIndex, oldTile.zIndex));
		if(oldTile.resource!=null)
		{
			controlledStones--;
		}
	}
}
