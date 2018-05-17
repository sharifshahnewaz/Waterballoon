using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandController : MonoBehaviour
{
	GameObject leftController;
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
		leftController = GameObject.FindGameObjectWithTag ("LeftController");
		if (leftController != null) {
			initX = calibrator.leftHandInitPos.x;
			currentX = leftController.transform.position.x;
			if (calibrator.HeadCalibrated && currentX < initX) {
				xPos = initX + (currentX - initX) * calibrator.scale;
				if (currentX > maxX) {
					maxX = currentX;
				}
			} else {
				xPos = leftController.transform.position.x;
			}
			this.transform.position = new Vector3 (xPos, leftController.transform.position.y, leftController.transform.position.z);
			this.transform.eulerAngles = new Vector3 (
				leftController.transform.rotation.eulerAngles.x,
				leftController.transform.rotation.eulerAngles.y,
				leftController.transform.rotation.eulerAngles.z);
		}
	}
}
