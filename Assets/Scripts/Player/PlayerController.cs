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

	public float controlledCreaturesPoints = 2.5f;
	public float creatureGetOppressedDiscountPoints = 2f;
	public float partialyConvertedCreaturePoints = 1.5f;
	public float pointsPerCoins = 0.1f;
	public float pointsPerStones = 10f;

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
		bool lost = false;

		if(controlledCreatures.Count == 0 && oppressedCreatures.Count == 0)
		{
			lost = true;
			foreach(CreatureController creature in attemptingToConvert)
			{
				creature.ConvertingTeamLost();
			}
		}

		return lost;
	}

	public void InstantiateCreature(TileController tile)
	{
		tile.InstantiateCreature (this);
	}

	public void ControllCreature(CreatureController creature)
	{
		if (!controlledCreatures.Contains (creature)) {
			controlledCreatures.Add (creature);

			if (creature.occupiedTile.resource != null) {
				controlledStones++;
			}
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
		else if(oppressedCreatures.Contains (creature)){
			oppressedCreatures.Remove (creature);

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

	public float GetActualStateCost(bool normalizeByOtherPlayers = true)
	{
		float stateCost = 0f;

		stateCost += controlledStones * pointsPerStones;
		stateCost += attemptingToConvert.Count * partialyConvertedCreaturePoints;
		stateCost -= oppressedCreatures.Count * creatureGetOppressedDiscountPoints;

		if(normalizeByOtherPlayers)
		{
			stateCost += (controlledCreatures.Count / GameManager.instance.GetTotalCreaturesFromActualState ())
																					* controlledCreaturesPoints;

			stateCost += (coinCount / GameManager.instance.GetTotalCoinsFromActualState ()) * pointsPerCoins;
		}
		else
		{
			stateCost += controlledCreatures.Count * controlledCreaturesPoints;
			stateCost += coinCount * pointsPerCoins;
		}

		return stateCost;
	}
}
