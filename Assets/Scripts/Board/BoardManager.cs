﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// Class to organize all sorts of tiles.
[System.Serializable]
public class TileLists{
	public GameObject[] normalTiles;
	public GameObject[] rightEdgeTiles;
	public GameObject[] leftEdgeTiles;
	public GameObject[] upEdgeTiles;
	public GameObject[] downEdgeTiles;
	public GameObject[] upperLeftCornerTile;
	public GameObject[] upperRightCornerTile;
	public GameObject[] lowerLeftCornerTile;
	public GameObject[] lowerRightCornerTile;
	public float tileSideSize = 3.0f;
}

[System.Serializable]
public class InitialTile{
	public int coordX;
	public int coordY;
}

[System.Serializable]
public class ListOfInitialTile{
	public List<InitialTile> initialTiles;
}

// Class to organize all sorts of resources.
[System.Serializable]
public class ResourceList{
	public int resourceFadigue;
	public GameObject[] stones;
	public GameObject[] trees;
	public GameObject[] barriers;
}

// Script to handle general operations of the boardgame.
public class BoardManager : MonoBehaviour {

	public int maxResourceGenerationAttempts = 20;
	public int boardSize = 16;
	public int stonesPerQuadrant = 5;
	public TileLists tiles;
	public ResourceList resources;
	public ParticleSystem explosionParticles;
	public ParticleSystem rockExplorationParticles;

	public List<ListOfInitialTile> intialTilesForPlayer;
	public List<Coordinate> stones;

	private Transform boardHolder;
	private GameObject selectedTile;
	private bool existsSelectedTile;

	private Dictionary<string, GameObject> boardGameByID;

	// Creates a new boardgame with all the necessary setups to start a new game.
	public void SetupScene(){
		BoardSetup ();
		CreateResources ();

		selectedTile = null;
		existsSelectedTile = false;
	}

	// Tests if there's any selected tile in the game.
	// If true, returns the selected tile with the out parameter.
	public bool SelectedTile(out GameObject tile)
	{
		tile = selectedTile;
		return existsSelectedTile;
	}

	// Informs the manager that this specific tile is the selected one.
	public void SetSelectedTile(GameObject tile)
	{
		//If another tile was already selected, unselect it
		if (selectedTile != null) {
			TileController controller = selectedTile.GetComponent<TileController> ();
			if (controller != null) {
				controller.Unselect ();
			}
		}
		selectedTile = tile;
		existsSelectedTile = true;
	}

	// Informs the manager that there's no tile selected in the game anymore.
	public void SetSelectionToNull()
	{
		selectedTile = null;
		existsSelectedTile = false;
	}

	// Given an ID, returns the controller of the tile, if exists.
	public TileController getTile(int x, int z)
	{
		string id = TileController.GetStringID (x, z);
		TileController tileInstance = boardGameByID [id].GetComponent <TileController>();
		return tileInstance;
	}

	// Tests if the tile has neighbours in the boardgame.
	// If true, populates the neighbours list with it's neighbours.
	public bool hasNeighbours(int x, int z, out List<GameObject> neighbours)
	{

		neighbours = new List<GameObject> ();

		if (x < 0 || x >= boardSize || z < 0 || z >= boardSize)
			return false;

		GameObject neighbourTile;

		// Add left neighbour to the list.
		if (x > 0){
			neighbourTile = boardGameByID [TileController.GetStringID (x - 1, z)];
			neighbours.Add(neighbourTile);
		}

		// Add right neighbour to the list.
		if (x < boardSize - 1) {
			neighbourTile = boardGameByID [TileController.GetStringID (x + 1, z)];
			neighbours.Add(neighbourTile);
		}

		// Add upper neighbour to the list.
		if (z > 0) {
			neighbourTile = boardGameByID [TileController.GetStringID (x, z - 1)];
			neighbours.Add(neighbourTile);
		}

		// Add lower neighbour to the list.
		if (z < boardSize - 1) {
			neighbourTile = boardGameByID [TileController.GetStringID (x, z + 1)];
			neighbours.Add (neighbourTile);
		}

		return true;
	}

	// Instantiates all tiles of the boardgame.
	private void BoardSetup()
	{
		boardGameByID = new Dictionary<string, GameObject> ();

		boardHolder = new GameObject ("Board").transform;

		for (int z = -1; z < boardSize + 1; z++) 
		{
			for (int x = -1; x < boardSize + 1; x++)
			{
				GameObject toInstantiate;
				if (z == -1) {
					if (x == -1)
						toInstantiate = tiles.upperRightCornerTile [Random.Range (0, tiles.upperRightCornerTile.Length)];
					else if (x == boardSize)
						toInstantiate = tiles.lowerRightCornerTile [Random.Range (0, tiles.lowerRightCornerTile.Length)];
					else
						toInstantiate = tiles.rightEdgeTiles [Random.Range (0, tiles.rightEdgeTiles.Length)];
				} else if (z == boardSize) {
					if (x == -1)
						toInstantiate = tiles.upperLeftCornerTile [Random.Range (0, tiles.upperLeftCornerTile.Length)];
					else if (x == boardSize)
						toInstantiate = tiles.lowerLeftCornerTile [Random.Range (0, tiles.lowerLeftCornerTile.Length)];
					else
						toInstantiate = tiles.leftEdgeTiles [Random.Range (0, tiles.leftEdgeTiles.Length)];
				} else if (x == -1) {
					toInstantiate = tiles.upEdgeTiles [Random.Range (0, tiles.upEdgeTiles.Length)];
				} else if (x == boardSize) {
					toInstantiate = tiles.downEdgeTiles [Random.Range (0, tiles.downEdgeTiles.Length)];
				} else {
					toInstantiate = tiles.normalTiles [Random.Range (0, tiles.normalTiles.Length)];
				}

				GameObject instance = Instantiate (toInstantiate, new Vector3 (z * tiles.tileSideSize, 0f, x * tiles.tileSideSize), Quaternion.identity) as GameObject;
				instance.transform.SetParent (boardHolder);

				TileController tile = instance.GetComponent<TileController> ();

				// Tests is this tile has a TileController script attached.
				if (tile != null) {
					// If it is a playable tile (with TileController), add it to the list of active tiles.
					tile.SetID (x, z);
					boardGameByID.Add (tile.GetStringID (), instance);
				}

			}
		}
	}

	// Generates resources for some tiles on the boardgame.
	// The boardgame is divided into four quadrants to balance the resource distribution.
	private void CreateResources ()
	{
		int minX;
		int maxX;
		int minZ;
		int maxZ;

		stones = new List<Coordinate> ();
		// First quadrant.
		minX = 1;
		maxX = ((boardSize + 1) / 2);
		minZ = 1;
		maxZ = ((boardSize + 1)/ 2);
		GenerateAllRocks (minX, maxX, minZ, maxZ);

		// Second quadrant.
		minX = ((boardSize + 1) / 2);
		maxX = boardSize - 1;
		minZ = 1;
		maxZ = ((boardSize + 1) / 2);
		GenerateAllRocks (minX, maxX, minZ, maxZ);

		// Third quadrant.
		minX = ((boardSize + 1) / 2);
		maxX = boardSize - 1;
		minZ = ((boardSize + 1) / 2);
		maxZ = boardSize - 1;
		GenerateAllRocks (minX, maxX, minZ, maxZ);

		// Fourth quadrant.
		minX = 1;
		maxX = ((boardSize + 1) / 2);
		minZ = ((boardSize + 1)/ 2);
		maxZ = boardSize - 1;
		GenerateAllRocks (minX, maxX, minZ, maxZ);

	}

	// Creates the number of stones per quadrant, receiving the boundary indexes of the quadrant.
	private void GenerateAllRocks(int minXIndex, int maxXIndex, int minZIndex, int maxZIndex)
	{
		int xIndex;
		int zIndex;
		GameObject stoneModel;
		Coordinate stoneCoord;

		for(int i=0, attempt = 0; i<stonesPerQuadrant && attempt < maxResourceGenerationAttempts; attempt++){
			xIndex = Random.Range (minXIndex, maxXIndex);
			zIndex = Random.Range (minZIndex, maxZIndex);
			TileController tileInstance = boardGameByID [TileController.GetStringID (xIndex, zIndex)].GetComponent <TileController> ();


			if(tileInstance!=null && tileInstance.resourceType == TileController.ResourceType.None){
				i++;
				stoneModel = resources.stones [Random.Range (0, resources.stones.Length)];
				tileInstance.InstantiateResource (stoneModel);
				tileInstance.resourceType = TileController.ResourceType.Stone;

				stoneCoord = new Coordinate (xIndex, zIndex);
				stones.Add (stoneCoord);
			}
		}
	}

}
