using UnityEngine;
using System.Collections;

public class NavPointBehavior : MonoBehaviour
{
	/** Whether the NavPoint prompts a vertical jump */
	public bool verticalJump;

	/** Whether the NavPoint prompts a horizontal jump */
	public bool horizontalJump;

	/** The location the character should be jumping to */
	public GameObject jumpLocation;

	/** Whether this NavPoint kills the player */
	public bool die;

	/** Whether this NavPoint ends the level */
	public bool end;

	/** Whether the NavPoint forces the character to jump, whether they'd make it or not */
	public bool forceProceed;

	/** Whether or not the jump in question is safe for the character */
	private bool jumpSafe;

	/** The ideal distance above the ground NavPoints should be */
	private float distToGround = 1.0f;

	/** Above this height, NavPoints do not snap to the ground */
	private float aboveGroundDist = 3.0f;

	/** The prefab for the similuated player */
	public Transform simPrefab;

	/** Whether the NavPoint simulates the jump */
	public bool simulating;

	/** Timer for the simulation */
	private float simTimer;

	/** How much power the character should put into their jump */
	public float jumpPower = 1.0f;

	private GameObject simChar;

	private CharacterNavigate charScript;

	void Start ()
	{

		if (jumpPower == 0.0f) {
			jumpPower = 1.0f;
		}

		RaycastHit hit;
		if (Physics.Raycast (transform.position, Vector3.down, out hit, aboveGroundDist)) {
			float newPlace = hit.point.y + distToGround;
			Vector3 current = transform.position;
			transform.position = new Vector3 (current.x, newPlace, current.z);
		}

		//if jumping at this nav, simulate it now


		/**if (verticalJump || horizontalJump) {
			Transform t = Instantiate (simPrefab, transform.position, Quaternion.identity) as Transform;
			simChar = t.gameObject;
			charScript = simChar.GetComponent<CharacterNavigate>();

			if (verticalJump) {
				charScript.jumpVertical ();
				Debug.Log("Junmp Vert");
			} else {
				charScript.jumpHorizontal ();
				Debug.Log("Junmp Hor");
			}

			simulating = true;
			simTimer = 0.0f;

		}
		*/
	}

	void Update ()
	{

		if (simulating && simTimer <= 1.0f) {
			simTimer += Time.deltaTime;
			//Debug.Log("Waiting");
			return;
		}

		if (simulating) {

			if (charScript.onGround) {
				jumpSafe = true;
				simulating = false;
				Debug.Log ("Passed Sim");
				//Destroy(simChar);
			} else if (charScript.terminalVelocity >= Mathf.Abs (transform.position.y - simChar.transform.position.y)) {
				jumpSafe = false;
				simulating = false;
				Debug.Log ("Failed Sim");
				Destroy (simChar);
			}
		}
	}

	public bool simJump ()
	{
		return true;
	}

	public bool CanMakeJump ()
	{
		return true;
	}

}
