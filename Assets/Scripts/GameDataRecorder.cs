using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GameDataRecorder : MonoBehaviour
{

	StringBuilder gameInfo;
	StringBuilder calibrationData;
	float originalX;

	float rightMax = -100.0f;
	float leftMax = 100.0f;

	Calibrator calibrator;

	void Start ()
	{
		gameInfo = new StringBuilder ();
		gameInfo.Append ("Ball No, Direction, Percentage, OriginalX, ScaledX, ScaledY, ScaledZ, Status\n");

		calibrationData = new StringBuilder ();
		calibrationData.Append ("Left Int X, Head Int X, Right Int X, Left Int Y, Head Int Y, Right Int Y, Left Int Z, Head Int Z, Right Int Z\n");

		calibrator = GetComponent<Calibrator> ();
	}

	public void record (int ballNo, string direction, float percentage, Vector3 target, string status)
	{
		if (status.Contains ("Hit")) {
			if (percentage <= 1) {
				originalX = target.x;
			} else {
				if (direction.Contains ("Right")) {
					originalX = calibrator.rightHandInitPos.x + (target.x - calibrator.rightHandInitPos.x) / calibrator.scale;
					if (rightMax < originalX) {
						rightMax = originalX;
					}
				} else {
					originalX = calibrator.leftHandInitPos.x - (calibrator.leftHandInitPos.x - target.x) / calibrator.scale;
					if (leftMax> originalX) {
						leftMax = originalX;
					}
				}
			}
		} else {
			originalX = 0;
		}

		gameInfo.Append ("" + ballNo + ", " + direction + ", " + percentage + ", " + originalX + ", " + target.x + ", " + target.y + ", " + target.z + ", " + status + "\n");
	}

	void OnApplicationQuit ()
	{
		string studyCondition;
		if (calibrator.scale < 1.0f) {
			studyCondition = "under-scaled";
		} else if (calibrator.scale > 1.0f) {
			studyCondition = "over-scaled";
		} else {
			studyCondition = "not-scaled";
		}

		string fileId = System.DateTime.Now.Ticks.ToString ();

		System.IO.File.AppendAllText ("Data/" + studyCondition + "-game-" + fileId + ".csv", gameInfo.ToString ());
		Debug.Log (studyCondition + "-game data is written");

		calibrationData.Append (
			calibrator.leftHandInitPos.x + ", " + calibrator.headInitPos.x + ", " + calibrator.rightHandInitPos.x + ", " +
			calibrator.leftHandInitPos.y + ", " + calibrator.headInitPos.y + ", " + calibrator.rightHandInitPos.y + ", " +
			calibrator.leftHandInitPos.z + ", " + calibrator.headInitPos.z + ", " + calibrator.rightHandInitPos.z + "\n");
			
		System.IO.File.AppendAllText ("Data/" + studyCondition + "-calibration-" + fileId + ".csv", calibrationData.ToString ());
		Debug.Log (studyCondition + "-calibration data is written");
	}
}
