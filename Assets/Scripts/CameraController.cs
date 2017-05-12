using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script to move and rotate the camera using mouse buttons.

public class CameraController : MonoBehaviour {

	public float movingVelocity = 0.2f;
	public float rotatingVelocity = 0.5f;
	public float zoomFactor= 3.0f;

	private bool canRotate;
	private Vector3 clickMoveOrigin;
	private Vector3 rotationPoint;
	private Vector3 cameraMovingOrigin;
	private Vector3 cameraRotatingVector;
	private Camera cam;


	void Start () {
		cam = gameObject.GetComponent <Camera> ();
		if(cam==null)
		{
			Debug.Log ("Couldn't get camera component!");
		}

		canRotate = false;
	}
	

	void Update () {
		
		// Move using left mouse button.
		MoveCamera (0);

		// Rotate using middle mouse button.
		//RotateCamera (2);

		// Zoom camera using scrool wheel.
		ZoomCamera ();

		LookAtSelectedTile ();
	}

	public void MoveCamera(int button)
	{
		if(Input.GetMouseButtonDown (button))
		{
			clickMoveOrigin = Input.mousePosition;
			cameraMovingOrigin = cam.transform.position;
		}

		else if(Input.GetMouseButton (button))
		{
			cam.transform.position = cameraMovingOrigin + (new Vector3((Input.mousePosition.x - clickMoveOrigin.x), 0f, 
															(Input.mousePosition.y - clickMoveOrigin.y))*movingVelocity);
		}
	}

	public void RotateCamera (int button)
	{

		if (Input.GetMouseButtonDown (button)) {

			Ray camRay = cam.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast (camRay, out hit, LayerMask.GetMask ("Board")))
			{
				rotationPoint = hit.point;
				canRotate = true;
				Debug.Log ("Rotating aroung: "+rotationPoint);
			}

			cameraRotatingVector = rotationPoint - cam.transform.position;
		} 

		else if(Input.GetMouseButtonUp (button))
		{
			canRotate = false;
		}

		else if (Input.GetMouseButton (button) && canRotate) 
		{
			cameraRotatingVector = rotationPoint - Input.mousePosition;
			cam.transform.RotateAround (rotationPoint, new Vector3(0f, 1f, 0f), rotatingVelocity);
		}
	}

	public void ZoomCamera ()
	{
		float moveRange = - Input.GetAxis ("Mouse ScrollWheel") * zoomFactor;

		Vector3 zoom = new Vector3(0f, moveRange, moveRange);

		cam.transform.position += zoom;
	}

	public void LookAtSelectedTile()
	{
		GameObject tile;
		if(GameManager.instance.boardScript.SelectedTile (out tile))
		{
			cam.transform.LookAt (tile.transform.position);
		}
	}
}
