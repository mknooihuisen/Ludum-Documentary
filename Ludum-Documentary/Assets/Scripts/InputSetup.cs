using UnityEngine;
using System.Collections;

public class InputSetup : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
		cInput.SetKey ("GravityWell", Keys.Q, Keys.Keypad4);
		cInput.SetKey ("GravityShift", Keys.A, Keys.Keypad1);
		cInput.SetKey ("Magnetic", Keys.W, Keys.Keypad5);
		cInput.SetKey ("Electric", Keys.S, Keys.Keypad2);
		cInput.SetKey ("Weak", Keys.E, Keys.Keypad6);
		cInput.SetKey ("Strong", Keys.R, Keys.Keypad3);
		cInput.SetKey ("Up", Keys.Mouse0);
		cInput.SetKey ("Down", Keys.Mouse1);
	}
}
