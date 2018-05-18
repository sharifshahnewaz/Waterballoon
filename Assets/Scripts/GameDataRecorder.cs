using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GameDataRecorder : MonoBehaviour {

	StringBuilder gameInfo;

	Calibrator calibrator;

	void Start(){
		gameInfo = new StringBuilder ();
		gameInfo.Append ("Ball No, Direction, Percentage,TargetX, TargetY, TargetZ, Status\n");
		calibrator = GetComponent<Calibrator> ();
	}

	public void record(int ballNo, string direction, float percentage, Vector3 target, string status){
		gameInfo.Append ("" + ballNo + ", " + direction + ", " + percentage + ", " + target.x + ", " + target.y + ", " + target.z + ", " + status + "\n");
	}

	void OnApplicationQuit ()
	{
		string studyCondition;
		if (calibrator.scale < 1.0f) {
			studyCondition = "under-scaled";
		}
		else if (calibrator.scale > 1.0f) {
			studyCondition = "over-scaled";
		}
		else {
			studyCondition = "not-scaled";
		}
		System.IO.File.AppendAllText ("Data/" + studyCondition + "-game-" + System.DateTime.Now.Ticks.ToString () + ".csv", gameInfo.ToString ());
		Debug.Log (studyCondition + "-game data is written");
	}
}
