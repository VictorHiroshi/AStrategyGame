using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary {
	public float xMin;
	public float xMax;
	public float zMin;
	public float zMax;
	public float yMin;
	public float yMax;
}

public class CameraController : MonoBehaviour {

	public float automaticCameraVelocity = 20f;
	public float manualCameraVelocity = 0.08f;
	public float zoomFactor = 3.0f;
	public Boundary cameraBoundaries;

	[HideInInspector] public bool canZoom;
	[HideInInspector] public bool canMove;

	private Camera m_Camera;
	private Vector3 cameraOrigin;
	private Vector3 clickPoint;


	void Awake () {
		m_Camera = GetComponentInChildren<Camera> ();
		canZoom = true;
		canMove = true;
	}

	void Update () {
		MoveCamera (1);
		ZoomCamera ();
	}

	// Move the camera to look at the specified target, maintaining the actual ratio.
	public void MoveToTarget (Transform target)
	{
		// TODO: Smooth the transition.
		//transform.position = target.position;

		StartCoroutine (SmoothlyMove (target));
	}

	// Guarantees that the camera will remain within the boardgame space.
	private void CheckBoundaries ()
	{
		Vector3 correctedPosition = transform.position;

		if(transform.position.x < cameraBoundaries.xMin){
			correctedPosition.x = cameraBoundaries.xMin;
		}
		if(transform.position.x > cameraBoundaries.xMax){
			correctedPosition.x = cameraBoundaries.xMax;
		}
		if(transform.position.z < cameraBoundaries.zMin){
			correctedPosition.z = cameraBoundaries.zMin;
		}
		if(transform.position.z > cameraBoundaries.zMax){
			correctedPosition.z = cameraBoundaries.zMax;
		}

		transform.position = correctedPosition;
	}

	// Checks for movements of the mouse while pressing the given button.
	private void MoveCamera(int button)
	{
		if(!canMove)
		{
			return;
		}

		if(Input.GetMouseButtonDown (button))
		{
			clickPoint = Input.mousePosition;
			cameraOrigin = transform.position;
		}

		else if(Input.GetMouseButton (button))
		{
			transform.position = cameraOrigin + (new Vector3((Input.mousePosition.x - clickPoint.x), 0f, 
				(Input.mousePosition.y - clickPoint.y)) * manualCameraVelocity);

			CheckBoundaries ();
		}
	}

	// Zoom in or out whit the scroll wheel.
	private void ZoomCamera ()
	{
		if(!canZoom)
		{
			return;
		}
		float moveRange = - Input.GetAxis ("Mouse ScrollWheel") * zoomFactor;
		Vector3 zoom = new Vector3(0f, moveRange, moveRange);


		if((m_Camera.transform.position.y > cameraBoundaries.yMin && moveRange < 0f) || 
			(m_Camera.transform.position.y < cameraBoundaries.yMax) && moveRange > 0f)
		{
			m_Camera.transform.position += zoom;
		}
	}

	private IEnumerator SmoothlyMove(Transform target)
	{

		yield return null;

		float step;
		canMove = false;
		canZoom = false;

		while(transform.position != target.position)
		{	
			step = automaticCameraVelocity * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, target.position, step);
			yield return null;
		}

		canMove = true;
		canZoom = true;
	}
}
