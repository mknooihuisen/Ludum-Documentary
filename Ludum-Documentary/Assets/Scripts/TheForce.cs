using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TheForce : MonoBehaviour
{
	private static int BACKGROUND = 15;

	private static float WELL_STRENGTH = 30.0f;
	private static float FIELD_STRENGTH = 35.0f;
	private static float TERMINAL_SPEED = 50.0f;

	private static int NO_FORCE = 0;
	private static int GRAVITY_WELL = 1;
	private static int GRAVITY_SHIFT = 2;
	private static int MAGNETIC = 3;
	private static int ELECTRIC = 4;
	private static int WEAK = 5;
	private static int STRONG = 6;

	private int force = NO_FORCE;

	private List<GameObject> caught;

	void Start ()
	{
		caught = new List<GameObject> ();
	}

	GameObject PlaceForce ()
	{
		Ray ray = Camera.main.ScreenPointToRay (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0));
		RaycastHit hit;
		GameObject hitObject = null;
		if (Physics.Raycast (ray.origin, ray.direction, out hit, 3000.0f, 1 << BACKGROUND)) {
			if (hit.collider.gameObject.layer == BACKGROUND || hit.collider.gameObject.layer == 0) {
				Vector3 clickedPosition = hit.point;
				hitObject = hit.collider.gameObject;
				clickedPosition.z = 1.0f;
				transform.position = clickedPosition;
			}
		}
		return hitObject;
	}

	void FixedUpdate ()
	{
		if (cInput.GetKey ("GravityWell") && (force == GRAVITY_WELL || force == NO_FORCE)) {
			force = GRAVITY_WELL;
			GameObject hitObject = PlaceForce ();
			if (cInput.GetKey ("Up")) {
				Gravitate (true);
			} else if (cInput.GetKey ("Down")) {
				Gravitate (false);
			} else {
				RemoveCaughtObjects ();
			}
		} else if (cInput.GetKey ("Magnetic") && (force == MAGNETIC || force == NO_FORCE)) {
			force = MAGNETIC;
			GameObject hitObject = PlaceForce ();
			if (cInput.GetKey ("Up")) {
				Magnetize (true);
			} else if (cInput.GetKey ("Down")) {
				Magnetize (false);
			} else {
				RemoveCaughtObjects ();
			}
		} else if (cInput.GetKey ("GravityShift") && (force == GRAVITY_SHIFT || force == NO_FORCE)) {
			force = GRAVITY_SHIFT;
			GameObject hitObject = PlaceForce ();
			if (cInput.GetKey ("Up")) {
				GravityShift (true);
			} else if (cInput.GetKey ("Down")) {
				GravityShift (false);
			} else {
				RemoveCaughtObjects ();
			}
		} else {
			this.transform.position = new Vector3 (0, -100.0f, 1.0f);
			force = NO_FORCE;
			RemoveCaughtObjects ();
		}
	}

	void Gravitate (bool towards)
	{
		GameObject[] objs = FindObjectsOfType<GameObject> ();
		foreach (GameObject go in objs) {
			if (go.GetComponent<Rigidbody> () != null && go.GetComponent<Rigidbody> ().mass > 0.001) {
				if (Vector3.Distance (transform.position, go.transform.position) < 5.0f) {
					go.GetComponent<Rigidbody> ().useGravity = false;
					if (caught.Count == 0 || !caught.Contains (go)) {
						caught.Add (go);
					}
					float speed = WELL_STRENGTH / Mathf.Pow (Mathf.Abs (go.transform.position.x - transform.position.x) + Mathf.Abs (go.transform.position.y - transform.position.y), 2.0f);
					if (!towards) {
						speed = -speed;
					}
					MoveAtSpeed (go, speed);
				} else {
					go.GetComponent<Rigidbody> ().useGravity = true;
					caught.Remove (go);
				}
			}
		}
	}

	void GravityShift (bool increase)
	{
		GameObject[] objs = FindObjectsOfType<GameObject> ();
		foreach (GameObject go in objs) {
			if (go.GetComponent<Rigidbody> () != null && go.GetComponent<Rigidbody> ().mass > 0.001) {
				if (Vector3.Distance (transform.position, go.transform.position) < 5.0f) {
					if (caught.Count == 0 || !caught.Contains (go)) {
						caught.Add (go);
					}
					if (increase) {
						go.GetComponent<Rigidbody> ().AddForce (new Vector3 (0.0f, -19.6f, 0.0f) * Time.deltaTime * 100);
					} else if (go.GetComponent<Rigidbody> ().useGravity == true) {
						go.GetComponent<Rigidbody> ().AddForce (new Vector3 (0.0f, 0.98f, 0.0f) * Time.deltaTime * 100);
						go.GetComponent<Rigidbody> ().AddTorque (new Vector3 (Random.Range (0.0f, 0.98f), Random.Range (0.0f, 0.98f), Random.Range (0.0f, 0.98f)) * Time.deltaTime * 100);
						go.GetComponent<Rigidbody> ().useGravity = false;
					}
				} else {
					go.GetComponent<Rigidbody> ().useGravity = true;
					caught.Remove (go);
				}
			}
		}
	}

	void Magnetize (bool towards)
	{
		GameObject[] objs = FindObjectsOfType<GameObject> ();
		foreach (GameObject go in objs) {
			if (go.GetComponent<Rigidbody> () != null && go.GetComponent<Rigidbody> ().mass > 0.001 && go.tag == "Metal") {
				if (Vector3.Distance (transform.position, go.transform.position) < 5.0f) {
					if (go.GetComponent<Rigidbody> ().useGravity == true) {
						go.GetComponent<Rigidbody> ().useGravity = false;
					}
					if (caught.Count == 0 || !caught.Contains (go)) {
						caught.Add (go);
					}
					float speed = FIELD_STRENGTH / Mathf.Pow (Mathf.Abs (go.transform.position.x - transform.position.x) + Mathf.Abs (go.transform.position.y - transform.position.y), 1.0f);
					if (!towards) {
						speed = -speed;
					}
					MoveAtSpeed (go, speed);

				} else {
					go.GetComponent<Rigidbody> ().useGravity = true;
					caught.Remove (go);
				}
			}
		}

	}

	void MoveAtSpeed (GameObject go, float speed)
	{
		if (speed > TERMINAL_SPEED) {
			speed = TERMINAL_SPEED;
		} else if (speed < -TERMINAL_SPEED) {
			speed = -TERMINAL_SPEED;
		}

		Vector3 moveTo = transform.position;
		moveTo.z = 0;
		go.transform.position = Vector3.MoveTowards (go.transform.position, moveTo, speed * Time.deltaTime);
		if (moveTo == go.transform.position) {
			go.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			go.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
		}
	}

	void RemoveCaughtObjects ()
	{
		if (caught.Count > 0) {
			foreach (GameObject go in caught) {
				go.GetComponent<Rigidbody> ().useGravity = true;
			}
			caught.RemoveRange (0, caught.Count);
		}
	}
}
