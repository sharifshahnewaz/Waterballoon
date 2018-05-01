using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHandController : MonoBehaviour
{
	GameObject rightHand;
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
		rightHand = GameObject.FindGameObjectWithTag ("RightController");
		if (rightHand != null) {
			initX = gameController.rightHandInitPos.x;
			currentX = rightHand.transform.position.x;
			if (gameController.HeadCalibrated && currentX > initX) {
				xPos = initX + (currentX - initX) * gameController.scale;
			} else {
				xPos = rightHand.transform.position.x;
			}
			this.transform.position = new Vector3 (xPos, rightHand.transform.position.y, rightHand.transform.position.z);
			this.transform.eulerAngles = new Vector3 (
				rightHand.transform.rotation.eulerAngles.x,
				rightHand.transform.rotation.eulerAngles.y,
				rightHand.transform.rotation.eulerAngles.z + 180.0f); // rotate around z axis to match the hand
			
		}
	}
}
