using UnityEngine;
using System.Collections;

public class ScrollingScript : MonoBehaviour {
	public Vector2 ScrollSpeed = new Vector2(0,0);
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
		transform.Translate(ScrollSpeed);
	}
}
