using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles each tile individually.
// This script controlls sofar the selection and unselection of each tile and its neighbours.

public class TileController : MonoBehaviour {

	public Transform spawnPoint;

	[HideInInspector] public CreatureController creature;
	[HideInInspector] public GameObject resource;
	[HideInInspector] public enum ResourceType {None, Stone, Barrier, Tree};
	[HideInInspector] public ResourceType resourceType = ResourceType.None;
	[HideInInspector] public int xIndex, zIndex;

	private bool selected;
	private bool isHighlighted;
	private GameObject highlightObject;
	private GameObject highlightInstance;
	private Vector3 instantiatingPosition;

	void Start(){
		
		selected = false;
		isHighlighted = false;

		float tileSize = GameManager.instance.boardScript.tiles.tileSideSize;
		instantiatingPosition = new Vector3 (zIndex * tileSize, 0f, xIndex * tileSize);
		highlightObject = GameManager.instance.tileHighlightObject;

	}


	// Tests clicks only for the tile on which the mouse is over.
	void OnMouseOver()
	{
		
		// Toggle the selection of the current tile to true or false.
		if (Input.GetMouseButtonDown (0)) {
			if(ActionsManager.instance.actualState == ActionState.WaitingActionCall){
				selected = !selected;

				// If this tile is selected, highlight the neigbhours.
				if (selected) {
					Select ();
				}

			//If the tile was selected and then clicked, it will be unselected
			else {
					Unselect ();
				}
			}
			else if(ActionsManager.instance.actualState == ActionState.PerformingAction)
			{
				if(isHighlighted)
				{
					ActionsManager.instance.SetTargetTile (this);
				}
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
		isHighlighted = true;
		GameManager.instance.highlightedTiles.Add (this);
	}

	// Delete highlight object from this tile.
	public void UnHighlight()
	{
		if (highlightInstance != null) {
			Destroy (highlightInstance);
		}
		isHighlighted = false;
	}

	public void Select()
	{
		if (!selected) {
			selected = true;
		}
		GameManager.instance.boardScript.SetSelectedTile (this.gameObject);
		Highlight();
	}

	// Remove selection from current tile and it's neighbours.
	public void Unselect()
	{
		if (selected) {
			selected = false;
		}
		GameManager.instance.boardScript.SetSelectionToNull();

		UnHighlight ();
	}
		
	// Receives a game object and instantiates it as a creature.
	public void InstantiateCreature(GameObject creature, int playerIndex)
	{
		GameObject instance = Instantiate (creature, spawnPoint.position, Quaternion.identity);
		this.creature = instance.GetComponent <CreatureController> ();

		if(this.creature==null)
		{
			Debug.LogError ("Can't reach creature controller.");
		}

		this.creature.ChangeTeam (playerIndex);
	}

	// Receives a gameobject and instantiates it as a resource.
	public void InstantiateResource(GameObject resource)
	{
		this.resource = Instantiate (resource, transform, false);
	}
}
