using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public Radio radio;
	public Animator spotlight;

	private Animator anim;

	void Start () {
		anim = GetComponent<Animator> ();
	}

	void Update () {
		anim.SetBool ("isCalm", radio.isSignalClear && radio.stationName == "Calm FM");
		GetComponent<InsanityManager> ().Deteriorate ((radio.isSignalClear && radio.stationName == "Horror FM") ? 1 : 0);

		if (GameController.UIState == GameUIState.GAMEOVER) {
			if (!GameController.hasWon) {
				spotlight.SetBool ("isRed", GameController.hasWon);
				GetComponent<CameraShake> ().smooth = true;
				GetComponent<CameraShake> ().ShakeCamera (1, 10);
			}
		}
	}

	public bool IsDead() {
		return GetComponent<InsanityManager> ().currentSanity <= 0;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.CompareTag("Ghost")) {
			Destroy(other.gameObject, 2);
		}
	}
}
