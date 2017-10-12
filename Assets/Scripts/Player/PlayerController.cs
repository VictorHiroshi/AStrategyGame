//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class to control player informations.
[System.Serializable]
public class PlayerController : ScriptableObject{
	public Color color;
	public List<CreatureController> controlledCreatures;
	public List<CreatureController> oppressedCreatures;
	public List<CreatureController> attemptingToConvert;
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

	public bool CheckIfLost()
	{
		// TODO: Check if lost game, if yes, reassign oppressed creatures and turn of attempting to convert creatures.
		return false;
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

	public void CreatureGetOppressed(CreatureController creature)
	{
		if (controlledCreatures.Contains (creature)) {
			controlledCreatures.Remove (creature);
			oppressedCreatures.Add (creature);
		}
	}

	public void GetBackOppressedCreature(CreatureController creature)
	{
		if (oppressedCreatures.Contains (creature))
		{
			oppressedCreatures.Remove (creature);
			controlledCreatures.Add (creature);
		}
	}
}
