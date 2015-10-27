using UnityEngine;
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
	public bool onGround { get; private set; }

	public float maxDropDistance;
	private float distToGround;

	//Death variables
	private bool dead;
	private Vector3 deathPoint;
	public float crumpleSpeed;
	private Vector3 finalRestingPlace;
	private float startTime, journeyLength;

	private Animator anim;

	public bool isSimChar;

	// Use this for initialization
	void Start ()
	{
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

		verticalJumpPower *= 10.0f;
		horizontalJumpPower *= 10.0f;

		distToGround = gameObject.GetComponent<CapsuleCollider> ().bounds.extents.y + 0.1f;

		//anim = GetComponentInChildren<Animator> ();
		dead = false;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		checkGround ();

		//die when falling too far
		if (onGround && goingToNav == null && Mathf.Abs (transform.position.y - lastNav.transform.position.y) > maxDropDistance) {
			die ();
		}

		//make sure we have a point to go to and we're on the ground
		if (goingToNav != null && onGround) {


			//rotate if necessary
			if (directionToNav == Vector3.left) {
				transform.Rotate (0, 180.0f, 0);
				directionToNav = Vector3.right;
			}





			if (timer >= walkSpeed && !dead) {
				rb.AddForce (transform.forward * speed);
				timer = 0.0f;
			}
			timer += Time.deltaTime;
		
		
		} else {
			goingToNav = GetNextNavPoint ();
		}
	}

	float checkGround (Vector3 startPos)
	{

		RaycastHit hit;
		if (Physics.Raycast (startPos, Vector3.down, out hit, Mathf.Infinity)) {

			//if this is the player, we care about onGround
			if (startPos == transform.position) {
				if (hit.distance <= distToGround) {
					onGround = true;
					////anim.SetBool("jumping", false);
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
		Debug.Log ("I hit: " + col.gameObject.name);

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

	public void jumpHorizontal ()
	{
		if (onGround || isSimChar) {
			//anim.SetBool("jumping", true);
			float up = horizontalJumpPower / 4.0f;
			Rigidbody rb = GetComponent<Rigidbody>();
			rb.AddForce (Vector3.forward * 45.0f);
			rb.AddForce (Vector3.up * up);
		}
	}

	public void jumpVertical ()
	{
		if (onGround || isSimChar) {
			//anim.SetBool("jumping", true);
			float forward = verticalJumpPower / 3.0f;

			Rigidbody rb = GetComponent<Rigidbody>();


			rb.AddForce (Vector3.up * verticalJumpPower);
			rb.AddForce (Vector3.forward * forward);
		}
	}
	

	/**
	 * Get the next nav point we should go to
	 */
	GameObject GetNextNavPoint ()
	{
		if (isSimChar) {
			return null;
		}

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
		dead = true;
		rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ ;

	}	
}
