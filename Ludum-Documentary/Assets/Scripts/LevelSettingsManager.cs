using UnityEngine;
using System.Collections;

public class LevelSettingsManager : MonoBehaviour
{

	private bool gravActive, magActive, weakActive, strongActive;

	public bool isPlayerDead { get; set; }

	public float energy = 10000.0f;

	// Use this for initialization
	void Start ()
	{
		isPlayerDead = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Debug.Log (energy);
		if (isPlayerDead) {
			//Debug.Log("Ouch!");
		} 
	}

	public void toggleActive ()
	{

	}
}
