﻿using UnityEngine;
using System.Collections;

public class CharacterNavigate : MonoBehaviour
{

	private int navpointLayer = 30;
	private int navMask;

	private Vector3 lastDir;

	public float speed;

	private GameObject lastNav;
	private GameObject goingToNav;
	private Vector3 directionToNav;

	private Vector3[] directions;

	private Rigidbody rb;
	private float timer;
	public float walkSpeed;
	public float verticalJumpPower;
	public float horizontalJumpPower;
	private bool onGround;

	public float maxDropDistance;
	private float distToGround;

	// Use this for initialization
	void Start ()
	{
		navMask = 1 << navpointLayer;
		Debug.Log (navMask);

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

		verticalJumpPower *= 10.0f;
		horizontalJumpPower *= 10.0f;

		distToGround = gameObject.GetComponent<CapsuleCollider> ().bounds.extents.y + 0.1f;


	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		checkGround ();


		//make sure we have a point to go to and we're on the ground
		if (goingToNav != null && onGround) {


			//rotate if necessary
			if (directionToNav == Vector3.left) {
				transform.Rotate (0, 180.0f, 0);
				directionToNav = Vector3.right;
			}


			//move toward point
			//transform.Translate (Vector3.forward * speed * Time.deltaTime);


			if (timer >= walkSpeed) {
				rb.AddForce (transform.forward * speed);
				timer = 0.0f;
			}
			timer += Time.deltaTime;
		
		} else {
			goingToNav = GetNextNavPoint ();
		}

		Debug.Log ("Heading to: " + goingToNav);

	}

	float checkGround (Vector3 startPos)
	{

		RaycastHit hit;
		if (Physics.Raycast (startPos, Vector3.down, out hit, Mathf.Infinity)) {

			//if this is the player, we care about onGround
			if (startPos == transform.position) {
				if (hit.distance <= distToGround) {
					onGround = true;
				} else {
					onGround = false;
				}
			}

			return hit.distance;
		}

		//You missed.
		return Mathf.Infinity;
	}

	float checkGround ()
	{
		return checkGround (transform.position);
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject == goingToNav) {
			lastNav = goingToNav;

			//check nav settings
			NavPointBehavior myNav = lastNav.GetComponent<NavPointBehavior> ();
			if (myNav.verticalJump) {
				if (myNav.CanMakeJump ()) {
					jumpVertical ();
				}
			} else if (myNav.horizontalJump) {
				if (myNav.CanMakeJump ()) {
					jumpHorizontal ();
				}

			} else if (myNav.die) {
				die ();
			} else if (myNav.end) {
				reachedEnd ();
			}

			lastNav.SetActive (false);
			goingToNav = GetNextNavPoint ();
			timer = -0.2f;
		}
	}

	void reachedEnd ()
	{
		Debug.Log ("You Win!");
	}

	void jumpHorizontal ()
	{
		if (onGround) {
			float up = horizontalJumpPower / 4.0f;
			rb.AddForce (Vector3.forward * horizontalJumpPower);
			rb.AddForce (Vector3.up * up);
		}
	}

	void jumpVertical ()
	{
		if (onGround) {
			float forward = verticalJumpPower / 3.0f;
			rb.AddForce (Vector3.up * verticalJumpPower);
			rb.AddForce (Vector3.forward * forward);
		}
	}
	

	/**
	 * Get the next nav point we should go to
	 */
	GameObject GetNextNavPoint ()
	{

		RaycastHit hit;

		//all four directions
		foreach (Vector3 dir in directions) {

			//we hit something
			if (Physics.Raycast (transform.position, dir, out hit, Mathf.Infinity, navMask)) {

				//make sure we weren't just there
				if (hit.transform.gameObject != lastNav) {
					Debug.DrawLine (transform.position, hit.transform.position);
					directionToNav = dir;
					return hit.transform.gameObject;
				}
			}
		}

		return null;

	}

	public void die ()
	{
		Destroy (this.gameObject);
	}	
}
