using UnityEngine;
using System.Collections;

public class ManipulatableObject : MonoBehaviour
{

	public bool metal; // Affected by magnetism

	public bool canBeRadioactive; // Whether the object can become radioactive

	public bool isRadioactive; // Produces radiation

	public bool hasMass = true; // Affected by gravity

	public bool destructable; // Can break apart

	public bool crushable; // Can be crushed by a gravity well

	public bool powerable; // Can be powered by an electic current

	private bool radioactiveColor; // Whether the radioactive color has already been applied

	public bool powered { get; set; } // Whether the object has been given power

	private bool crushed { get; set; } // Whether the object has been crushed

	private bool destroyed { get; set; } // Whether the objecty has been destroyed

	public Transform radioactive; // The radioactive particle effect prefab

	private GameObject radioactiveObject; // The gameobject of the radioactive particle effect prefab

	private LevelSettingsManager levelSettings; // The level settings

	void Start ()
	{
		GameObject [] temp = GameObject.FindGameObjectsWithTag ("GameController");
		foreach (GameObject go in temp) {
			if (go.name == "_LevelSettings") {
				levelSettings = go.GetComponent<LevelSettingsManager> ();
			}
		}

		// Don't let a radioactive object be declared to be unable to be radioactive
		if (isRadioactive) {
			canBeRadioactive = true;
		}
	}

	void Update ()
	{
		// If this object falls below the level boundry, disable it
		if (transform.position.y < levelSettings.levelBottom) {
			Debug.Log ("DESTROY!");
			if (this.gameObject.name == "Character") {
				levelSettings.isPlayerDead = true;
			} else {
				this.gameObject.SetActive (false);
			}
		}
		if (transform.position.z != 0.0f) {
			transform.position = new Vector3 (transform.position.x, transform.position.y, 0.0f);
		}
		giveRadioactiveColor ();
	}

	public void crush ()
	{
		if (!crushed) {
			gameObject.transform.localScale = gameObject.transform.localScale / 2;
			crushed = true;
		}
		gameObject.GetComponent<Rigidbody> ().isKinematic = false;
		gameObject.GetComponent<Rigidbody> ().useGravity = true;
	}

	public void giveRadioactiveColor ()
	{
		if (canBeRadioactive && isRadioactive && !radioactiveColor) {
			Material m = GetComponent<Renderer> ().material;
			m.color = new Color (m.color.r, m.color.g + 0.4f, m.color.b);
			radioactiveColor = true;
			Transform trans = Instantiate (radioactive);
			radioactiveObject = trans.gameObject;
			radioactiveObject.transform.parent = this.gameObject.transform;
			radioactiveObject.transform.localPosition = Vector3.zero;
			radioactiveObject.transform.rotation = this.gameObject.transform.rotation;
		} else if (!isRadioactive && radioactiveColor) {
			Material m = GetComponent<Renderer> ().material;
			m.color = new Color (m.color.r, m.color.g - 0.4f, m.color.b);
			
			radioactiveColor = false;
			Destroy (radioactiveObject);
		}
	}
}
