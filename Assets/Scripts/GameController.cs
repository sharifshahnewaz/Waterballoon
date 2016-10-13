using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

public class GameController : MonoBehaviour {
    public GameObject tennisball;

    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

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


    Vector3 headInitPos, leftHandInitPos, rightHandInitPos, leftHandPos, rightHandPos, headPos;
    Vector3 target;

    public int totalBall = 120;

    private string displayMessage = null;
    private StringBuilder balanceDataRecorder;



    public int sampleRate = 10;
    public String studyCondition;

    public GameObject hitText;
    public GameObject missText;
    public GameObject messageText;

    void Start ( ) {
        Play = true;

        hit = 0;
        miss = 0;

        head = GameObject.FindWithTag ( "MainCamera" );

        if ( head == null ) {
            Debug.Log ( "Cannot find 'Head' of the avatar" );
        }

        displayMessage = "'P' to \nplay";

        headInitPos = Vector3.zero;
        leftHandInitPos = Vector3.zero;
        rightHandInitPos = Vector3.zero;
        leftHandPos = Vector3.zero;
        rightHandPos = Vector3.zero;
        headPos = Vector3.zero;

        StartCoroutine ( SpawnWaves ( ) );

    }

    void Update ( ) {

        if ( Input.GetKeyDown ( KeyCode.P ) ) {
            if ( Play ) {
                Play = false;
                displayMessage = "'P' to \nplay";
            } else {
                Play = true;
                displayMessage = "'P' to \nstop";
            }

        }

        if ( Input.GetKeyDown ( KeyCode.L ) ) {
            CalibrateLeft ( );
        }
        if ( Input.GetKeyDown ( KeyCode.R ) ) {
            CalibrateRighft ( );

        }
        if ( Input.GetKeyDown ( KeyCode.H ) ) {
            CalibrateHead ( );

        }


        if ( hit + miss >= totalBall ) {
            Play = false;
            displayMessage = "Game\nOver";
        }
        hitText.GetComponent<TextMesh> ( ).text = "Hit: " + hit;
        missText.GetComponent<TextMesh> ( ).text = "Miss: " + miss;
        messageText.GetComponent<TextMesh> ( ).text = displayMessage;

    }

    IEnumerator SpawnWaves ( ) {
        yield return new WaitForSeconds ( startWait );

        GameObject bowlingMachineHead = GameObject.FindGameObjectsWithTag ( "BowlingMachineHead" )[0];
        Vector3 spawnPosition = new Vector3 ( bowlingMachineHead.transform.position.x,
                        bowlingMachineHead.transform.position.y + 0.1f, bowlingMachineHead.transform.position.z - 0.5f );
        Quaternion spawnRotation = Quaternion.identity;

        while ( true ) {
            for ( int i = 0; i < hazardCount; i++ ) {
                if ( LeftCalibrated && RightCalibrated /*&& HeadCalibrated*/ && Play ) {

                    if ( isThrowLeft ) {
                        target = leftHandPos;
                        target.x += distanceAdjustment ( );
                        isThrowLeft = false;
                    } else {
                        target = rightHandPos;
                        target.x += distanceAdjustment ( );
                        isThrowLeft = true;
                    }
                    spawnRotation = Quaternion.LookRotation ( spawnPosition - target - new Vector3 ( 0, 0, 0.10f ) );
                    Instantiate ( tennisball, spawnPosition, spawnRotation );
                }
                yield return new WaitForSeconds ( spawnWait );
            }

            yield return new WaitForSeconds ( waveWait );



        }
    }

    private float distanceAdjustment ( ) {
        return UnityEngine.Random.Range ( -0.5f, 0.5f );
    }

    public bool Calibrate ( out Vector3 handInitPos, String tag, out Vector3 hanPos ) {
        handInitPos = GameObject.FindWithTag ( tag ).transform.position;
        hanPos = handInitPos;
        Debug.Log ( tag + " calibrated" );
        return true;
    }

    public void CalibrateLeft ( ) {
        LeftCalibrated = Calibrate ( out leftHandInitPos, "LeftHand", out leftHandPos );
    }

    public void CalibrateRighft ( ) {
        RightCalibrated = Calibrate ( out rightHandInitPos, "RightHand", out rightHandPos );
    }

    public void CalibrateHead ( ) {
        HeadCalibrated = Calibrate ( out headInitPos, "MainCamera", out headPos );
    }

    public void AddHit ( ) {
        hit += 1;
        //UpdateScore ();
    }
    public void AddMiss ( ) {
        miss += 1;
        //UpdateScore ();
    }


    void OnApplicationQuit ( ) {

        long fileId = System.DateTime.Now.Ticks;

    }
}