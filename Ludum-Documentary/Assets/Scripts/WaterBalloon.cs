using UnityEngine;
using System.Collections;

public class WaterBalloon : MonoBehaviour
{

	private float fullSpeed = 0.1f;
	private float speed = 0.02f;

	public Transform steam;
	private GameObject steamObject;

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
			if (steamObject == null && speed >= fullSpeed / 2) {
				Transform trans = Instantiate (steam);
				steamObject = trans.gameObject;
				steamObject.transform.parent = this.gameObject.transform;
				steamObject.transform.localPosition = Vector3.zero + new Vector3 (0, 8.0f, 0);
			}
		} else {
			if (steamObject != null) {
				Destroy (steamObject);
				steamObject = null;
			}
		}
	}
}
