using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class to control player informations.
[System.Serializable]
public class PlayerController {
	public Color color;
	public List<string> controlledTiles;
	public GameObject creature;
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
		// TODO: Verify if new tile has stones in it to add to controlled stones and add tile to controlledTiles
		controlledTiles.Add (TileController.GetStringID (newTile.xIndex, newTile.zIndex));
		if(newTile.resource!=null)
		{
			controlledStones++;
		}
	}

	public void LeaveTile(TileController oldTile)
	{
		// TODO: Verify if old tile had stones in it to remove it from controlled stones and remove tile from controlledTiles
		controlledTiles.Remove (TileController.GetStringID (oldTile.xIndex, oldTile.zIndex));
		if(oldTile.resource!=null)
		{
			controlledStones--;
		}
	}
}
