using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour 
{
	[Header("Clock")]
	public float timer;
	public int minutes, seconds, fraction;

	[Header("Night Time")]
	public float timeMultiplier = 1;
	public int dawnTime = 6;

	private Text timerText;

	void Awake() {
		timerText = GetComponent<Text> ();

		timer = 0.0f;
	}

	void FixedUpdate()
	{
		timer += timeMultiplier * Time.deltaTime;
		timer = Mathf.Max (timer, 0);

		minutes = (int) timer / 60;
		seconds = (int) timer % 60;

		timerText.text = string.Format ("{0:00} : {1:00} AM", minutes, seconds);

		if (minutes >= dawnTime) {
			GameController.UIState = GameUIState.GAMEOVER;
			GameController.hasWon = true;
		}
	}
}

