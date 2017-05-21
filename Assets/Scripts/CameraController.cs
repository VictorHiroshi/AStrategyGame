using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary {
	public float xMin;
	public float xMax;
	public float zMin;
	public float zMax;
}

public class CameraController : MonoBehaviour {

	public float dampTime = 0.2f;
	public float minDistance = 6.4f;
	public float cameraVelocity = 0.08f;
	public float zoomFactor = 3.0f;
	public Boundary cameraBoundaries;

	private Camera m_Camera;
	private Vector3 cameraOrigin;
	private Vector3 clickPoint;
	Vector3 moveVelocity;

	// Use this for initialization
	void Awake () {
		m_Camera = GetComponentInChildren<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		MoveCamera (1);
		ZoomCamera ();
	}
		

	// Move the camera to look at the specified target, maintaining the actual ratio.
	public void MoveToTarget (Transform target)
	{
		
		transform.position = Vector3.SmoothDamp (transform.position, target.position, ref moveVelocity, dampTime);
	}

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

	private void MoveCamera(int button)
	{
		if(Input.GetMouseButtonDown (button))
		{
			clickPoint = Input.mousePosition;
			cameraOrigin = transform.position;
		}

		else if(Input.GetMouseButton (button))
		{
			transform.position = cameraOrigin + (new Vector3((Input.mousePosition.x - clickPoint.x), 0f, 
				(Input.mousePosition.y - clickPoint.y))*cameraVelocity);

			CheckBoundaries ();
		}
	}

	private void ZoomCamera ()
	{
		float moveRange = - Input.GetAxis ("Mouse ScrollWheel") * zoomFactor;
		Vector3 zoom = new Vector3(0f, moveRange, moveRange);

		m_Camera.transform.position += zoom;
	}
}
