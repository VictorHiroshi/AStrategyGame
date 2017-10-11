//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class to control player informations.
[System.Serializable]
public class PlayerController : ScriptableObject{
	public Color color;
	public List<CreatureController> controlledCreatures;
	public List<CreatureController> oppressedEnemyCreatures;
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

	public void InstantiateCreature(TileController tile)
	{
		tile.InstantiateCreature (this);
	}

	public void ControllCreature(CreatureController creature)
	{
		controlledCreatures.Add (creature);

		if(creature.occupiedTile.resource != null)
		{
			controlledStones++;
		}
	}

	public void LoseCreature(CreatureController creature)
	{
		if (controlledCreatures.Contains (creature)) {
			controlledCreatures.Remove (creature);

			if (creature.occupiedTile.resource != null) {
				controlledStones--;
			}
		}
	}
/*	public void ControllNewTile(CreatureController creature)
	{
		controlledTiles.Add (creature);
		if(newTile.resource!=null)
		{
			controlledStones++;
		}
	}

	public void LeaveTile(CreatureController creature)
	{
		controlledTiles.Remove (creature);
		if(oldTile.resource!=null)
		{
			controlledStones--;
		}
	}*/
}
