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


	void Start ()
	{

		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController> ();
		}
		if (gameController == null) {
			Debug.Log ("Cannot find 'GameController' script");
		}
		leftHand = GameObject.FindGameObjectWithTag ("LeftHand");
		rightHand = GameObject.FindGameObjectWithTag ("RightHand");
		leftCatch = rightCatch = false;
	}

	void OnTriggerEnter (Collider other)
	{
		//Debug.Log ( other.tag );
		if (other.tag == "Boundary") {
			gameController.AddMiss ();
			AudioSource ballShooting = GameObject.Find ("BallShooting").GetComponent<AudioSource> ();
			// ballShooting.pitch = 0.6f;
			// ballShooting.volume = 0.03f;
			ballShooting.Play ();
			Destroy (gameObject);
			//return;
		} else if (other.tag == "LeftHand") {
			leftCatch = true;
		} else if (other.tag == "RightHand") {
			rightCatch = true;
		}
		if (leftCatch || rightCatch) {
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
			Destroy (gameObject);
			//}
		}
	}
}