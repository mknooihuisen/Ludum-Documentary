using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TheForce : MonoBehaviour
{
	private static int BACKGROUND = 15;
	private static int MANIPULATABLE = 14;

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

	private GameObject manipulated;

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
			Vector3 clickedPosition = hit.point;
			hitObject = hit.collider.gameObject;
			clickedPosition.z = 1.0f;
			transform.position = clickedPosition;
		}
		return hitObject;
	}

	GameObject ForceOnObject ()
	{
		Ray ray = Camera.main.ScreenPointToRay (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0));
		RaycastHit hit;
		GameObject hitObject = null;
		if (Physics.Raycast (ray.origin, ray.direction, out hit, 3000.0f, 1 << MANIPULATABLE)) {
			hitObject = hit.collider.gameObject;
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
		} else if (cInput.GetKey ("Weak") && (force == WEAK || force == NO_FORCE)) {
			GameObject hitObject = ForceOnObject ();
			if (hitObject != null) {
				Renderer r = hitObject.GetComponent<Renderer> ();
				if (manipulated == null) {
					manipulated = hitObject;
					Select (manipulated, false);
				} else if (manipulated != hitObject) {
					Select (manipulated, true);
					manipulated = hitObject;
					Select (manipulated, false);
				}
				if (cInput.GetKeyDown ("Up")) {
					r.material.color = new Color (r.material.color.r, r.material.color.g + 0.5f, r.material.color.b);
					Select (manipulated, true);
					manipulated = null;
				} else if (cInput.GetKeyDown ("Down")) {
					r.material.color = new Color (r.material.color.r, r.material.color.g - 0.5f, r.material.color.b);
					Select (manipulated, true);
					manipulated = null;
				}
			} else {
				if (manipulated != null) {
					Select (manipulated, true);
					manipulated = null;
				}
			}
		} else if (cInput.GetKey ("Strong") && (force == STRONG || force == NO_FORCE)) {
			GameObject hitObject = ForceOnObject ();
			if (hitObject != null) {
				Renderer r = hitObject.GetComponent<Renderer> ();
				if (manipulated == null) {
					manipulated = hitObject;
					Select (manipulated, false);
				} else if (manipulated != hitObject) {
					Select (manipulated, true);
					manipulated = hitObject;
					Select (manipulated, false);
				}
				if (cInput.GetKeyDown ("Up")) {
					// Do nothing for now...
				} else if (cInput.GetKeyDown ("Down")) {
					hitObject.SetActive (false);
				}
			} else {
				if (manipulated != null) {
					Select (manipulated, true);
					manipulated = null;
				}
			}
		} else {
			this.transform.position = new Vector3 (0, -100.0f, 1.0f);
			force = NO_FORCE;
			if (manipulated != null) {
				Select (manipulated, true);
				manipulated = null;
			}
			RemoveCaughtObjects ();
		}
	}

	void Select (GameObject select, bool deselect)
	{
		if (deselect) {
			if (select != null) {
				Renderer r = select.GetComponent<Renderer> ();
				r.material.color = new Color (r.material.color.r - 0.2f, r.material.color.g - 0.2f, r.material.color.b - 0.2f);
			}
		} else {
			if (select != null) {
				Renderer r = select.GetComponent<Renderer> ();
				r.material.color = new Color (r.material.color.r + 0.2f, r.material.color.g + 0.2f, r.material.color.b + 0.2f);
			}
		}
	}

	void Gravitate (bool towards)
	{
		GameObject[] objs = FindManipulatableObjects ();
		foreach (GameObject go in objs) {
			if (go.GetComponent<Rigidbody> () != null && go.GetComponent<ManipulatableObject> ().hasMass) {
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
		GameObject[] objs = FindManipulatableObjects ();
		foreach (GameObject go in objs) {
			if (go.GetComponent<Rigidbody> () != null && go.GetComponent<ManipulatableObject> ().hasMass) {
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
		GameObject[] objs = FindManipulatableObjects ();
		foreach (GameObject go in objs) {
			if (go.GetComponent<Rigidbody> () != null && go.GetComponent<ManipulatableObject> ().metal) {
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

	GameObject[] FindManipulatableObjects ()
	{
		GameObject[] objs = FindObjectsOfType<GameObject> ();
		List<GameObject> maniplatable = new List<GameObject> ();
		for (int i = 0; i < objs.Length; i++) {
			if (objs [i].layer == MANIPULATABLE) {
				if (objs [i].GetComponent<ManipulatableObject> () != null) {
					maniplatable.Add (objs [i]);
				}
			}
		}
		return maniplatable.ToArray ();
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
