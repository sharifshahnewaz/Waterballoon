﻿using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

public class ReachRecorder : MonoBehaviour
{
	public string studyCondition;
	float leftInt, rightInt, leftCurrent, rightCurrent, leftMax, rightMax;
	bool recording = false;
	GameObject leftController, rightController;
	StringBuilder sb;

	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{		
		if (recording) {
			leftController = GameObject.FindGameObjectWithTag ("LeftController");
			if (leftController != null) {
				leftCurrent = leftController.transform.position.x;
				if (leftCurrent < leftMax) {
					leftMax = leftCurrent;
				}
			}

			rightController = GameObject.FindGameObjectWithTag ("RightController");
			if (rightController != null) {
				rightCurrent = rightController.transform.position.x;
				if (rightCurrent > rightMax) {
					rightMax = rightCurrent;
				}
			}
		}		
	}

	public void StartRecording (float leftInt, float rightInt)
	{
		this.leftInt = leftInt;
		this.rightInt = rightInt;
		recording = true;
	}

	public void WriteInFile ()
	{
		sb = new StringBuilder ();
		float midPoint = leftInt + (rightInt - leftInt) / 2;
		sb.Append ("Left Init, Left Max, Left Init Length, Left Max Length, Left Reach," +
		" Right Init, Right Max, Right Init Length, Right Max Length, Right Reach\n");
		sb.Append (leftInt + ", " + leftMax + ", " + (midPoint - leftInt) + ", " + (midPoint - leftMax) + ", " + (leftInt - leftMax) + ", " +
		rightInt + ", " + rightMax + ", " + (rightInt - midPoint) + ", " + (rightMax - midPoint) + ", " + (rightMax - rightInt));
		System.IO.File.AppendAllText ("Data/" + studyCondition + "-reach-" + System.DateTime.Now.Ticks.ToString () + ".csv", sb.ToString ());
		Debug.Log (studyCondition + "-reach is written");
	}
}
