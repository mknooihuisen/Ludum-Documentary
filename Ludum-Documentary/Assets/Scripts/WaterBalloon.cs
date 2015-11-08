using UnityEngine;
using System.Collections;

public class WaterBalloon : MonoBehaviour
{

	private float fullSpeed = 0.2f;
	private static float START_SPEED = 0.01f;
	private float speed = START_SPEED;

	public Transform steam;
	private GameObject steamObject;

	void FixedUpdate ()
	{
		if (gameObject.GetComponent<Rigidbody> ().velocity.y < -0.5f) {
			gameObject.GetComponent<Rigidbody> ().velocity = (new Vector3 (0, -0.5f, 0));
		} else if (!gameObject.GetComponent<ManipulatableObject> ().isRadioactive && gameObject.GetComponent<Rigidbody> ().velocity.y > 0) {
			gameObject.GetComponent<Rigidbody> ().velocity = (new Vector3 (0, -0.05f, 0));
		}
		if (gameObject.GetComponent<ManipulatableObject> ().isRadioactive) {
			gameObject.GetComponent<Rigidbody> ().AddForce (new Vector3 (0, speed, 0));
			if (speed < fullSpeed) {
				speed = speed + START_SPEED;
				if (speed > fullSpeed) {
					speed = fullSpeed;
				}
			}
			if (steamObject == null && speed >= fullSpeed) {
				Transform trans = Instantiate (steam);
				steamObject = trans.gameObject;
				steamObject.transform.parent = this.gameObject.transform;
				steamObject.transform.localPosition = Vector3.zero + new Vector3 (0, 10.0f, 0);
			}
		} else {
			if (steamObject != null) {
				Destroy (steamObject);
				steamObject = null;
			}
		}
	}
}
