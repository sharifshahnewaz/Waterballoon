using UnityEngine;
using System;
using System.Collections;

public class DestroyByContact : MonoBehaviour
{
	//public GameObject explosion;
	//public GameObject playerExplosion;
	public int scoreValue;
	private GameController gameController;
	private GameObject leftHand;
	private GameObject rightHand;
	bool leftCatch, rightCatch;
	AudioSource splash;
	AudioSource hit;

	SteamVR_TrackedObject leftController;
	SteamVR_TrackedObject rightController;

	void Start ()
	{

		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController> ();
		}
		if (gameController == null) {
			Debug.Log ("Cannot find 'GameController' script");
		}

		splash = GameObject.Find ("Splash").GetComponent<AudioSource> ();
		hit = GameObject.Find ("BalloonHit").GetComponent<AudioSource> ();

		leftHand = GameObject.FindGameObjectWithTag ("LeftHand");
		rightHand = GameObject.FindGameObjectWithTag ("RightHand");
		leftCatch = rightCatch = false;
	}

	void OnTriggerEnter (Collider other)
	{
		leftController = GameObject.FindWithTag ("LeftController").GetComponent<SteamVR_TrackedObject>();
		rightController = GameObject.FindWithTag ("RightController").GetComponent<SteamVR_TrackedObject>();
		//Debug.Log ( other.tag );
		if (other.tag == "Boundary") {
			gameController.AddMiss ();

			Destroy (gameObject);
			//return;
		} else if (other.tag == "LeftHand") {
			leftCatch = true;
			SteamVR_Controller.Input ((int)leftController.index).TriggerHapticPulse (3000);
		} else if (other.tag == "RightHand") {
			rightCatch = true;
			SteamVR_Controller.Input ((int)rightController.index).TriggerHapticPulse (3000);
		}
		if (leftCatch || rightCatch) {
			hit.Play ();
			gameController.Play = false;
			GetComponent<Rigidbody> ().velocity = Vector3.zero;
			GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
			gameObject.transform.position = other.gameObject.transform.position;

		}
	}

	void Update ()
	{
		if (leftCatch || rightCatch) { 
			gameObject.transform.position = (leftCatch) ? leftHand.transform.position : rightHand.transform.position;

		}
		if (!gameController.Play && Math.Abs (Vector3.Distance (leftHand.transform.position, rightHand.transform.position)) < 0.1f) {
			//if ((leftCatch && rightHand.transform.position.x < gameController.headInitPos.x)
			//	|| (rightCatch && leftHand.transform.position.x > gameController.headInitPos.x)) {
			gameController.Play = true;  
			gameController.AddHit ();
			leftCatch = rightCatch = false;
			splash.Play ();
			Destroy (gameObject);
			//}
		}
	}
}