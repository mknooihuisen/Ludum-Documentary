using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreScript : MonoBehaviour
{

	private Text scoreText;

	public Text shadowText;

	private int score = -1;

	private LevelSettingsManager levelSettings;

	// Use this for initialization
	void Start ()
	{
		//Dynamically grab level settings
		GameObject [] temp = GameObject.FindGameObjectsWithTag ("GameController");
		foreach (GameObject go in temp) {
			if (go.name == "_LevelSettings") {
				levelSettings = go.GetComponent<LevelSettingsManager> ();
			}
		}

		scoreText = gameObject.GetComponent<Text> ();
		scoreText.text = " ";
		shadowText.text = scoreText.text;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (score == -1 && levelSettings.endReached) {
			calculateScore ();
			scoreText.text = "Score: " + string.Format ("{0:n0}", score);
			shadowText.text = scoreText.text;
		}
	}

	void calculateScore ()
	{
		float energyLeft = levelSettings.energy;
		float timeTaken = Time.timeSinceLevelLoad;
		// Score is energy remaining squared divided by 10
		// This makes 10,000,000 the max score with 10,000 energy
		float floatScore = Mathf.Pow (energyLeft, 2.0f) / 10.0f;
		score = (int)floatScore;
	}
}
