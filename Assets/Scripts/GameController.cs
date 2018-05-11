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
	public float scale = 1.0f;

	Animation anim;

	private GameObject head;


	//public GUIText scoreText;
	//public GUIText restartText;
	//public GUIText gameOverText;


	private int hit;
	private int miss;

	public bool Play { get; set; }

	public bool LeftCalibrated { get; set; }

	public bool RightCalibrated { get; set; }

	public bool HeadCalibrated { get; set; }

	bool isThrowLeft;
	int leftHit, leftMiss, rightHit, rightMiss;
	float leftPercentage, rightPercentage;

	public Vector3 headInitPos, leftHandInitPos, rightHandInitPos, leftHandPos, rightHandPos, headPos;
	Vector3 target;

	public int totalBall = 120;

	private string displayMessage = null;
	private StringBuilder balanceDataRecorder;



	public int sampleRate = 10;
	public String studyCondition;

	public GameObject hitText;
	public GameObject missText;
	public GameObject messageText;

	void Start ()
	{
		Play = false;

		hit = 0;
		miss = 0;

		head = GameObject.FindWithTag ("MainCamera");

		if (head == null) {
			Debug.Log ("Cannot find 'Head' of the avatar");
		}

		displayMessage = "'P' to \nplay";

		headInitPos = Vector3.zero;
		leftHandInitPos = Vector3.zero;
		rightHandInitPos = Vector3.zero;
		leftHandPos = Vector3.zero;
		rightHandPos = Vector3.zero;
		headPos = Vector3.zero;

		StartCoroutine (SpawnWaves ());
		leftPercentage = 0.90f;
		rightPercentage = 0.90f;

		anim = GameObject.Find ("Jim").GetComponent<Animation> ();
	}

	void Update ()
	{

		if (Input.GetKeyDown (KeyCode.P)) {
			if (Play) {
				Play = false;
				displayMessage = "'P' to \nplay";
				Debug.Log ("Play stopped");
			} else {
				Play = true;
				displayMessage = "'P' to \nstop";
				Debug.Log ("Play started");
			}

		}

		/*if (Input.GetKeyDown (KeyCode.L)) {
			CalibrateLeft ();
		}
		if (Input.GetKeyDown (KeyCode.R)) {
			CalibrateRight ();
		}
		if (Input.GetKeyDown (KeyCode.H)) {
			CalibrateHead ();

		}*/

		if (Input.GetKeyDown (KeyCode.C)) {
			CalibrateLeft ();
			CalibrateRight ();
			CalibrateHead ();
		}

		if (hit + miss >= totalBall) {
			Play = false;
			displayMessage = "Game\nOver";
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
				if (LeftCalibrated && RightCalibrated && HeadCalibrated && Play) {

					if (isThrowLeft) {
						target = leftHandPos;
						target.x = leftDistanceAdjustment ();
						                        
					} else {
						target = rightHandPos;
						target.x = rightDistanceAdjustment ();						                        
					}
					spawnRotation = Quaternion.LookRotation (spawnPosition - target - new Vector3 (0, 0, 0.10f));
					anim.Play ();
					yield return new WaitForSeconds (0.8f);
					Instantiate (tennisball, spawnPosition, spawnRotation);
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
		return headInitPos.x - Vector3.Distance (headInitPos, leftHandPos) * leftPercentage;
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
		return headInitPos.x + Vector3.Distance (rightHandPos, headInitPos) * rightPercentage;
	}

	public bool Calibrate (out Vector3 handInitPos, String tag, out Vector3 hanPos)
	{
		handInitPos = GameObject.FindWithTag (tag).transform.position;
		hanPos = handInitPos;
		Debug.Log (tag + " calibrated");
		return true;
	}

	public void CalibrateLeft ()
	{
		LeftCalibrated = Calibrate (out leftHandInitPos, "LeftController", out leftHandPos);
	}

	public void CalibrateRight ()
	{
		RightCalibrated = Calibrate (out rightHandInitPos, "RightController", out rightHandPos);
	}

	public void CalibrateHead ()
	{
		HeadCalibrated = Calibrate (out headInitPos, "MainCamera", out headPos);
	}

	public void AddHit ()
	{
		hit += 1;
		//the next one will be thrown left. This was one right
		if (isThrowLeft)
			rightHit++;
		else
			leftHit++;

	}

	public void AddMiss ()
	{
		miss += 1;
		if (isThrowLeft)
			rightMiss++;
		else
			leftMiss++;
	}


	void OnApplicationQuit ()
	{
		long fileId = System.DateTime.Now.Ticks;
	}
}