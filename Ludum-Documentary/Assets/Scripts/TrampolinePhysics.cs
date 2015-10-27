using UnityEngine;
using System.Collections;

public class TrampolinePhysics : MonoBehaviour
{

	private Rigidbody player;

	private float bounceback;

	void OnTriggerEnter (Collider other)
	{
		if (other.GetComponent<Rigidbody> () != null) {
			player = other.gameObject.GetComponent<Rigidbody> ();
			bounceback = -player.velocity.y;
			Debug.Log (bounceback);
			player.velocity = new Vector3 (player.velocity.x, 0, 0);
		}
	}

	void Update ()
	{
		if (player != null) {
			player.velocity = new Vector3 (player.velocity.x, bounceback, 0);
			player = null;
		}
	}
}
