using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// Script to handle general operations of the boardgame.

public class BoardManager : MonoBehaviour {

	public int columns = 20;
	public int rows = 20;
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

	private Transform boardHolder;
	private GameObject selectedTile;
	private bool existsSelectedTile;

	private Dictionary<string, GameObject> boardGameByID;


	void Awake()
	{
		GetComponent <BoxCollider> ().size=(new Vector3(columns*tileSideSize+6, 0, rows*tileSideSize+6));
		GetComponent <BoxCollider> ().transform.position = (new Vector3 ((columns * tileSideSize/2)-tileSideSize, 0,
																				(rows * tileSideSize/2)-tileSideSize));

		existsSelectedTile = false;
	}
		
	// Instantiates all tiles of the board game.
	void BoardSetup()
	{

		boardGameByID = new Dictionary<string, GameObject> ();

		boardHolder = new GameObject ("Board").transform;

		for (int z = -1; z < columns + 1; z++) 
		{
			for (int x = -1; x < rows + 1; x++)
			{
				GameObject toInstantiate;
				if (z == -1) {
					if (x == -1)
						toInstantiate = upperRightCornerTile [Random.Range (0, upperRightCornerTile.Length)];
					else if (x == rows)
						toInstantiate = lowerRightCornerTile [Random.Range (0, lowerRightCornerTile.Length)];
					else
						toInstantiate = rightEdgeTiles [Random.Range (0, rightEdgeTiles.Length)];
				} else if (z == columns) {
					if (x == -1)
						toInstantiate = upperLeftCornerTile [Random.Range (0, upperLeftCornerTile.Length)];
					else if (x == rows)
						toInstantiate = lowerLeftCornerTile [Random.Range (0, lowerLeftCornerTile.Length)];
					else
						toInstantiate = leftEdgeTiles [Random.Range (0, leftEdgeTiles.Length)];
				} else if (x == -1) {
					toInstantiate = upEdgeTiles [Random.Range (0, upEdgeTiles.Length)];
				} else if (x == rows) {
					toInstantiate = downEdgeTiles [Random.Range (0, downEdgeTiles.Length)];
				} else {
					toInstantiate = normalTiles [Random.Range (0, normalTiles.Length)];
				}

				GameObject instance = Instantiate (toInstantiate, new Vector3 (z * tileSideSize, 0f, x * tileSideSize), Quaternion.identity);
				instance.transform.SetParent (boardHolder);

				TileController tile = instance.GetComponent<TileController> ();

				
				if (tile != null) {
					tile.SetID (x, z);
					boardGameByID.Add (tile.GetStringID (), instance);
				}
					
			}
		}
	}

	public void SetupScene(){
		BoardSetup ();
		// TODO: add creatures
		//		 add resources
		//		 setup HUD.
	}

	public bool SelectedTile(out GameObject tile)
	{
		tile = selectedTile;
		return existsSelectedTile;
	}

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
	}

	public void SetSelectionToNull()
	{
		selectedTile = null;
	}

	public bool hasNeighbours(int x, int z, out List<GameObject> neighbours)
	{
		// UNDONE

		neighbours = new List<GameObject> ();

		if (x < 0 || x >= rows || z < 0 || z >= columns)
			return false;

		GameObject nighbourTile;

		// Add left neighbour to the list.
		if (x > 0){
			nighbourTile = boardGameByID [TileController.GetStringID (x - 1, z)];
			neighbours.Add(nighbourTile);
		}

		// Add right neighbour to the list.
		if (x < rows - 1) {
			nighbourTile = boardGameByID [TileController.GetStringID (x + 1, z)];
			neighbours.Add(nighbourTile);
		}

		// Add upper neighbour to the list.
		if (z > 0) {
			nighbourTile = boardGameByID [TileController.GetStringID (x, z - 1)];
			neighbours.Add(nighbourTile);
		}

		// Add lower neighbour to the list.
		if (z < columns - 1) {
			nighbourTile = boardGameByID [TileController.GetStringID (x, z + 1)];
			neighbours.Add (nighbourTile);
		}

		return true;
	}
}
