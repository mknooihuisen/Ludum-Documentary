using UnityEngine;
using System.Collections;

public class NavPointBehavior : MonoBehaviour
{

	public bool verticalJump;

	public bool horizontalJump;

	public GameObject jumpLocation;

	public bool die;

	public bool end;

	public bool forceProceed;

	private bool jumpSafe;

	private GameObject character;
	private GameObject	 simChar;

	private float distToGround = 1.0f;
	private float aboveGroundDist = 3.0f;

	public Transform simPrefab;

	public bool simulating;
	private CharacterNavigate charScript;
	private float simTimer;

	void Start ()
	{

		character = GameObject.FindGameObjectWithTag ("Player");

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

	void Update() {

		if (simulating && simTimer <= 1.0f) {
			simTimer += Time.deltaTime;
			//Debug.Log("Waiting");
			return;
		}

		if (simulating) {

			if(charScript.onGround) {
				jumpSafe = true;
				simulating = false;
				Debug.Log("Passed Sim");
				//Destroy(simChar);
			} else if(charScript.maxDropDistance >= Mathf.Abs(transform.position.y - simChar.transform.position.y)) {
				jumpSafe = false;
				simulating = false;
				Debug.Log("Failed Sim");
				Destroy(simChar);
			}
		}
	}

	public bool simJump () {
		return true;
	}

	public bool CanMakeJump ()
	{
		return true;
	}

}
