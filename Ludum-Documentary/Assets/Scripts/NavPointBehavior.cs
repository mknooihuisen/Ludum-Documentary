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

	private GameObject character;

	private float distToGround = 1.0f;
	private float aboveGroundDist = 3.0f;

	void Start ()
	{

		character = GameObject.FindGameObjectWithTag ("Player");

		RaycastHit hit;
		if (Physics.Raycast (transform.position, Vector3.down, out hit, aboveGroundDist)) {
			float newPlace = hit.point.y + distToGround;
			Vector3 current = transform.position;
			transform.position = new Vector3 (current.x, newPlace, current.z);
		}
	}

	public bool CanMakeJump ()
	{
		return true;
	}

}
