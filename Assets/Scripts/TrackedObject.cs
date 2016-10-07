using UnityEngine;
using System.Collections;

public class TrackedObject {

    public Vector3 position;
    public bool isCalibrated;
	public TrackedObject () {
        position = Vector3.zero;
        isCalibrated = false;
	}
	
}
