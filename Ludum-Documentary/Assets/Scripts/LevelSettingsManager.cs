using UnityEngine;
using System.Collections;

public class LevelSettingsManager : MonoBehaviour {

	private bool gravActive, magActive, weakActive, strongActive;

	public bool isPlayerDead { get; set; }

	// Use this for initialization
	void Start () {
		isPlayerDead = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void toggleActive() {

	}
}
