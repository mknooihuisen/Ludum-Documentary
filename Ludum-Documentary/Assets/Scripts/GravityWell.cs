using UnityEngine;
using System.Collections;

public class GravityWell : MonoBehaviour
{
	private static int BACKGROUND = 15;

	private static float WELL_STRENGTH = 30.0f;
	private static float TERMINAL_SPEED = 50.0f;

	void Update ()
	{
		if (cInput.GetKey ("GravityWell")) {
			Ray ray = Camera.main.ScreenPointToRay (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0));
			RaycastHit hit;
			GameObject hitObject = null;
			if (Physics.Raycast (ray, out hit, 3000)) {
				if (hit.collider.gameObject.layer == BACKGROUND || hit.collider.gameObject.layer == 0) {
					Vector3 clickedPosition = hit.point;
					hitObject = hit.collider.gameObject;
					clickedPosition.z = 0;
					transform.position = clickedPosition;
					Debug.Log (hitObject);
				}
			}
			if (cInput.GetKey ("Up")) {
				GameObject[] objs = FindObjectsOfType<GameObject> ();

				foreach (GameObject go in objs) {
					if (go.GetComponent<Rigidbody> () != null && go.GetComponent<Rigidbody> ().mass > 0) {
						if (go.Equals (hitObject) || Mathf.Abs (go.transform.position.x - transform.position.x) < 0.2f && Mathf.Abs (go.transform.position.y - transform.position.y) < 0.2f) {
							go.transform.position = transform.position;
						} else if (Mathf.Abs (go.transform.position.x - transform.position.x) < 10.0f && Mathf.Abs (go.transform.position.y - transform.position.y) < 10.0f) {
							Gravitate (go, false);
						}
					}
				}
			} else if (cInput.GetKey ("Down")) {
				GameObject[] objs = FindObjectsOfType<GameObject> ();
								
				foreach (GameObject go in objs) {
					if (go.GetComponent<Rigidbody> () != null && go.GetComponent<Rigidbody> ().mass > 0) {
						if (Mathf.Abs (go.transform.position.x - transform.position.x) < 10.0f && Mathf.Abs (go.transform.position.y - transform.position.y) < 10.0f) {
							Gravitate (go, true);
						}
					}
				}
			}
		}
	}

	void Gravitate (GameObject go, bool anti)
	{
		float speed = WELL_STRENGTH / Mathf.Pow (Mathf.Abs (go.transform.position.x - transform.position.x) + Mathf.Abs (go.transform.position.y - transform.position.y), 2.0f);
		if (speed > TERMINAL_SPEED) {
			speed = TERMINAL_SPEED;
		}
		if (anti) {
			speed = -speed;
		}
		go.transform.position = Vector3.MoveTowards (go.transform.position, transform.position, speed * Time.deltaTime);
	}
}
