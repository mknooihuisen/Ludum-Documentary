using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {

	public float arrivalTime { get; private set; }

	public bool startHere = false;

	private GameObject character;

	private LevelSettingsManager settings;

	// Use this for initialization
	void Start () {
		character = GameObject.FindGameObjectWithTag ("Player");
		GameObject [] controllers = GameObject.FindGameObjectsWithTag ("GameController");

		//get the level settings script
		foreach (GameObject go in controllers) {
			if(go.name == "_LevelSettings") {
				settings = go.GetComponent<LevelSettingsManager>();
				break;
			}
		}
	}

	//Update checkpoint
	void OnTriggerEnter (Collider col) {

		if (col.gameObject == character) {
			arrivalTime = Time.timeSinceLevelLoad;
			settings.startPoint = this.gameObject;
		}
	
	}
}
