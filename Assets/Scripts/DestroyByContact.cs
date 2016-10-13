using UnityEngine;
using System;
using System.Collections;

public class DestroyByContact : MonoBehaviour {
    //public GameObject explosion;
    //public GameObject playerExplosion;
    public int scoreValue;
    private GameController gameController;
    private GameObject leftController;
    private GameObject rightController;
    bool leftCatch, rightCatch;


    void Start ( ) {

        GameObject gameControllerObject = GameObject.FindWithTag ( "GameController" );
        if ( gameControllerObject != null ) {
            gameController = gameControllerObject.GetComponent<GameController> ( );
        }
        if ( gameController == null ) {
            Debug.Log ( "Cannot find 'GameController' script" );
        }
        leftController = GameObject.FindGameObjectWithTag ( "LeftHand" );
        rightController = GameObject.FindGameObjectWithTag ( "RightHand" );
        leftCatch = rightCatch = false;
    }

    void OnTriggerEnter ( Collider other ) {
        //Debug.Log ( other.tag );
        if ( other.tag == "Boundary" ) {
            gameController.AddMiss ( );
            AudioSource ballShooting = GameObject.Find ( "BallShooting" ).GetComponent<AudioSource> ( );
         // ballShooting.pitch = 0.6f;
           // ballShooting.volume = 0.03f;
            ballShooting.Play ( );
            Destroy ( gameObject );
            //return;
        } else if ( other.tag == "LeftHand" ) {
            leftCatch = true;
            gameController.AddHit();
        } else if ( other.tag == "RightHand" ) {
            rightCatch = true;
            gameController.AddHit ();
        }
        if ( leftCatch || rightCatch ) {
            gameController.Play = false;
            GetComponent<Rigidbody> ( ).velocity = Vector3.zero;
            GetComponent<Rigidbody> ( ).angularVelocity = Vector3.zero;
            gameObject.transform.position = other.gameObject.transform.position;

        }
    }
    void Update ( ) {
        if ( leftCatch || rightCatch ) { 
            gameObject.transform.position = (leftCatch)? leftController.transform.position: rightController.transform.position;

        }
        if ( !gameController.Play && Math.Abs ( Vector3.Distance ( leftController.transform.position, rightController.transform.position ) ) < 0.1f ) {
            gameController.Play = true;
            
            leftCatch = rightCatch = false;
            Destroy ( gameObject );
        }
    }
}