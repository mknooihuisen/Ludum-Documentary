using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NavToggler : MonoBehaviour {

	private float aboveGroundDist = 3.0f;
	private float distToGround = 1.0f;

	public List<GameObject> navs;

	// Use this for initialization
	void Start () {
		RaycastHit hit;
		if (Physics.Raycast (transform.position, Vector3.down, out hit, aboveGroundDist)) {
			float newPlace = hit.point.y + distToGround;
			Vector3 current = transform.position;
			transform.position = new Vector3 (current.x, newPlace, current.z);
		}
	}

	void OnTriggerEnter (Collider col) {
		if(col.gameObject.CompareTag("Player")) {
			foreach (GameObject point in navs) {
				point.SetActive(!point.activeSelf);
			}
			

			this.gameObject.SetActive(false);
		}
	}
}
