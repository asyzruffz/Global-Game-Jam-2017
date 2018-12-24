using UnityEngine;
using UnityEngine.UI;

public class ScreenRadio : MonoBehaviour {

	public Radio radio;

	private Text screenText;

	void Start () {
		screenText = GetComponent<Text> ();
	}
	
	void Update () {
		if(radio.isSignalClear) {
			screenText.text = radio.stationName + string.Format (" - {0:0.##}", radio.stationList[radio.nearestStationIndex].frequency);
		} else {
			screenText.text = "- - - - - - - - - - - -";
		}
	}
}
