using System;
using UnityEngine;
using UnityEngine.Audio;

public class Radio : MonoBehaviour {

	const float minFrequency = 88;
	const float maxFrequency = 108;

	[Header ("Radio")]
	[Range(minFrequency, maxFrequency)]
    public float frequency = minFrequency; // in MHz
	public bool isSignalClear;
	public string stationName;

	[Header ("Stations")]
	public float signalRange = 5;
	//public float signal = minFrequency;
	public RadioStation[] stationList;
	public int nearestStationIndex = -1;

	[Header("Other")]
    public AudioClip staticSound;

	[Header ("References")]
	public AudioMixer radioMixer;

	private AudioSource[] audioSources;

	void Start () {
		audioSources = GetComponents<AudioSource> ();

		// Assign the noise
		if (staticSound) {
			audioSources[0].clip = staticSound;
			audioSources[0].Play ();
		} else {
			Debug.Log ("No noise found!");
		}

		// Assign the songs
		if (stationList.Length > 0) {
			nearestStationIndex = 0;
			stationName = stationList[nearestStationIndex].name;

			audioSources[1].clip = stationList[0].sound;
			audioSources[1].Play ();
			audioSources[2].clip = stationList[1].sound;
			audioSources[2].Play ();
		} else {
			Debug.Log ("No song found!");
		}
	}
	
	void Update () {
		// Clamp the frequency
		frequency = Mathf.Clamp (frequency, minFrequency, maxFrequency);

		// How clear the signal is.
		float attenuation = Mathf.InverseLerp(0, signalRange, NearestStationFrequency ());
		Mathf.Clamp01 (attenuation);

		if (GameController.UIState == GameUIState.INGAME) {
			InterpolateSound (1 - attenuation);
		}

		isSignalClear = (attenuation < 0.3f);
	}

	float NearestStationFrequency () {
		float minFrequencyDistance = 300;
		int newNearestIndex = -1;

		// Loop to check the nearest station
		for(int i = 0; i < stationList.Length; i++) {
			if(Mathf.Abs(frequency - stationList[i].frequency) < minFrequencyDistance) {
				minFrequencyDistance = Mathf.Abs (frequency - stationList[i].frequency);
				newNearestIndex = i;
			}
		}

		// Change the playing song to the nearest station song
		if(newNearestIndex != -1 && newNearestIndex != nearestStationIndex) {
			stationName = stationList[newNearestIndex].name;
			nearestStationIndex = newNearestIndex;
		}

		return minFrequencyDistance;
	}

	void InterpolateSound(float value) {
		switch(nearestStationIndex) {
			case 0:
				radioMixer.SetFloat ("MelodyVolume", MapValueToVolume (value));
				radioMixer.SetFloat ("HorrorVolume", MapValueToVolume (0));
				break;
			case 1:
				radioMixer.SetFloat ("MelodyVolume", MapValueToVolume (0));
				radioMixer.SetFloat ("HorrorVolume", MapValueToVolume (value));
				break;
		}
		radioMixer.SetFloat ("NoiseVolume", MapValueToVolume (1 - value));
	}
	
	float MapValueToVolume(float value) {
		if(value < 0.01f) {
			// Totally mute it below 0.01 threshold
			return -80;
		} else {
			return Mathf.Lerp (-20, 0, value);
		}
	}
	
	public void SetRadioMute (bool muted) {
		radioMixer.SetFloat ("MasterVolume", MapValueToVolume (muted ? 0 : 1));
	}

	[Serializable]
	public struct RadioStation {
		public string name;
		public float frequency;
		public AudioClip sound;
	}
}
