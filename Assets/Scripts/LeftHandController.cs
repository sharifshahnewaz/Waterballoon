using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandController : MonoBehaviour
{
	GameObject leftController;
	private float xPos, initX, currentX, maxX;
	private GameController gameController;
	// Use this for initialization
	void Start ()
	{
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController> ();
		}
		if (gameController == null) {
			Debug.Log ("Cannot find 'GameController' script");
		}
	}

	// Update is called once per frame
	void Update ()
	{
		leftController = GameObject.FindGameObjectWithTag ("LeftController");
		if (leftController != null) {
			initX = gameController.leftHandInitPos.x;
			currentX = leftController.transform.position.x;
			if (gameController.HeadCalibrated && currentX < initX) {
				xPos = initX + (currentX - initX) * gameController.scale;
				if (currentX > maxX) {
					maxX = currentX;
					Debug.Log ("Left max "+maxX);
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
