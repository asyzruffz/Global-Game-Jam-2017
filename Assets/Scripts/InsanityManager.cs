using UnityEngine;
using UnityEngine.UI;

public class InsanityManager : MonoBehaviour {

	[Header("Sanity")]
	public float currentSanity;
	public float minSanity = 0f;
	public float maxSanity = 100f;
	public float recoverRateSanity = 1f;

	[Header("Insanity")]
    public float dropRateSanity = 1f;
	public float insaneDuration = 3;

	[Header ("HUD")]
	public Image sanityBar;

	private Animator anim;
	private bool isCrazy;
	private float crazeTimer = 0;

	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	void Update () {
		if (GameController.UIState == GameUIState.INGAME) {
			// Sanity decrease over time
			Deteriorate (1);
		}

		if (isCrazy) {
			// Increase the rate of decrease further if crazy
			Deteriorate (dropRateSanity);
			crazeTimer += Time.deltaTime;

			if(crazeTimer >= insaneDuration) {
				isCrazy = false;
			}
		} else if (anim.GetBool("isCalm")) {
			// Recover sanity when calmed
			Deteriorate (-recoverRateSanity);
		}

		currentSanity = Mathf.Clamp (currentSanity, minSanity, maxSanity);
		sanityBar.fillAmount = Mathf.InverseLerp (minSanity, maxSanity, currentSanity);
		anim.SetBool ("isCrazy", isCrazy);
	}

	public void Deteriorate(float amount) {
		currentSanity -= amount * Time.deltaTime;
	}

	void OnTriggerEnter2D (Collider2D other) {
		if(other.gameObject.CompareTag("Ghost")) {
			isCrazy = true;
			crazeTimer = 0;
		}
	}
}
