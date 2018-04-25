using UnityEngine;
using System.Collections;

public class RandomRoate : MonoBehaviour
{

	public float tumble;
	float timeLeft;
	Vector3 newScale;

	void Start ()
	{
		GetComponent<Rigidbody> ().angularVelocity = Random.insideUnitSphere * tumble; 
		timeLeft = 0.5f;
		newScale = new Vector3 (Random.Range (0.14f, 0.15f), Random.Range (0.14f, 0.17f), Random.Range (0.14f, 0.15f));
		//StartCoroutine (ScaleChange ());
	}

	/*IEnumerator ScaleChange ()
	{
		float timeLeft = 0.1f;
		while (true) {
			Debug.Log ("Running");
			yield return new WaitForSeconds (timeLeft);
			gameObject.transform.localScale = Vector3.Lerp (
				gameObject.transform.localScale,
				new Vector3 (Random.Range (0.1f, 0.17f), Random.Range (0.1f, 0.17f), Random.Range (0.1f, 0.17f)), Time.deltaTime / timeLeft);
			timeLeft -= Time.deltaTime;
			Debug.Log ("timeleft: " + timeLeft);
			if (timeLeft < 0)
				timeLeft = 0.1f;
		}
	}*/

	void Update ()
	{
		if (timeLeft > 0) {
			gameObject.transform.localScale = Vector3.Lerp (gameObject.transform.localScale, newScale, Time.deltaTime / timeLeft);
			timeLeft -= Time.deltaTime;
		} else {
			timeLeft = 0.5f;
			newScale = new Vector3 (Random.Range (0.14f, 0.15f), Random.Range (0.14f, 0.17f), Random.Range (0.14f, 0.15f));
		}
	}
}
