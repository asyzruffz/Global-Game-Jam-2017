using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {

	[Header ("Hint Arrow")]
	public float blinkInterval = 0.5f;

	[Header("Reference")]
	public Image leftBlinker;
	public Image rightBlinker;
	public Radio radio;

	private float blinkTimer = 0;
	private bool blinkBool;
	private bool frequencyAtLeft;
	private bool frequencyAtRight;

	void Start () {
		
	}
	
	void Update () {
		FrequencyCheck ();
		Blink ();
	}

	void FrequencyCheck() {
		bool notAtRange = !radio.isSignalClear;

		// Check the nearest station if not within a range
		if (notAtRange) {
			if (radio.stationList[radio.nearestStationIndex].frequency < radio.frequency)
				frequencyAtLeft = true;
			else
				frequencyAtLeft = false;

			if (radio.stationList[radio.nearestStationIndex].frequency > radio.frequency)
				frequencyAtRight = true;
			else
				frequencyAtRight = false;
		} 
		else 
		{
			frequencyAtLeft = true;
			frequencyAtRight = true;
		}

	}

	void Blink() {
		// blink on and off each interval
		if(blinkTimer >= blinkInterval) {
			blinkBool = !blinkBool;
			blinkTimer = 0;
		}
		
		// blink the left arrow to hint towards left
		if (frequencyAtLeft) {
			leftBlinker.enabled = blinkBool;
		}
		else {
			leftBlinker.enabled = false;
		}

		// blink the right arrow to hint towards right
		if (frequencyAtRight) {
			rightBlinker.enabled = blinkBool;
		}
		else {
			rightBlinker.enabled = false;
		}

		// blink both if the frequency is correct
		if (frequencyAtLeft && frequencyAtRight) {
			leftBlinker.enabled = true;
			rightBlinker.enabled = true;
		}

		// Update blink timer
		blinkTimer += Time.deltaTime;
	}

	public void OnDrag(float value) {
		radio.frequency = Mathf.Lerp(88.0f, 108.0f, value);
	}
}
