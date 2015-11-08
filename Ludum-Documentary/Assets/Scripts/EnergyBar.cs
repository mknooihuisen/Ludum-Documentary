using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnergyBar : MonoBehaviour
{
	public Text energyTotal;

	private RectTransform barSize;

	private float totalEnergy;
	
	private LevelSettingsManager levelSettings;

	void Start ()
	{
		barSize = GetComponent<RectTransform> ();
		setPercent (1.0f);
		GameObject[] temp = GameObject.FindGameObjectsWithTag ("GameController");
		foreach (GameObject go in temp) {
			if (go.name == "_LevelSettings") {
				levelSettings = go.GetComponent<LevelSettingsManager> ();
			}
		}
		totalEnergy = levelSettings.energy;
	}

	void Update ()
	{
		setPercent (levelSettings.energy / totalEnergy);
		int energyInt = (int)levelSettings.energy + 0;
		energyTotal.text = "" + string.Format ("{0:n0}", energyInt);
	}

	void setPercent (float percent)
	{
		if (percent <= 0.0f) {
			barSize.sizeDelta = Vector2.zero;
		} else {
			barSize.sizeDelta = new Vector2 (200.0f * percent, barSize.sizeDelta.y);
			barSize.localPosition = new Vector3 ((-100.0f * percent) + 100, barSize.transform.localPosition.y, barSize.transform.localPosition.z);
		}

	}
}
