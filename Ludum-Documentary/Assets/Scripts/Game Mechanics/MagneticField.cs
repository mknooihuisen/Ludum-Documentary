using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagneticField : MonoBehaviour
{

	private static int BACKGROUND = 15;
	
	private static float WELL_STRENGTH = 20.0f;
	private static float TERMINAL_SPEED = 50.0f;

	private List<GameObject> caught;
	
	void Start ()
	{
		caught = new List<GameObject> ();
	}

	void Update ()
	{
		if (cInput.GetKey ("Magnetic")) {
			Ray ray = Camera.main.ScreenPointToRay (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0));
			RaycastHit hit;
			GameObject hitObject = null;
			if (Physics.Raycast (ray, out hit, 3000)) {
				if (hit.collider.gameObject.layer == BACKGROUND || hit.collider.gameObject.layer == 0) {
					Vector3 clickedPosition = hit.point;
					hitObject = hit.collider.gameObject;
					clickedPosition.z = 1.0f;
					transform.position = clickedPosition;
					Debug.Log (hitObject);
				}
			}
			if (cInput.GetKey ("Up")) {
				GameObject[] objs = FindObjectsOfType<GameObject> ();
				
				foreach (GameObject go in objs) {
					if (go.GetComponent<Rigidbody> () != null && go.tag == "Metal") {
						go.GetComponent<Rigidbody> ().useGravity = false;
						if (!caught.Contains (go)) {
							caught.Add (go);
						}
						if (go.Equals (hitObject) || Vector3.Distance (transform.position, go.transform.position) < 0.1f) {
							go.transform.position = transform.position;
						} else if (Vector3.Distance (transform.position, go.transform.position) < 5.0f) {
							Magnetize (go, false);
						}
					}
				}
			} else if (cInput.GetKey ("Down")) {
				GameObject[] objs = FindObjectsOfType<GameObject> ();
				
				foreach (GameObject go in objs) {
					if (go.GetComponent<Rigidbody> () != null && go.tag == "Metal") {
						go.GetComponent<Rigidbody> ().useGravity = false;
						if (!caught.Contains (go)) {
							caught.Add (go);
						}
						if (go.Equals (hitObject) || Vector3.Distance (transform.position, go.transform.position) < 0.1f) {
							go.transform.position = transform.position;
						} else if (Vector3.Distance (transform.position, go.transform.position) < 5.0f) {
							Magnetize (go, true);
						}
					}
				}
			} else {
				RemoveCaughtObjects ();
			}
		} else {
			this.transform.position = new Vector3 (0, -100.0f, 1.0f);
			RemoveCaughtObjects ();
		}
	}
	
	void Magnetize (GameObject go, bool anti)
	{
		float speed = WELL_STRENGTH / Mathf.Pow (Mathf.Abs (go.transform.position.x - transform.position.x) + Mathf.Abs (go.transform.position.y - transform.position.y), 1.0f);
		if (speed > TERMINAL_SPEED) {
			speed = TERMINAL_SPEED;
		}
		if (anti) {
			speed = -speed;
		}
		go.transform.position = Vector3.MoveTowards (go.transform.position, transform.position, speed * Time.deltaTime);
	}

	void RemoveCaughtObjects ()
	{
		if (caught.Count > 0) {
			foreach (GameObject go in caught) {
				go.GetComponent<Rigidbody> ().useGravity = true;
				caught.Remove (go);
			}
		}
	}
}
