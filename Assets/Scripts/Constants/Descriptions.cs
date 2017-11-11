using System;
using UnityEngine;


public class Descriptions
{
	public static string RESOURCE_STONE = "Stones increase the total income when within your territory. You can exploit then to get more coins.";

	public static string NO_DESCRIPTION = "=)";
	public static string HEALING = String.Format("Cost: {0} coin!\nClick here to heal this creature.", ActionsManager.instance.healingCost);

	public static string MOVE = String.Format("Cost: {0} coin!\nAction to move your creature around the board. Remember that once a criature leaves a tile, it's no longer part of your territory.", ActionsManager.instance.movingCost);
	public static string DUPLICATE = String.Format("Cost: {0} coins!\nAction to expand your therritory with new creatures.", ActionsManager.instance.duplicateCost);

	public static string LIGHT_EXPLOIT = String.Format("Cost: {0} coin!\nAction to exploit resources gathering just as much as mother nature gives you.", ActionsManager.instance.exploitCost);
	public static string HEAVY_EXPLOIT = String.Format("Cost: {0} coin!\nAction to exploit resources massively. It wil destroy the resource soon or later. Be careful!", ActionsManager.instance.exploitCost);

	public static string ATTACK = String.Format("Cost: {0} coins!\nAction to attack an opponent. This destroys his creature and moves yours to take his place.", ActionsManager.instance.attackCost);
	public static string CONVERT = String.Format("Cost: {0} coins!\nAction to convert an opponent. This keeps the other creature alive, but it will from now on help you. You won't control its actions, though.", ActionsManager.instance.convertingCost);
	public static string OPPRESS = String.Format("Cost: {0} coins!\nAction to oppress an opponent. This keeps the other creature alive and you get control of it for a couple turns.", ActionsManager.instance.oppressingCost);
	public static string DEFEND = String.Format("Cost: {0} coin!\nIf your creature is attacked while defending, it will lose only half of it's health. If you select this option with a defending creature, it will turn off the defense!", ActionsManager.instance.defenseCost);

	public static string NEXT_TURN_BUTTON = "ATTENTION!\nThis will take you to the next turn!";
	public static string COINS = "This is your money. Use it to pay creatures to perform actions. Each turn you'll receive some more coins. You can get then by exploring resources, by the way.";

	public static string UNAVALIABLE_ACTION = "It's not possible to execute this action now!";
	public static string TOO_TIRED_MAAN = "This creature is too tired, ma'am. Let it rest a turn!";
	public static string SELECT_A_TILE = "Select a tile before performing any action.";
	public static string NOT_YOUR_CREATURE = "This is not your creature, fella!";
	public static string NO_ENEMY = "Chill out! There is no enemy in range";
	public static string NO_MONEY = "You don't have enought money for this action now. :(";
	public static string NO_CREATURE_THERE = "There's no creature in this tile, ma'am!";
	public static string NO_STONE = "I have nothing to gather here, dude!";
}
