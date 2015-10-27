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

	public GameObject character;

	private float distToGround = 1.0f;
	private float aboveGroundDist = 3.0f;

	void Start ()
	{

		RaycastHit hit;
		if (Physics.Raycast (transform.position, Vector3.down, out hit, aboveGroundDist)) {
			float newPlace = hit.point.y + distToGround;
			Vector3 current = transform.position;
			transform.position = new Vector3 (current.x, newPlace, current.z);
		}
	}

	public bool CanMakeJump ()
	{
		RaycastHit hit;
		if (Physics.Raycast (transform.position, Vector3.down, out hit, 3000.0f)) {
			float upForce = 0, sideForce = 0;
			if (horizontalJump) {
				sideForce = character.GetComponent<CharacterNavigate> ().horizontalJumpPower;
				upForce = sideForce / 4.0f;

			} else if (verticalJump) {
				upForce = character.GetComponent<CharacterNavigate> ().verticalJumpPower;
				sideForce = upForce / 3.0f;
			}

			// Convert the forces to velocity based on V = sqrt(F / M)
			sideForce = Mathf.Sqrt (sideForce / character.GetComponent<Rigidbody> ().mass);
			upForce = Mathf.Sqrt (upForce / character.GetComponent<Rigidbody> ().mass);

			float dist = hit.distance;
			float gravity = 9.8f;
			float heightA = transform.position.y - dist;
			float heightB = jumpLocation.transform.position.y + (jumpLocation.GetComponent<Collider> ().bounds.extents.y);
			float size = jumpLocation.GetComponent<Collider> ().bounds.extents.x;
			if (transform.position.x < jumpLocation.transform.position.x) {
				size = -size;
			}
			float distanceB = Mathf.Abs ((jumpLocation.transform.position.x + size) - character.transform.position.x);
			float vertForce = upForce;
			float horiForce = sideForce;
			float time = distanceB * 10 / horiForce;
			float jumpDist = vertForce * time - (0.5f * gravity * time * time);
			Debug.Log (heightA + " " + heightB + " " + distanceB + " " + vertForce + " " + horiForce + " " + time + " " + gravity + " : " + jumpDist);

			if (jumpDist > Mathf.Abs (heightB - heightA)) {
				return true;
			} else {
				return false;
			}
		}
		return false;
	}

}
