using UnityEngine;
using System.Collections;

public class WaterBalloon : MonoBehaviour
{

	private float fullSpeed = 0.25f;
	private float speed = 0.01f;

	void FixedUpdate ()
	{
		if (gameObject.GetComponent<ManipulatableObject> ().isRadioactive == true) {
			gameObject.GetComponent<Rigidbody> ().AddForce (new Vector3 (0, speed, 0));
			if (speed < fullSpeed) {
				speed = speed + speed;
				if (speed > fullSpeed) {
					speed = fullSpeed;
				}
			}
		}
	}
}
