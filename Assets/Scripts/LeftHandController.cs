using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandController : MonoBehaviour
{
	GameObject leftHand;
	private float xPos, initX, currentX;
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
		leftHand = GameObject.FindGameObjectWithTag ("LeftController");
		if (leftHand != null) {
			initX = gameController.leftHandInitPos.x;
			currentX = leftHand.transform.position.x;
			if (gameController.HeadCalibrated && currentX < initX) {
				xPos = initX + (currentX - initX) * gameController.scale;
			} else {
				xPos = leftHand.transform.position.x;
			}
			this.transform.position = new Vector3 (xPos, leftHand.transform.position.y, leftHand.transform.position.z);
			this.transform.eulerAngles = new Vector3 (
				leftHand.transform.rotation.eulerAngles.x,
				leftHand.transform.rotation.eulerAngles.y,
				leftHand.transform.rotation.eulerAngles.z);
		}
	}
}
