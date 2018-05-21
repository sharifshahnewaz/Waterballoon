using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calibrator : MonoBehaviour
{
	
	public bool LeftCalibrated { get; set; }

	public bool RightCalibrated { get; set; }

	public bool HeadCalibrated { get; set; }

	public Vector3 headInitPos, leftHandInitPos, rightHandInitPos;

	public float scale =1.0f;

	public bool isDebug = false;

	public bool Calibrate (out Vector3 handInitPos, string tag)
	{
		handInitPos = GameObject.FindWithTag (tag).transform.position;
		Debug.Log (tag + " calibrated");
		return true;
	}

	public void CalibrateLeft ()
	{
		LeftCalibrated = Calibrate (out leftHandInitPos, "LeftController");
	}

	public void CalibrateRight ()
	{
		RightCalibrated = Calibrate (out rightHandInitPos, "RightController");
	}

	public void CalibrateHead ()
	{
		HeadCalibrated = Calibrate (out headInitPos, "MainCamera");
	}
}
