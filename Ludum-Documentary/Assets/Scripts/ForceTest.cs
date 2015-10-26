using UnityEngine;
using System.Collections;

public class ForceTest : MonoBehaviour {

	public float thrust;
	public Rigidbody rb;


	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKey(KeyCode.Space)) {
			rb.AddForce(transform.right * thrust);
			Debug.Log("UP!");

		}else if (Input.GetKey(KeyCode.RightArrow)) {
			transform.Translate(transform.right *Time.deltaTime);
		}
	
	}
}
