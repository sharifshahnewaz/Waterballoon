using UnityEngine;

using System;

public class Mover : MonoBehaviour 
{
	public float speed;
    private GameObject leftController;
    private GameObject rightController;
    private GameController gameController;

    void Start ()
	{
        GameObject gameControllerObject = GameObject.FindWithTag ( "GameController" );
        if ( gameControllerObject != null ) {
            gameController = gameControllerObject.GetComponent<GameController> ( );
        }
        if ( gameController == null ) {
            Debug.Log ( "Cannot find 'GameController' script" );
        }   

        GetComponent<Rigidbody>().velocity = transform.forward * speed;
        leftController = GameObject.FindGameObjectWithTag ( "LeftHand" );
        rightController = GameObject.FindGameObjectWithTag ( "RightHand" );
    }
    void Update ( ) {
        if ( Math.Abs ( Vector3.Distance ( leftController.transform.position, gameObject.transform.position ) ) < 0.1f ) {
            GetComponent<Rigidbody> ( ).velocity = Vector3.zero;
            transform.position = leftController.transform.position;
            gameController.play = false;
            if ( Math.Abs ( Vector3.Distance ( leftController.transform.position, rightController.transform.position ) ) < 0.1f ) {
                gameController.play = true;
                Destroy ( gameObject );
            }
        }
    }
}
