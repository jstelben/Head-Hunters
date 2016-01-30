using UnityEngine;
using System.Collections;

public class TomahawkScript : MonoBehaviour {
	public Vector2 throwForce;
	public float torqueForce;
	public bool InHand = true;

	// Use this for initialization
	void Start () {
		InHand = true;
		//GetComponent<Rigidbody2D>().isKinematic = true;
		//GetComponent<Rigidbody2D>().detectCollisions = false;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void Throw() {
		InHand = false;
		Destroy(gameObject, 2.5f);
	}
}
