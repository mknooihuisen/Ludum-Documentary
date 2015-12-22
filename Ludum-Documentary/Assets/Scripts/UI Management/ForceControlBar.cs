using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ForceControlBar : MonoBehaviour
{

	/** Gravity Textures */
	public Sprite gravity;
	public Sprite gravityOn;
	public Sprite gravityOff;
	public GameObject gravityImage;
	public Sprite gravityShift;
	public Sprite gravityShiftOn;
	public Sprite gravityShiftOff;
	public GameObject gravityShiftImage;
	
	/** Electromagnetism Textures */
	public Sprite current;
	public Sprite currentOn;
	public Sprite currentOff;
	public GameObject currentImage;
	public Sprite magnet;
	public Sprite magnetOn;
	public Sprite magnetOff;
	public GameObject magnetImage;
	
	/** Weak Force Textures */
	public Sprite weakForce;
	public Sprite weakForceOn;
	public Sprite weakForceOff;
	public GameObject weakForceImage;
	
	/** Strong Force Textures */
	public Sprite strongForce;
	public Sprite strongForceOn;
	public Sprite strongForceOff;
	public GameObject strongForceImage;

	public Sprite missingForce;
	
	private LevelSettingsManager levelSettings;

	void Start ()
	{
		GameObject[] temp = GameObject.FindGameObjectsWithTag ("GameController");
		foreach (GameObject go in temp) {
			if (go.name == "_LevelSettings") {
				levelSettings = go.GetComponent<LevelSettingsManager> ();
			}
		}
	}

	void Update ()
	{
		SetAllOff ();
		if (levelSettings.isPlayerDead || levelSettings.energy <= 0.0f) {
			return;
		}
		if (cInput.GetKey ("GravityWell") && levelSettings.gravWellActive) {
			if (cInput.GetKey ("Down")) {
				gravityImage.GetComponent<Image> ().sprite = gravityOff;
			} else if (cInput.GetKey ("Up")) {
				gravityImage.GetComponent<Image> ().sprite = gravityOn;
			}
		} else if (cInput.GetKey ("GravityShift") && levelSettings.gravShiftActive) {
			if (cInput.GetKey ("Down")) {
				gravityShiftImage.GetComponent<Image> ().sprite = gravityShiftOff;
			} else if (cInput.GetKey ("Up")) {
				gravityShiftImage.GetComponent<Image> ().sprite = gravityShiftOn;
			}
		} else if (cInput.GetKey ("Magnetic") && levelSettings.magActive) {
			if (cInput.GetKey ("Down")) {
				magnetImage.GetComponent<Image> ().sprite = magnetOff;
			} else if (cInput.GetKey ("Up")) {
				magnetImage.GetComponent<Image> ().sprite = magnetOn;
			}
		} else if (cInput.GetKey ("Electric") && levelSettings.elecActive) {
			if (cInput.GetKey ("Down")) {
				currentImage.GetComponent<Image> ().sprite = currentOff;
			} else if (cInput.GetKey ("Up")) {
				currentImage.GetComponent<Image> ().sprite = currentOn;
			}
		} else if (cInput.GetKey ("Weak") && levelSettings.weakActive) {
			if (cInput.GetKey ("Down")) {
				weakForceImage.GetComponent<Image> ().sprite = weakForceOff;
			} else if (cInput.GetKey ("Up")) {
				weakForceImage.GetComponent<Image> ().sprite = weakForceOn;
			}
		} else if (cInput.GetKey ("Strong") && levelSettings.strongActive) {
			if (cInput.GetKey ("Down")) {
				strongForceImage.GetComponent<Image> ().sprite = strongForceOff;
			} else if (cInput.GetKey ("Up")) {
				strongForceImage.GetComponent<Image> ().sprite = strongForceOn;
			}
		}
	}

	void SetAllOff ()
	{
		if (levelSettings.gravWellActive && gravityImage.GetComponent<Image> ().sprite != gravity) {
			gravityImage.GetComponent<Image> ().sprite = gravity;
		} else if (!levelSettings.gravWellActive && gravityImage.GetComponent<Image> ().sprite != missingForce) {
			gravityImage.GetComponent<Image> ().sprite = missingForce;
		}
		if (levelSettings.gravShiftActive && gravityShiftImage.GetComponent<Image> ().sprite != gravityShift) {
			gravityShiftImage.GetComponent<Image> ().sprite = gravityShift;
		} else if (!levelSettings.gravShiftActive && gravityShiftImage.GetComponent<Image> ().sprite != missingForce) {
			gravityShiftImage.GetComponent<Image> ().sprite = missingForce;
		}
		if (levelSettings.magActive && magnetImage.GetComponent<Image> ().sprite != magnet) {
			magnetImage.GetComponent<Image> ().sprite = magnet;
		} else if (!levelSettings.magActive && magnetImage.GetComponent<Image> ().sprite != missingForce) {
			magnetImage.GetComponent<Image> ().sprite = missingForce;
		}
		if (levelSettings.elecActive && currentImage.GetComponent<Image> ().sprite != current) {
			currentImage.GetComponent<Image> ().sprite = current;
		} else if (!levelSettings.elecActive && currentImage.GetComponent<Image> ().sprite != missingForce) {
			currentImage.GetComponent<Image> ().sprite = missingForce;
		}
		if (levelSettings.weakActive && weakForceImage.GetComponent<Image> ().sprite != weakForce) {
			weakForceImage.GetComponent<Image> ().sprite = weakForce;
		} else if (!levelSettings.weakActive && weakForceImage.GetComponent<Image> ().sprite != missingForce) {
			weakForceImage.GetComponent<Image> ().sprite = missingForce;
		}
		if (levelSettings.strongActive && strongForceImage.GetComponent<Image> ().sprite != strongForce) {
			strongForceImage.GetComponent<Image> ().sprite = strongForce;
		} else if (!levelSettings.strongActive && strongForceImage.GetComponent<Image> ().sprite != missingForce) {
			strongForceImage.GetComponent<Image> ().sprite = missingForce;
		}
	}
}
