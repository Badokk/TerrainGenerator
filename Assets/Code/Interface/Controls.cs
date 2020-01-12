using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
	//BoardManager manager;
	Camera cam;
	readonly float camSpeed = 30;
	readonly float scrollSpeed = 10;
	readonly float rotateSpeed = 10;

	Vector3 startingCameraPosition;
	Quaternion startingCameraRotation;

	// Use this for initialization
	void Start()
	{
		cam = Camera.main;
		startingCameraPosition = cam.transform.position;
		startingCameraRotation = cam.transform.rotation;
		//manager = BoardManager.getInstance();
	}

	// Update is called once per frame
	void Update()
	{
		ControlCamera();
		//MouseControl();
	}

	void ControlCamera()
	{
		// Camera movement
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
			cam.transform.Translate(0, Time.deltaTime * camSpeed, 0);
		if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
			cam.transform.Translate(0, Time.deltaTime * camSpeed * -1, 0);
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
			cam.transform.Translate(Time.deltaTime * camSpeed * -1, 0, 0);
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
			cam.transform.Translate(Time.deltaTime * camSpeed, 0, 0);

		cam.transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * scrollSpeed);

		// Camera rotation
		if (Input.GetKey(KeyCode.R))
			cam.transform.Rotate(rotateSpeed * Time.deltaTime * Vector3.left);
		if (Input.GetKey(KeyCode.F))
			cam.transform.Rotate(rotateSpeed * Time.deltaTime * Vector3.right);
		if (Input.GetKey(KeyCode.Q))
			cam.transform.Rotate(rotateSpeed * Time.deltaTime * Vector3.up);
		if (Input.GetKey(KeyCode.E))
			cam.transform.Rotate(rotateSpeed * Time.deltaTime * Vector3.down);

		// Reset to default
		if (Input.GetKey(KeyCode.P))
		{
			cam.transform.position = startingCameraPosition;
			cam.transform.rotation = startingCameraRotation;
		}

	}
}
