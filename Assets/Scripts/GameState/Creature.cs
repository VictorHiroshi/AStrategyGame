using System;
using UnityEngine;
using System.Runtime.ConstrainedExecution;

public enum CreatureSpecies {Slime, Bat, Ghost, Rabbit};

[System.Serializable]
public class Creature : ScriptableObject{

	public static int nextID = 0;

	protected int ID;

	public CreatureSpecies specie;
	public int belongsToPlayer;
	public int influencedByPlayer;
	public int oppressedByPlayer;
	public bool isTired;
	public bool halfTired;
	public Coordinate coordinate;

	public Creature(CreatureSpecies specie)
	{
		ID = nextID;
		nextID ++;

		this.specie = specie;
	}


	public override bool Equals(object obj)
	{
		if(obj == null)
		{
			return false;
		}

		Creature other = obj as Creature;

		if((System.Object)other == null)
		{
			return false;
		}

		return ID == other.ID;
	}

	public bool Equals(Creature other)
	{
		if((object)other == null)
		{
			return false;
		}

		return ID == other.ID;
	}
}

