using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Text>().text = this.GetHeadCount().ToString();
		this.SetCurrentTime(90);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
