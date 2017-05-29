using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles each tile individually.
// This script controlls sofar the selection and unselection of each tile and its neighbours.

public class TileController : MonoBehaviour {

	[HideInInspector] public GameObject creature;
	[HideInInspector] public GameObject resource;
	[HideInInspector] public enum ResourceType {None, Stone, Barrier, Tree};
	[HideInInspector] public ResourceType resourceType = ResourceType.None;

	public Transform spawnPoint;

	private int xIndex, zIndex;
	private bool selected;
	private GameObject highlightObject;
	private GameObject highlightInstance;
	private Vector3 instantiatingPosition;

	void Start(){
		
		selected = false;

		float tileSize = GameManager.instance.boardScript.tiles.tileSideSize;
		instantiatingPosition = new Vector3 (zIndex * tileSize, 0f, xIndex * tileSize);
		highlightObject = GameManager.instance.tileHighlightObject;

	}


	// Tests clicks only for the tile on which the mouse is over.
	void OnMouseOver()
	{
		
		// Toggle the selection of the current tile to true or false.
		if (Input.GetMouseButtonDown (0)) {
			selected = !selected;

			Debug.Log ("tile " + GetStringID () + "selected = " + selected); 

			// If this tile is selected, highlight the neigbhours.
			if (selected) {

				List<GameObject> neighbours = new List<GameObject> ();

				GameManager.instance.boardScript.hasNeighbours (xIndex, zIndex, out neighbours);
				GameManager.instance.boardScript.SetSelectedTile (this.gameObject);

				foreach (GameObject neighbour in neighbours) {

					TileController controller = neighbour.GetComponent<TileController> ();

					if (controller != null) {
						controller.Highlight ();
					}
				}

				Highlight();
			}

			//If the tile was selected and then clicked, it will be unselected
			else {
				// TODO: Fix it. Still not working. 
				Debug.Log ("Unselecting " + xIndex + " " + zIndex);
				Unselect ();
				GameManager.instance.boardScript.SetSelectionToNull();
			}
		}
	}
		
	// Static function to get formated string ID for the given indexes.
	public static string GetStringID(int x, int z)
	{
		return(x + ", " + z);
	}

	// Returns the formated string ID for the current tile.
	public string GetStringID()
	{
		return(xIndex + ", " + zIndex);
	}

	// Informs this tile about it's indexes in the boardgame.
	public void SetID(int x, int z)
	{
		xIndex = x;
		zIndex = z;
	}

	// Instantiates the highlight object over this tile.
	public void Highlight()
	{
		highlightInstance = Instantiate(highlightObject, instantiatingPosition, Quaternion.identity);
	}

	// Delete highlight object from this tile.
	public void UnHighlight()
	{
		if (highlightInstance != null) {
			Destroy (highlightInstance);
		}
	}

	// Remove selection from current tile and it's neighbours.
	public void Unselect()
	{
		//Tests if the tile is selected, before unselecting it
		if (selected) {
			selected = false;
		}

		List<GameObject> neighbours = new List<GameObject> ();
		GameManager.instance.boardScript.hasNeighbours (xIndex, zIndex, out neighbours);
		GameManager.instance.boardScript.SetSelectionToNull();

		foreach (GameObject neighbour in neighbours) {
			TileController controller = neighbour.GetComponent<TileController> ();
			if (controller != null) {
				controller.UnHighlight ();
			}
		}

		UnHighlight ();
	}
		
	// Receives a game object and instantiates it as a creature.
	public void InstantiateCreature(GameObject creature)
	{
		this.creature = Instantiate (creature, spawnPoint.position, Quaternion.identity);
	}

	// Receives a gameobject and instantiates it as a resource.
	public void InstantiateResource(GameObject resource)
	{
		this.resource = Instantiate (resource, transform, false);
	}
}
