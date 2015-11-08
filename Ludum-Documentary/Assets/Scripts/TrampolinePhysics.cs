using UnityEngine;
using System.Collections;

public class TrampolinePhysics : MonoBehaviour
{

	private Rigidbody player;

	private float bounceback;

	private int framesTaken;

	private Vector3 correctScale;

	private Vector3 compressedScale;

	private static int BOUNCEBACK_FRAMES = 5;

	void Start ()
	{
		correctScale = this.transform.localScale;
		compressedScale = new Vector3 (this.transform.localScale.x, this.transform.localScale.y / 2, this.transform.localScale.z);
		framesTaken = 1;
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.GetComponent<Rigidbody> () != null) {
			player = other.gameObject.GetComponent<Rigidbody> ();
			bounceback = -player.velocity.y;
			Debug.Log (bounceback);
			player.velocity = new Vector3 (player.velocity.x, 0, 0);
			this.transform.localScale = compressedScale;
			framesTaken = 0;
			// Make it so hitting the trampoline can't kill the player
			if (player.name == "Character") {
				player.GetComponent<CharacterNavigate> ().previousVelocity = player.GetComponent<Rigidbody> ().velocity;
			}
		}
	}

	void Update ()
	{
		if (player != null) {
			player.velocity = new Vector3 (player.velocity.x, bounceback, 0);
			// Make it so hitting the trampoline can't kill the player
			if (player.name == "Character") {
				player.GetComponent<CharacterNavigate> ().previousVelocity = player.GetComponent<Rigidbody> ().velocity;
			}
			player = null;
		}
		if (framesTaken > BOUNCEBACK_FRAMES) {
			this.transform.localScale = correctScale;
		} else if (this.transform.localScale == compressedScale) {
			framesTaken++;
		}
	}
}
