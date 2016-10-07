using UnityEngine;
using System.Collections;

public class TrackedObject : MonoBehaviour {

    public Vector3 position;
    public bool isCalibrated;
	public TrackedObject () {
        position = Vector3.zero;
        isCalibrated = false;
	}
	
}
