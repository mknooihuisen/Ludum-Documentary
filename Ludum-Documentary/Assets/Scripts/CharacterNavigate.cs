using UnityEngine;
using System.Collections;

public class CharacterNavigate : MonoBehaviour {

	private int navpointLayer = 30;
	private int navMask;

	private Vector3 lastDir;

	public float speed = 2;

	private GameObject newNav;

	private Vector3 [] directions;

	// Use this for initialization
	void Start () {
		navMask = 1 << navpointLayer;
		//lastDir = "";
		newNav = transform.gameObject;


		directions = new Vector3[4];
		directions [0] = Vector3.right;
		directions [1] = Vector3.down;
		directions [2] = Vector3.left;
		directions [3] = Vector3.up;

	}
	
	// Update is called once per frame
	void Update () {

		//are we at a nav point? Lets find another.
		if (transform.position == newNav.transform.position) {
			GetNextNavPoint ();
		}

		//rotate if necessary
		if (transform.rotation != newNav.transform.rotation) {
			Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (newNav.transform.position - transform.position), speed * Time.deltaTime);
		} else {
			//move toward point
			transform.Translate (transform.forward * speed);
		}

	}

	void GetNextNavPoint() {

		RaycastHit hit;

		foreach (Vector3 dir in directions) {

			if(Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity)) {

				Debug.Log("Hit!");
				Debug.DrawLine(transform.position, hit.transform.position);

			}
		}

	}




}
