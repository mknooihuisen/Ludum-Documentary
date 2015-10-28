using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelSettingsManager : MonoBehaviour {

	private bool gravActive, magActive, weakActive, strongActive;

	public bool isPlayerDead { get; set; }

	public GameObject startPoint { get; set; }

	private GameObject character;

	public float energy = 10000.0f;

	// Use this for initialization
	void Start () {
		isPlayerDead = false;

		//get character
		character = GameObject.FindGameObjectWithTag ("Player");

		//send character to initial spawn point
		respawn ();
	}
	
	// Update is called once per frame
	void Update () {
	
		if (isPlayerDead) {
			//Debug.Log("Ouch!");
		} 

		//TODO: Move this, or remove it
		if (Input.GetKeyDown (KeyCode.Space)) {
			respawn ();
		}
	}

	private void respawn() {
		if (startPoint != null) {

			character.GetComponent<Rigidbody>().velocity = Vector3.zero;
			character.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

			character.transform.position = startPoint.transform.position;

			character.GetComponent<Rigidbody>().velocity = Vector3.zero;
			character.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

			character.GetComponent<CharacterNavigate>().dead = false;

			//re-add cached navs
			NavPointCache script = character.GetComponent<NavPointCache>();
			List<GameObject> cache = script.cache;

			foreach(GameObject go in cache) {
				go.SetActive(true);
			}
		}
	}

	public void toggleActive() {

	}
}
