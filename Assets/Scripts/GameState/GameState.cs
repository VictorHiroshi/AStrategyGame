using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameState : ScriptableObject {
	public int boardSize;
	public int activePlayer;
	public int playersOnBoard;
	public int turnCount;
	public int[] playerCoins;
	public float[] playerPoints;
	public List<Coordinate> stones;
	public List<Creature> creatures;

	public GameState(int boardSize, int activePlayer, int playersOnBoard, int turnCount, List<Coordinate> stones)
	{
		creatures = new List<Creature> ();

		this.boardSize = boardSize;
		this.activePlayer = activePlayer;
		this.playersOnBoard = playersOnBoard;
		this.turnCount = turnCount;
		this.stones = new List<Coordinate>(stones);

		playerCoins = new int[playersOnBoard];
		playerPoints = new float[playersOnBoard];
	}

	public GameState(GameState original)
	{
		creatures = new List<Creature> (original.creatures);

	}
}
