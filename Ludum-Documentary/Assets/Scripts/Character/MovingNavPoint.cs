using UnityEngine;
using System.Collections;

public class MovingNavPoint : MonoBehaviour
{

	public GameObject holdTo;

	private Vector3 holdToPosition;

	private float x;

	private float y;

	void Start ()
	{
		holdToPosition = holdTo.transform.position;
		x = this.transform.position.x - holdTo.transform.position.x;
		y = this.transform.position.y - holdTo.transform.position.y;
	}

	void Update ()
	{
		if (holdToPosition != holdTo.transform.position) {
			holdToPosition = holdTo.transform.position;
			this.transform.position = new Vector3 (holdToPosition.x + x, holdToPosition.y + y, 0);
		}
		if (!holdTo.activeSelf) {
			gameObject.SetActive (false);
		}
	}
}
