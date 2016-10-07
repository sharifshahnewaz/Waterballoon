using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

public class GameController : MonoBehaviour
{
	public GameObject tennisball;
	
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;
    
	private GameObject head;
	private GameObject OVRPlayerController;
	
	//public GUIText scoreText;
	//public GUIText restartText;
	//public GUIText gameOverText;

	
	private int hit;
	private int miss;
    public bool play;
    bool isLeft;

    TrackedObject headInitPos, leftHandInitPos, rightHandInitPos;

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
		play = false;
       
		hit = 0;
		miss = 0;

        head = GameObject.FindWithTag ("MainCamera");

		if (head == null) {
			Debug.Log ("Cannot find 'Head' of the avatar");
		}
		
		displayMessage = "'P' to \nplay";
		
		StartCoroutine (SpawnWaves ());
		
	}
	
	void Update ()
	{
		
		if (Input.GetKeyDown (KeyCode.P)) {
			if (play) {
				play = false;				
				displayMessage = "'P' to \nplay";
			} else {
				play = true;
				displayMessage = "'P' to \nstop";
			}
			
		}

        if ( Input.GetKeyDown ( KeyCode.L ) ) {
            leftHandInitPos.position = GameObject.FindWithTag ( "LeftHand" ).transform.position;
            leftHandInitPos.isCalibrated = true;
        }
        if ( Input.GetKeyDown ( KeyCode.R ) ) {
            rightHandInitPos.position = GameObject.FindWithTag ( "RightHand" ).transform.position;
            rightHandInitPos.isCalibrated = true;
        }
        if ( Input.GetKeyDown ( KeyCode.H ) ) {
            headInitPos.position = GameObject.FindWithTag ( "MainCamera" ).transform.position;
            headInitPos.isCalibrated = true;
        }


        if (hit + miss >= totalBall) {
			play = false;
			displayMessage = "Game\nOver";
		}
		hitText.GetComponent <TextMesh> ().text = "Hit: " + hit;
		missText.GetComponent <TextMesh> ().text = "Miss: " + miss;
		messageText.GetComponent <TextMesh> ().text = displayMessage;
		
	}

	IEnumerator SpawnWaves ()
	{
		yield return new WaitForSeconds (startWait);
	
		while (true) {		
						
			for (int i = 0; i < hazardCount; i++) {
				GameObject bowlingMachineHead = GameObject.FindGameObjectsWithTag ("BowlingMachineHead") [0];
				Vector3 spawnPosition = new Vector3 (bowlingMachineHead.transform.position.x, 
								bowlingMachineHead.transform.position.y+0.1f, bowlingMachineHead.transform.position.z-0.5f);
                //Debug.Log ( bowlingMachineHead.transform.position );
				Quaternion spawnRotation = Quaternion.identity;
				if (head != null) {
					spawnRotation = Quaternion.LookRotation (spawnPosition - head.transform.position - new Vector3 (0, 0, 0.10f));
				}	
				if (play) {							
					Instantiate (tennisball, spawnPosition, spawnRotation);
					
				}		
				yield return new WaitForSeconds (spawnWait);
			}
			
			yield return new WaitForSeconds (waveWait);
			
							
						
		}
	}
   
	
	public void AddHit ()
	{
		hit += 1;
		//UpdateScore ();
	}
	public void AddMiss ()
	{
		miss += 1;
		//UpdateScore ();
	}
		
		
	void OnApplicationQuit ()
	{
		
		long fileId = System.DateTime.Now.Ticks;
		//TODO remove comments below
		//System.IO.File.AppendAllText (studyCondition + "-balance-" + fileId.ToString () + ".csv", balanceDataRecorder.ToString ());
		//System.IO.File.AppendAllText (studyCondition + "-score-" + fileId.ToString () + ".txt", "Hit: " + hit + ", Miss: " + miss);
	}
}