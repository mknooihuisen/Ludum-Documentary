using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TheForce : MonoBehaviour
{
	private static int BACKGROUND = 15;
	private static int MANIPULATABLE = 14;

	private static float WELL_STRENGTH = 3000.0f;
	private static float FIELD_STRENGTH = 35.0f;
	private static float TERMINAL_SPEED = 50.0f;
	private static float FORCE_MULTIPLIER = 300.0f;

	private static int NO_FORCE = 0;
	private static int GRAVITY_WELL = 1;
	private static int GRAVITY_SHIFT = 2;
	private static int MAGNETIC = 3;
	private static int ELECTRIC = 4;
	private static int WEAK = 5;
	private static int STRONG = 6;

	private int force = NO_FORCE;

	public bool characterDead;

	private List<GameObject> caught;

	private GameObject manipulated;

	public Material silicon;

	private LevelSettingsManager levelSettings;

	private float energyDrain;

	void Start ()
	{
		GameObject [] temp = GameObject.FindGameObjectsWithTag ("GameController");
		foreach (GameObject go in temp) {
			if (go.name == "_LevelSettings") {
				levelSettings = go.GetComponent<LevelSettingsManager> ();
			}
		}
		caught = new List<GameObject> ();
		energyDrain = 0.0f;
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

	GameObject GenerateObject ()
	{
		Ray ray = Camera.main.ScreenPointToRay (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0));
		RaycastHit hit;
		GameObject hitObject = null;
		if (Physics.Raycast (ray.origin, ray.direction, out hit, 3000.0f, 1 << BACKGROUND)) {
			Vector3 clickedPosition = hit.point;
			hitObject = hit.collider.gameObject;
			clickedPosition.z = 1.0f;
			GameObject sphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			sphere.transform.position = clickedPosition;
			sphere.AddComponent<Rigidbody> ();
			sphere.AddComponent<ManipulatableObject> ();
			sphere.layer = MANIPULATABLE;
			sphere.GetComponent<Renderer> ().material = silicon;
			sphere.GetComponent<ManipulatableObject> ().hasMass = true;
		}
		return hitObject;
	}

	/**
	 * Uses a force on the selected object
	 */
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
		bool spendEnergy = true;
		if (levelSettings.isPlayerDead || levelSettings.energy <= 0.0f) {
			if (this.transform.position != new Vector3 (0, -100.0f, -100.0f)) {
				this.transform.position = new Vector3 (0, -100.0f, -100.0f);
			}
			force = NO_FORCE;
			if (manipulated != null) {
				Select (manipulated, true);
				manipulated = null;
			}
			RemoveCaughtObjects ();
			return;
		}
		if (cInput.GetKey ("GravityWell") && (force == GRAVITY_WELL || force == NO_FORCE)) {
			force = GRAVITY_WELL;
			PlaceForce ();
			if (cInput.GetKey ("Up")) {
				Gravitate (true);
			} else if (cInput.GetKey ("Down")) {
				Gravitate (false);
			} else {
				RemoveCaughtObjects ();
			}
		} else if (cInput.GetKey ("Magnetic") && (force == MAGNETIC || force == NO_FORCE)) {
			force = MAGNETIC;
			PlaceForce ();
			if (cInput.GetKey ("Up")) {
				Magnetize (true);
			} else if (cInput.GetKey ("Down")) {
				Magnetize (false);
			} else {
				RemoveCaughtObjects ();
			}
		} else if (cInput.GetKey ("GravityShift") && (force == GRAVITY_SHIFT || force == NO_FORCE)) {
			force = GRAVITY_SHIFT;
			PlaceForce ();
			if (cInput.GetKey ("Up")) {
				GravityShift (true);
			} else if (cInput.GetKey ("Down")) {
				GravityShift (false);
			} else {
				RemoveCaughtObjects ();
			}

			// These forces are applied to individual objects
		} else if (cInput.GetKey ("Electric") && (force == ELECTRIC || force == NO_FORCE)) {
			GameObject hitObject = ForceOnObject ();
			if (hitObject != null && hitObject.GetComponent<ManipulatableObject> ().powerable) {
				if (manipulated == null) {
					manipulated = hitObject;
					Select (manipulated, false);
				} else if (manipulated != hitObject) {
					Select (manipulated, true);
					manipulated = hitObject;
					Select (manipulated, false);
				}
				if (cInput.GetKeyDown ("Up")) {
					if (manipulated.GetComponent<ManipulatableObject> ().powered == false) {
						levelSettings.energy -= 10.0f;
						manipulated.GetComponent<ManipulatableObject> ().powered = true;
					}
				} else if (cInput.GetKeyDown ("Down")) {
					if (manipulated.GetComponent<ManipulatableObject> ().powered == true) {
						levelSettings.energy -= 10.0f;
						manipulated.GetComponent<ManipulatableObject> ().powered = false;
					}
				}
			} else if (hitObject == null) {
				if (manipulated != null) {
					Select (manipulated, true);
					manipulated = null;
				}
			}
		} else if (cInput.GetKey ("Weak") && (force == WEAK || force == NO_FORCE)) {
			GameObject hitObject = ForceOnObject ();
			if (hitObject != null && hitObject.GetComponent<ManipulatableObject> ().canBeRadioactive) {
				if (manipulated == null) {
					manipulated = hitObject;
					Select (manipulated, false);
				} else if (manipulated != hitObject) {
					Select (manipulated, true);
					manipulated = hitObject;
					Select (manipulated, false);
				}
				if (cInput.GetKeyDown ("Up")) {
					levelSettings.energy = levelSettings.energy - (30.0f + energyDrain);
					spendEnergy = false;
					manipulated.GetComponent<ManipulatableObject> ().isRadioactive = true;
				} else if (cInput.GetKeyDown ("Down")) {
					levelSettings.energy = levelSettings.energy - (30.0f + energyDrain);
					spendEnergy = false;
					manipulated.GetComponent<ManipulatableObject> ().isRadioactive = false;
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
				if (manipulated == null) {
					manipulated = hitObject;
					Select (manipulated, false);
				} else if (manipulated != hitObject) {
					Select (manipulated, true);
					manipulated = hitObject;
					Select (manipulated, false);
				}
				if (cInput.GetKeyDown ("Down")) {
					levelSettings.energy = levelSettings.energy - (100.0f + energyDrain);
					spendEnergy = false;
					hitObject.SetActive (false);
				}
			} else if (cInput.GetKeyDown ("Up")) {
				levelSettings.energy = levelSettings.energy - (100.0f + energyDrain);
				spendEnergy = false;
				GenerateObject ();
			} else {
				if (manipulated != null) {
					Select (manipulated, true);
					manipulated = null;
				}
			}
		} else {
			// If we aren't using a force, we aren't spending energy
			spendEnergy = false;
			force = NO_FORCE;

			if (energyDrain > 0) {
				energyDrain = energyDrain - 5.0f;
			}
			// Make sure the energy drain doesn't drop below zero
			if (energyDrain < 0) {
				energyDrain = 0;
			}

			// Put the force indicator back off screen
			if (this.transform.position != new Vector3 (0, -100.0f, -100.0f)) {
				this.transform.position = new Vector3 (0, -100.0f, -100.0f);
			}

			// Unselect all objects
			if (manipulated != null) {
				Select (manipulated, true);
				manipulated = null;
			}
			RemoveCaughtObjects ();
		}

		// Spend energy if we are using it
		if (spendEnergy && (cInput.GetKey ("Up") || cInput.GetKey ("Down")) && force != NO_FORCE) {
			levelSettings.energy -= energyDrain;
		}
	}

	/**
	 * Highlights objects that are hovered over
	 */
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

	/**
	 * The gravity well force
	 */
	void Gravitate (bool towards)
	{
		energyDrain += (1.5f * Time.deltaTime);
		GameObject[] objs = FindManipulatableObjects ();
		foreach (GameObject go in objs) {
			if (go.GetComponent<Rigidbody> () != null && go.GetComponent<ManipulatableObject> ().hasMass) {
				if (Vector3.Distance (transform.position, go.transform.position) < 5.0f) {
					go.GetComponent<Rigidbody> ().useGravity = false;
					if (go.GetComponent<ManipulatableObject> ().crushable) {
						if (Mathf.Abs (go.transform.position.x - transform.position.x) < 1.0f && Mathf.Abs (go.transform.position.y - transform.position.y) < 1.0f) {
							go.GetComponent<ManipulatableObject> ().crush ();
						}
					}
					if (caught.Count == 0 || !caught.Contains (go)) {
						caught.Add (go);
					}
					float speed = WELL_STRENGTH / Mathf.Pow (Mathf.Abs (go.transform.position.x - transform.position.x) + Mathf.Abs (go.transform.position.y - transform.position.y), 2.0f);
					if (!towards) {
						speed = -speed;
					}
					MoveWithForce (go, speed);
				} else {
					go.GetComponent<Rigidbody> ().useGravity = true;
					caught.Remove (go);
				}
			}
		}
	}

	/**
	 * The gravity shift (double or off) force
	 */
	void GravityShift (bool increase)
	{
		energyDrain += (0.5f * Time.deltaTime);
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

	/**
	 * The magnetization force
	 */
	void Magnetize (bool towards)
	{
		energyDrain += (2.5f * Time.deltaTime);
		GameObject[] objs = FindManipulatableObjects ();
		foreach (GameObject go in objs) {
			Rigidbody rb = go.GetComponent<Rigidbody> ();
			if (rb != null && go.GetComponent<ManipulatableObject> ().metal) {
				if (Vector3.Distance (transform.position, go.transform.position) < 5.0f) {
					if (rb.useGravity == true && towards) {
						rb.useGravity = false;
					}
					if (caught.Count == 0 || !caught.Contains (go)) {
						caught.Add (go);
					}
					float speed = FIELD_STRENGTH / Mathf.Pow (Mathf.Abs (go.transform.position.x - transform.position.x) + Mathf.Abs (go.transform.position.y - transform.position.y), 1.0f);
					if (!towards) {
						speed = -speed;
						if (rb.velocity.y < -10.0f) {
							rb.velocity = new Vector3 (rb.velocity.x, -10.0f, 0.0f);
						}
					}
					MoveAtSpeed (go, speed);

				} else {
					go.GetComponent<Rigidbody> ().useGravity = true;
					caught.Remove (go);
				}
			}
		}

	}

	/**
	 * Creates an array of manipulatable objects
	 */
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

	/**
	 * Moves an object with translation, rather than using forces
	 */
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

	/**
	 * Moves an object using forces, rather than translation
	 */
	void MoveWithForce (GameObject go, float speed)
	{
		if (speed > TERMINAL_SPEED * FORCE_MULTIPLIER) {
			speed = TERMINAL_SPEED * FORCE_MULTIPLIER;
		} else if (speed < -TERMINAL_SPEED * FORCE_MULTIPLIER) {
			speed = -TERMINAL_SPEED * FORCE_MULTIPLIER;
		}

		Vector3 moveTo = transform.position;
		moveTo.z = 0;
		go.GetComponent<Rigidbody> ().AddForce ((moveTo - go.transform.position) * speed * Time.deltaTime);
		if (moveTo == go.transform.position) {
			go.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			go.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
		}
	}

	/**
	 * Removes all objects caught by a force when the force ends or the player dies
	 */
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
