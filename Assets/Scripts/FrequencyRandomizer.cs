using UnityEngine;

public class FrequencyRandomizer : MonoBehaviour {

	public float changeInterval = 1;

	private Radio radio;
	private float timer = 0;

	void Start () {
		radio = GetComponent<Radio> ();
	}
	
	void Update () {
		// Change the frequency at fixed interval
		if(timer >= changeInterval) {
			string freq = "";
			for (int i = 0; i < radio.stationList.Length; i++) {
				float randomFrequency = Random.Range (88.0f, 108.0f);
				freq += randomFrequency + " ";
				radio.stationList[i].frequency = randomFrequency;
			}

			timer = 0;
		}

		timer += Time.deltaTime;
	}
}
