using UnityEngine;
using System.Collections;

public class ManipulatableObject : MonoBehaviour
{

	public bool metal; // Affected by magnetism

	public bool radioactive; // Produces radiation

	public bool hasMass; // Affected by gravity

	public bool destructable; // Can break apart

	public bool crushable; // Can be crushed by a gravity well

	public bool powerable; // Can be powered by an electic current

	private bool radioactiveColor; // Whether the radioactive color has already been applied

	private bool powered { get; set; } // Whether the object has been given power

	private bool crushed { get; set; } // Whether the object has been crushed

	private bool destroyed { get; set; } // Whether the objecty has been destroyed


	void Update ()
	{
		if (transform.position.z != 0.0f) {
			transform.position = new Vector3 (transform.position.x, transform.position.y, 0.0f);
		}
		if (radioactive && !radioactiveColor) {
			Material m = GetComponent<Renderer> ().material;
			m.color = new Color (m.color.r, m.color.g + 0.5f, m.color.b);
			radioactiveColor = true;
		} else if (!radioactive && radioactiveColor) {
			Material m = GetComponent<Renderer> ().material;
			m.color = new Color (m.color.r, m.color.g - 0.5f, m.color.b);
			radioactiveColor = true;
		}
	}
}
