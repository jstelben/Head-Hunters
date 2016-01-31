using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ScrollingScript : MonoBehaviour {
	public Vector2 ScrollSpeed = new Vector2(0,0);
	public bool IsLinkedToCamera = false;
	public bool IsLooping = false;

	private List<Transform> backgroundPart;
	// Use this for initialization
	void Start () {
		if(IsLooping) {
			backgroundPart = new List<Transform>();

			for(int i = 0; i < transform.childCount; i++) {
				Transform child = transform.GetChild(i);
				if(child.GetComponent<Renderer>() != null) {
					backgroundPart.Add(child);
				}
			}

			backgroundPart = backgroundPart.OrderBy(t => t.position.x).ToList();

		}
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log(Time.deltaTime);
		transform.Translate(ScrollSpeed * Time.deltaTime);
		if(IsLinkedToCamera) {
			Camera.main.transform.Translate(ScrollSpeed);
		}

		if(IsLooping) {
			Transform firstChild = backgroundPart.FirstOrDefault();
			if(firstChild != null) {
				if (firstChild.position.x < Camera.main.transform.position.x) {
					if (firstChild.GetComponent<Renderer>().IsVisibleFrom(Camera.main) == false) {
						Transform lastChild = backgroundPart.LastOrDefault();
						Vector3 lastPosition = lastChild.transform.position;
						Vector3 lastSize = (lastChild.GetComponent<Renderer>().bounds.max - lastChild.GetComponent<Renderer>().bounds.min);
						firstChild.position = new Vector3(lastPosition.x + lastSize.x, firstChild.position.y, firstChild.position.z);

						backgroundPart.Remove(firstChild);
						backgroundPart.Add(firstChild);
					}
				}
			}
		}
	}
}
