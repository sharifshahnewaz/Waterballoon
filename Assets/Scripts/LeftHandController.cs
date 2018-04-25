using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandController : MonoBehaviour
{
	GameObject leftHand;
	public float xPos;
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
			if (gameController.HeadCalibrated) {
				xPos = gameController.headInitPos.x + (leftHand.transform.position.x - gameController.headInitPos.x) * gameController.scale;
			} else {
				xPos = leftHand.transform.position.x;
			}
			this.transform.position = new Vector3 (xPos, leftHand.transform.position.y, leftHand.transform.position.z);
			this.transform.eulerAngles = new Vector3 (
				leftHand.transform.rotation.eulerAngles.x,
				leftHand.transform.rotation.eulerAngles.y,
				leftHand.transform.rotation.eulerAngles.z); // rotate around z axis to match the hand

		}
	}
}
