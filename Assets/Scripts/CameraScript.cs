using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	public float dampTime = 0.15f;

	private Vector3 velocity = Vector3.zero;
	public Transform target;
	public Transform EndOfStage;
	public Transform BottomOfStage;
	private Vector3 endOfScreen;
	// Use this for initialization
	void Start () {
		endOfScreen = GetComponent<Camera>().ScreenToWorldPoint(EndOfStage.position);
		endOfScreen = GetComponent<Camera>().ScreenToViewportPoint(EndOfStage.position);
	}
	
	// Update is called once per frame
	void Update () {
		if(target) {
			Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);
			Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
			Vector3 destination = (Vector3) transform.position + delta;
			
			endOfScreen = GetComponent<Camera>().WorldToViewportPoint(EndOfStage.position);
			Vector3 botOfScreen = GetComponent<Camera>().WorldToViewportPoint(BottomOfStage.position);
			float x = destination.x;
			float y = destination.y;
			//Debug.Log(destination.x + " " + endOfScreen.x);
			if(destination.x <= transform.position.x || endOfScreen.x <= 1.0f) {
				x = transform.position.x;
			}
			if(BottomOfStage.position.y < destination.y) {
				//y = transform.position.y;
				//Debug.Log("wtf");
			}
			transform.position = Vector3.SmoothDamp(transform.position, new Vector3(x, y, destination.z), ref velocity, dampTime);
		}

	}
}
