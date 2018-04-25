using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHandController : MonoBehaviour
{
	GameObject rightHand;
	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{
		rightHand = GameObject.FindGameObjectWithTag ("RightHand");
		if (rightHand != null) {
			this.transform.position = new Vector3 (rightHand.transform.position.x, rightHand.transform.position.y, rightHand.transform.position.z);
			this.transform.eulerAngles = new Vector3 (
				rightHand.transform.rotation.eulerAngles.x,
				rightHand.transform.rotation.eulerAngles.y,
				rightHand.transform.rotation.eulerAngles.z + 180.0f); // rotate around z axis to match the hand
		}
	}
}
