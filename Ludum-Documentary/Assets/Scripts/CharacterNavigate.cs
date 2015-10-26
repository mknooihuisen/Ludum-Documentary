using UnityEngine;
using System.Collections;

public class CharacterNavigate : MonoBehaviour {

	private int navpointLayer = 30;
	private int navMask;

	private Vector3 lastDir;

	public float speed = 0.01f;

	private GameObject lastNav;
	private GameObject goingToNav;
	private Vector3 directionToNav;

	private Vector3 [] directions;

	private Rigidbody rb;
	private float timer;
	public float walkSpeed = 0.5f;

	// Use this for initialization
	void Start () {
		navMask = 1 << navpointLayer;

		directions = new Vector3[4];
		directions [0] = Vector3.right;
		directions [1] = Vector3.down;
		directions [2] = Vector3.left;
		directions [3] = Vector3.up;

		//set object to correct starting rotation
		transform.Rotate (new Vector3 (0, 90.0f, 0));

		goingToNav = GetNextNavPoint ();
		lastNav = null;

		rb = GetComponent<Rigidbody> ();
		timer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {


		//make sure we have a point to go to
		if (goingToNav != null) {


			//rotate if necessary
			if (directionToNav == Vector3.left) {
				transform.Rotate (0, 180.0f, 0);
				directionToNav = Vector3.right;
			}


			//move toward point
			//transform.Translate (Vector3.forward * speed * Time.deltaTime);


			if(timer >= walkSpeed) {
				rb.AddForce(transform.forward * speed);
				timer = 0.0f;
			}
			timer += Time.deltaTime;
		
		} else {
			goingToNav = GetNextNavPoint ();
		}

	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject == goingToNav) {
			lastNav = goingToNav;
			lastNav.SetActive(false);
			goingToNav = GetNextNavPoint ();
			timer = -0.2f;
		}
	}
	

	GameObject GetNextNavPoint() {

		RaycastHit hit;

		//all four directions
		foreach (Vector3 dir in directions) {

			//we hit something
			if(Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity, navMask)) {

				//make sure we weren't just there
				if(hit.transform.gameObject != lastNav) {
					Debug.DrawLine(transform.position, hit.transform.position);
					directionToNav = dir;
					return hit.transform.gameObject;
				}
			}
		}

		return null;

	}
}
