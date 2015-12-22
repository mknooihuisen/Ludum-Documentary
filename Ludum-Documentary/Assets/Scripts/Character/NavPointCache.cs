using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NavPointCache : MonoBehaviour {

	public List<GameObject> cache { get; private set; }

	// Use this for initialization
	void Start () {
		cache = new List<GameObject> ();
	}

	void OnTriggerEnter(Collider col) {

		//If this is a navpoint, store it
		if (col.gameObject.tag == "NavPoint") {
			cache.Add (col.gameObject);
		} else if (col.gameObject.tag == "Respawn") {
			//checkpoint
			cache.Clear();
		}
	}
}
