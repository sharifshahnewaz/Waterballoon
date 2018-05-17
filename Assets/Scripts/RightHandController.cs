using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHandController : MonoBehaviour
{
	GameObject rightController;
	private float xPos, initX, currentX, maxX;
	private Calibrator calibrator;
	// Use this for initialization
	void Start ()
	{
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			calibrator = gameControllerObject.GetComponent<Calibrator> ();
		}
		if (calibrator == null) {
			Debug.Log ("Cannot find 'GameController' script");
		}
	}

	// Update is called once per frame
	void Update ()
	{
		rightController = GameObject.FindGameObjectWithTag ("RightController");
		if (rightController != null) {
			initX = calibrator.rightHandInitPos.x;
			currentX = rightController.transform.position.x;
			if (calibrator.HeadCalibrated && currentX > initX) {
				xPos = initX + (currentX - initX) * calibrator.scale;
				if (currentX < maxX) {
					maxX = currentX;
				}
			} else {
				xPos = rightController.transform.position.x;
			}
			this.transform.position = new Vector3 (xPos, rightController.transform.position.y, rightController.transform.position.z);
			this.transform.eulerAngles = new Vector3 (
				rightController.transform.rotation.eulerAngles.x,
				rightController.transform.rotation.eulerAngles.y,
				rightController.transform.rotation.eulerAngles.z + 180.0f); // rotate around z axis to match the hand
			
		}
	}
}
