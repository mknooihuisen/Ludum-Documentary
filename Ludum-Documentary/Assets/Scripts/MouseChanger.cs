using UnityEngine;
using System.Collections;

public class MouseChanger : MonoBehaviour
{

	/** Gravity Textures */
	public Texture2D gravityOn;
	public Texture2D gravityOff;
	public Texture2D gravityShiftOn;
	public Texture2D gravityShiftOff;

	/** Electromagnetism Textures */
	public Texture2D currentOn;
	public Texture2D currentOff;
	public Texture2D magnetOn;
	public Texture2D magnetOff;

	/** Weak Force Textures */
	public Texture2D weakForceOn;
	public Texture2D weakForceOff;

	/** Strong Force Textures */
	public Texture2D strongForceOn;
	public Texture2D strongForceOff;

	public CursorMode cursorMode = CursorMode.Auto;
	public Vector2 hotSpot = new Vector2 (100.0f, 0.0f);
	public bool characterDead;
	private LevelSettingsManager levelSettings;

	void Start ()
	{
		GameObject [] temp = GameObject.FindGameObjectsWithTag ("GameController");
		foreach (GameObject go in temp) {
			if (go.name == "_LevelSettings") {
				levelSettings = go.GetComponent<LevelSettingsManager> ();
			}
		}
	}

	void Update ()
	{
		if (levelSettings.isPlayerDead) {
			Cursor.SetCursor (null, Vector2.zero, cursorMode);
			return;
		}
		if (cInput.GetKey ("GravityWell")) {
			if (cInput.GetKey ("Down")) {
				Cursor.SetCursor (gravityOff, hotSpot, cursorMode);
			} else {
				Cursor.SetCursor (gravityOn, hotSpot, cursorMode);
			}
		} else if (cInput.GetKey ("GravityShift")) {
			if (cInput.GetKey ("Down")) {
				Cursor.SetCursor (gravityShiftOff, hotSpot, cursorMode);
			} else {
				Cursor.SetCursor (gravityShiftOn, hotSpot, cursorMode);
			}
		} else if (cInput.GetKey ("Magnetic")) {
			if (cInput.GetKey ("Down")) {
				Cursor.SetCursor (magnetOff, hotSpot, cursorMode);
			} else {
				Cursor.SetCursor (magnetOn, hotSpot, cursorMode);
			}
		} else if (cInput.GetKey ("Electric")) {
			if (cInput.GetKey ("Down")) {
				Cursor.SetCursor (currentOff, hotSpot, cursorMode);
			} else {
				Cursor.SetCursor (currentOn, hotSpot, cursorMode);
			}
		} else if (cInput.GetKey ("Weak")) {
			if (cInput.GetKey ("Down")) {
				Cursor.SetCursor (weakForceOff, hotSpot, cursorMode);
			} else {
				Cursor.SetCursor (weakForceOn, hotSpot, cursorMode);
			}
		} else if (cInput.GetKey ("Strong")) {
			if (cInput.GetKey ("Down")) {
				Cursor.SetCursor (strongForceOff, hotSpot, cursorMode);
			} else {
				Cursor.SetCursor (strongForceOn, hotSpot, cursorMode);
			}
		} else {
			Cursor.SetCursor (null, Vector2.zero, cursorMode);
		}
	}
}
