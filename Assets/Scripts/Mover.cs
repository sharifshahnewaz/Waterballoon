using UnityEngine;

public class Mover : MonoBehaviour {
    public float speed;


    void Start ( ) {
        GetComponent<Rigidbody> ( ).velocity = transform.forward * speed;
    }

    void Update ( ) {
        Vector3 newScale = 0.2f * new Vector3 ( Random.Range ( 0.6f, 1.0f ), Random.Range ( 0.6f, 1.0f ), Random.Range ( 0.6f, 1.0f ) ); ;
       // gameObject.transform.localScale = Vector3.Lerp ( gameObject.transform.localScale, newScale, Time.deltaTime*5 );
    }

}
