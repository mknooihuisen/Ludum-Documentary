using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour
{

	public bool powered;

	public bool goingUp;

	public float maxHeight;

	public float minHeight;

	private float height;

	private static float SPEED = 0.05f;

	private bool atMaxMin;

	private int waitTime = 120;

	private int waitCounter;

	private ManipulatableObject mo;

	private bool wasPowered;

	void Start ()
	{
		mo = gameObject.GetComponent<ManipulatableObject> ();
		mo.powered = powered;
		wasPowered = powered;
	}

	void Update ()
	{
		if (mo.powered && !wasPowered) {
			Renderer rend = gameObject.GetComponent<Renderer> ();
			rend.material.color = new Color (rend.material.color.r + 0.7f, rend.material.color.g + 0.7f, rend.material.color.b + 0.7f);
			wasPowered = true;
		} else if (!mo.powered && wasPowered) {
			Renderer rend = gameObject.GetComponent<Renderer> ();
			rend.material.color = new Color (rend.material.color.r - 0.7f, rend.material.color.g - 0.7f, rend.material.color.b - 0.7f);
			wasPowered = false;
		}
		if (mo.powered && !atMaxMin) {
			height = this.transform.position.y;
			if (goingUp) {
				this.transform.Translate (new Vector3 (0, SPEED, 0));
				if (this.transform.position.y >= maxHeight) {
					this.transform.position = new Vector3 (this.transform.position.x, maxHeight, 0);
					goingUp = !goingUp;
					atMaxMin = true;
				}
			} else if (!goingUp && height > minHeight) {
				this.transform.Translate (new Vector3 (0, -SPEED, 0));
				if (this.transform.position.y <= minHeight) {
					this.transform.position = new Vector3 (this.transform.position.x, minHeight, 0);
					goingUp = !goingUp;
					atMaxMin = true;
				}
			}
		} else if (mo.powered) {
			if (waitCounter == waitTime) {
				waitCounter = 0;
				atMaxMin = false;
			} else {
				waitCounter++;
			}
		}
	}
}
