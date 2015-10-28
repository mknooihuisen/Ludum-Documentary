using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraMovement : MonoBehaviour
{

	private GameObject character;

	private float width, height;

	private Camera camera;

	public float moveSpeed;

	private Vector3 location;

	private float zDist;

	private float journeyLength, startTime;

	private Vector3 oldLocation;

	public float startDelay;

	private float timer;

	private List<BodyData> correctRigidbodies;

	// Use this for initialization
	void Start ()
	{
		//Time.timeScale = 3;
		character = GameObject.FindGameObjectWithTag ("Player");
		camera = GetComponent<Camera> ();
		location = transform.position;

		//get current cam distance
		updateCamDistance ();

		timer = 0.0f;
		correctRigidbodies = new List<BodyData> ();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{

		//Don't worry about camera position while player enters.
		if (timer < startDelay) {
			timer += Time.deltaTime;
			return;
		}

		//get where the player is in the viewport
		Vector3 pos = camera.WorldToViewportPoint (character.transform.position);

		if (transform.position == location) {
			freeze (false);
			if (pos.x <= 0.0f) {
				//Debug.Log ("On left");
				location = new Vector3 (location.x - width + 2.0f, location.y, zDist);
				startMove ();
			} else if (pos.x >= 1.0f) {
				//Debug.Log ("On Right");
				location = new Vector3 (location.x + width, location.y, zDist);
				startMove ();
			} else if (pos.y >= 1.0f) {
				//Debug.Log ("On Top");
				location = new Vector3 (location.x, location.y + height, zDist);
				startMove ();
			} else if (pos.y <= 0.0f) {
				//Debug.Log ("On Bottom");
				location = new Vector3 (location.x, location.y - height, zDist);
				startMove ();

			}

		}

		//if we need to move.
		else {
			float distCovered = (Time.time - startTime) * moveSpeed;
			float fracJourney = distCovered / journeyLength;
			transform.position = Vector3.Lerp (oldLocation, location, fracJourney);
		}
	
	}

	void freeze (bool freeze)
	{
		if (freeze) {
			correctRigidbodies.RemoveRange (0, correctRigidbodies.Count);
			Rigidbody[] rigidbodies = FindObjectsOfType<Rigidbody> ();
			foreach (Rigidbody rb in rigidbodies) {
				if (!rb.isKinematic) {
					correctRigidbodies.Add (new BodyData (rb.velocity, false, rb));
					rb.isKinematic = true;
				}
			}
		} else if (correctRigidbodies.Count > 0 && correctRigidbodies [0].rb.isKinematic == true) {
			foreach (BodyData bd in correctRigidbodies) {
				Rigidbody rb = bd.go.GetComponent<Rigidbody> ();
				rb.isKinematic = false;
				rb.velocity = bd.velocity;
			}
		}
	}

	void startMove ()
	{
		oldLocation = transform.position;
		startTime = Time.time;
		journeyLength = Vector3.Distance (oldLocation, location);
		freeze (true);
	}

	/**
	 * Call whenever the distance between camera and 0 changes
	 */
	public void updateCamDistance ()
	{
		zDist = transform.position.z;

		//get the points in world coords at the edges of the camera.
		Vector3 bottomLeft = camera.ViewportToWorldPoint (new Vector3 (0.0f, 0.0f, zDist));
		Vector3 topRight = camera.ViewportToWorldPoint (new Vector3 (1.0f, 1.0f, zDist));

		width = Mathf.Abs (topRight.x - bottomLeft.x);
		height = Mathf.Abs (topRight.y - bottomLeft.y);


	}

	private class BodyData
	{
		public Vector3 velocity;
		public bool kinematic;
		public Rigidbody rb;
		public GameObject go;

		public BodyData (Vector3 velocity, bool kinematic, Rigidbody rb)
		{
			this.velocity = velocity;
			this.kinematic = kinematic;
			this.rb = rb;
			go = rb.gameObject;
		}
	}
}
