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

	public float terminalVelocity;
	private float distToGround;
	public Vector3 previousVelocity { get; set; }

	//Death variables
	public bool dead;
	private Vector3 deathPoint;
	public float crumpleSpeed;
	private Vector3 finalRestingPlace;
	private float startTime, journeyLength;

	private Animator anim;

	public bool isSimChar;

	public float navDist;

	private LevelSettingsManager levelSettings;

	private bool facingLeft;

	private bool endReached;

	private GameObject obstructingObject;

	private static float TIME_TO_RESET_OBSTRUCTION = 0.75f;

	private float obstructionTimer;

	// Use this for initialization
	void Start ()
	{
		endReached = false;
		facingLeft = false;
		obstructionTimer = 0;

		//Dynamically grab level settings
		GameObject [] temp = GameObject.FindGameObjectsWithTag ("GameController");
		foreach (GameObject go in temp) {
			if (go.name == "_LevelSettings") {
				levelSettings = go.GetComponent<LevelSettingsManager> ();
			}
		}

		previousVelocity = Vector3.zero;
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
		//die ();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (levelSettings.isPlayerDead) {
			die ();
		}
		if (endReached) {
			return;
		}

		checkGround ();

		//Detect Terminal Velocity
		if (Mathf.Abs (previousVelocity.x - rb.velocity.x) > terminalVelocity || Mathf.Abs (previousVelocity.y - rb.velocity.y) > terminalVelocity)
			die ();
		else {
			previousVelocity = rb.velocity;
		}


		//make sure we have a point to go to and we're on the ground
		if (goingToNav != null && onGround) {

			//rotate if necessary
//			if (transform.rotation.y == 270.0f && directionToNav == Vector3.right) {
//				transform.Rotate (0, 180.0f, 0);
//			} else
			if (directionToNav == Vector3.left && !facingLeft) {
				transform.Rotate (0, 180.0f, 0);
				facingLeft = true;
				directionToNav = Vector3.right;
			} else if (directionToNav == Vector3.right && facingLeft) {
				transform.Rotate (0, 180.0f, 0);
				facingLeft = false;
				directionToNav = Vector3.left;
			}

			if (timer >= walkSpeed && !dead) {
				if (stepCheck ()) {
					rb.AddForce (transform.up * speed * 3);
					rb.AddForce (transform.forward * speed * 2);
					Debug.Log ("JUMP!!!");
				} else {
					rb.AddForce (transform.forward * speed);
				}
				timer = 0.0f;
			}
			timer += Time.deltaTime;
		
		
		} else if (goingToNav == null) {
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
		if (col.gameObject.tag != "NavPoint") {
			return;
		}

		lastNav = col.gameObject;	

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

	void reachedEnd ()
	{
		endReached = true;
		Debug.Log ("You Win!");
	}

	public void jumpHorizontal ()
	{
		if (onGround || isSimChar) {
			//anim.SetBool("jumping", true);
			float up = horizontalJumpPower / 2.0f;

			Rigidbody rb = GetComponent<Rigidbody> ();

			NavPointBehavior nav = goingToNav.GetComponent<NavPointBehavior> ();
			rb.AddForce (Vector3.right * horizontalJumpPower * Mathf.Abs (nav.jumpPower));
			rb.AddForce (Vector3.up * up);
		}
	}

	public void jumpVertical ()
	{
		if (onGround || isSimChar) {
			//anim.SetBool("jumping", true);
			float forward = verticalJumpPower / 3.0f;

			Rigidbody rb = GetComponent<Rigidbody> ();

			NavPointBehavior nav = goingToNav.GetComponent<NavPointBehavior> ();
			rb.AddForce (Vector3.up * verticalJumpPower * Mathf.Abs (nav.jumpPower));
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
			if (Physics.Raycast (transform.position, dir, out hit, navDist, navMask)) {

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

	public bool stepCheck ()
	{
		obstructionTimer += Time.deltaTime;
		RaycastHit hit;
		Vector3 feetPosition = new Vector3 (transform.position.x, transform.position.y - 0.975f, transform.position.z);
		Vector3 kneePosition = new Vector3 (transform.position.x, transform.position.y - 0.5f, transform.position.z);
		if (Physics.Raycast (feetPosition, Vector3.right, out hit, 1.0f)) {

			// Don't jump repeatedly to get over the same object, wait a bit
			if (obstructingObject != null && hit.transform.gameObject == obstructingObject) {
				if (obstructionTimer < TIME_TO_RESET_OBSTRUCTION) {
					Debug.Log ("Obstruction Timer at " + obstructionTimer + " for " + hit.transform.gameObject.name);
					return false;
				} else {
					Debug.Log ("Obstruction Found! " + hit.transform.gameObject.name);
					obstructionTimer = 0;
				}
			} else {
				obstructingObject = hit.transform.gameObject;
				obstructionTimer = 0;
			}
			Vector3 obstructionPosition = hit.transform.position;
			if (Physics.Raycast (kneePosition, Vector3.right, out hit, 2.0f)) {
				if (hit.transform.position != obstructionPosition) {
					return true;
				}
			} else {
				return true;
			}
		}
		obstructionTimer = 0;
		return false;
	}

	public void die ()
	{
		Time.timeScale = 0;
		dead = true;
		rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezePositionZ;
		levelSettings.isPlayerDead = true;

		if (rb.velocity.x == 0.0f) {
			rb.AddForce (Vector3.right * 5.0f);
		}

	}	
}
