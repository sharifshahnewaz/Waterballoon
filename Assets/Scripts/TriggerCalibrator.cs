using UnityEngine;
using System.Collections;

public class TriggerCalibrator : MonoBehaviour
{

	public bool isLeft;
	private SteamVR_TrackedController device;
	private Calibrator calibrator;

	void Start ()
	{
		device = GetComponent<SteamVR_TrackedController> ();
		device.TriggerClicked += Trigger;
		device.Gripped += Gripped;
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			calibrator = gameControllerObject.GetComponent<Calibrator> ();
		}
		if (calibrator == null) {
			Debug.Log ("Cannot find 'GameController' script");
		}
	}

	void Trigger (object sender, ClickedEventArgs e)
	{
		if (isLeft && !calibrator.LeftCalibrated && calibrator.isDebug) {
			calibrator.CalibrateLeft ();
		} else if (!isLeft && !calibrator.RightCalibrated && calibrator.isDebug) {
			calibrator.CalibrateRight ();
		}
	}

	void Gripped (object sender, ClickedEventArgs e)
	{
		if (!calibrator.HeadCalibrated && calibrator.isDebug) {
			calibrator.CalibrateHead ();
		} 
	}
}
