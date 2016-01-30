using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour {
	public float startTime;

	private Text timerText;
	// Use this for initialization
	void Start () {
		timerText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		startTime -= Time.deltaTime;
		timerText.text = string.Format("{1}:{0}", (int)startTime % 60, (int)startTime / 60);
		//timerText.text = startTime.ToString();
	}

	public void IncreaseTime(float increase) {
		startTime += increase;
	}
}
