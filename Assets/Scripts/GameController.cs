using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

public class GameController : MonoBehaviour
{
	public bool isDebug = false;
	public GameObject tennisball;

	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;

	Animation anim;

	private GameObject head;
	private int hit;
	private int miss;

	public bool Play { get; set; }


	bool isThrowLeft;
	int leftHit, leftMiss, rightHit, rightMiss;
	float leftPercentage, rightPercentage;

	Vector3 target;

	public int totalBall = 50;

	private string displayMessage = null;
	private StringBuilder balanceDataRecorder;

	private Calibrator calibrator;

	public int sampleRate = 10;
	public String studyCondition;

	public GameObject hitText;
	public GameObject missText;
	public GameObject messageText;

	AudioSource ballShooting;
	GameDataRecorder dataRecorder;

	void Start ()
	{
		Play = true;

		hit = 0;
		miss = 0;

		head = GameObject.FindWithTag ("MainCamera");

		if (head == null) {
			Debug.Log ("Cannot find 'Head' of the avatar");
		}

		displayMessage = "";

		StartCoroutine (SpawnWaves ());
		leftPercentage = 0.90f;
		rightPercentage = 0.90f;

		calibrator = GetComponent<Calibrator> ();
		dataRecorder = GetComponent<GameDataRecorder> ();
		anim = GameObject.Find ("Jim").GetComponent<Animation> ();
		ballShooting = GameObject.Find ("BallShooting").GetComponent<AudioSource> ();
	}

	void Update ()
	{

		/*if (Input.GetKeyDown (KeyCode.P)) {
			if (Play) {
				Play = false;
				displayMessage = "'P' to play";
				Debug.Log ("Play stopped");
			} else {
				Play = true;
				displayMessage = "'P' to stop";
				Debug.Log ("Play started");
			}

		}
*/
		if (Input.GetKeyDown (KeyCode.C)) {
			calibrator.CalibrateLeft ();
			calibrator.CalibrateRight ();
			calibrator.CalibrateHead ();
		}


		if (hit + miss >= totalBall) {
			Play = false;
			displayMessage = "Game Over";
		}
			
		hitText.GetComponent<TextMesh> ().text = "Smashed: " + hit + " R: (" + rightPercentage + ")";
		missText.GetComponent<TextMesh> ().text = "Missed: " + miss + " L: (" + leftPercentage + ")";
		messageText.GetComponent<TextMesh> ().text = displayMessage;

	}

	IEnumerator SpawnWaves ()
	{
		yield return new WaitForSeconds (startWait);

		GameObject bowlingMachineHead = GameObject.FindGameObjectsWithTag ("BowlingMachineHead") [0];
		Vector3 spawnPosition = new Vector3 (bowlingMachineHead.transform.position.x,
			                        bowlingMachineHead.transform.position.y + 0.1f, bowlingMachineHead.transform.position.z - 0.5f);
		Quaternion spawnRotation = Quaternion.identity;

		while (true) {
			for (int i = 0; i < hazardCount; i++) {
				if (calibrator.LeftCalibrated && calibrator.RightCalibrated && calibrator.HeadCalibrated && Play) {

					if (isThrowLeft) {
						target = calibrator.leftHandInitPos;
						target.x = leftDistanceAdjustment ();
						                        
					} else {
						target = calibrator.rightHandInitPos;
						target.x = rightDistanceAdjustment ();						                        
					}
					spawnRotation = Quaternion.LookRotation (spawnPosition - target - new Vector3 (0, 0, 0.10f));
					anim.Play ();
					yield return new WaitForSeconds (0.8f);
					Instantiate (tennisball, spawnPosition, spawnRotation);
					ballShooting.Play ();
					isThrowLeft = !isThrowLeft;
				}
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);
		}
	}

	private float leftDistanceAdjustment ()
	{
		if (leftHit >= 2) {
			leftHit = leftMiss = 0;
			leftPercentage += 0.1f;
		} else if (leftMiss >= 2 && leftPercentage > 0.9f) {
			leftHit = leftMiss = 0;
			leftPercentage -= 0.1f;
		}
		return calibrator.headInitPos.x - Vector3.Distance (calibrator.headInitPos, calibrator.leftHandInitPos) * leftPercentage;
	}

	private float rightDistanceAdjustment ()
	{
		if (rightHit >= 2) {
			rightHit = rightMiss = 0;
			rightPercentage += 0.1f;
		} else if (rightMiss >= 2 && rightPercentage > 0.9f) {
			rightHit = rightMiss = 0;
			rightPercentage -= 0.1f;
		}
		return calibrator.headInitPos.x + Vector3.Distance (calibrator.rightHandInitPos, calibrator.headInitPos) * rightPercentage;
	}

	public void AddHit ()
	{
		hit += 1;
		//the next one will be thrown left. This was one right
		if (isThrowLeft)
			rightHit++;
		else
			leftHit++;

		dataRecorder.record (hit + miss, isThrowLeft ? "Right" : "Left", isThrowLeft ? rightPercentage : leftPercentage, target, "Hit");
	}

	public void AddMiss ()
	{
		miss += 1;
		if (isThrowLeft)
			rightMiss++;
		else
			leftMiss++;

		dataRecorder.record (hit + miss, isThrowLeft ? "Right" : "Left", isThrowLeft ? rightPercentage : leftPercentage, target, "Miss");
	}

}