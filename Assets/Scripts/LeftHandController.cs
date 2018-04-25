using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandController : MonoBehaviour {
	GameObject leftHand;
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update ()
	{
		leftHand = GameObject.FindGameObjectWithTag("LeftHand");
		if (leftHand != null) {
			this.transform.position = new Vector3 (leftHand.transform.position.x, leftHand.transform.position.y, leftHand.transform.position.z);
			this.transform.rotation = leftHand.transform.localRotation;
		}
	}
}
