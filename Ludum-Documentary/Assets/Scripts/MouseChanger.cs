using UnityEngine;
using System.Collections;

public class MouseChanger : MonoBehaviour
{
	/** Gravity Textures */
	public Texture2D gravityOn;
	public Texture2D gravityOff;

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
	public Vector2 hotSpot = Vector2.zero;

	void Update ()
	{
		if (Input.GetKey (KeyCode.Q)) {
			Cursor.SetCursor (gravityOn, hotSpot, cursorMode);
		} else if (Input.GetKey (KeyCode.A)) {
			Cursor.SetCursor (gravityOff, hotSpot, cursorMode);
		} else if (Input.GetKey (KeyCode.W)) {
			Cursor.SetCursor (currentOn, hotSpot, cursorMode);
		} else if (Input.GetKey (KeyCode.S)) {
			Cursor.SetCursor (currentOff, hotSpot, cursorMode);
		} else if (Input.GetKey (KeyCode.E)) {
			Cursor.SetCursor (weakForceOn, hotSpot, cursorMode);
		} else if (Input.GetKey (KeyCode.D)) {
			Cursor.SetCursor (weakForceOff, hotSpot, cursorMode);
		} else if (Input.GetKey (KeyCode.R)) {
			Cursor.SetCursor (strongForceOn, hotSpot, cursorMode);
		} else if (Input.GetKey (KeyCode.F)) {
			Cursor.SetCursor (strongForceOff, hotSpot, cursorMode);
		} else {
			Cursor.SetCursor (null, Vector2.zero, cursorMode);
		}
	}
}
