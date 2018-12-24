using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public enum GameUIState {
    MAINMENU,
    INGAME,
    PAUSED,
    GAMEOVER
}

public class GameController : MonoBehaviour {

    // Main code to control the gameplay here. Winning/Losing, scene transition, etc ...

    public static GameUIState UIState = GameUIState.MAINMENU;
	public static bool hasWon;

	[Header ("Player")]
    public PlayerController player;
	public string playerTime;

	[Header ("UI")]
    public string gameSceneName;
    public DisplayPanel panels;
	public Text gameOverText;
	public Slider volumeSlider;

	[Header ("Audio")]
	public AudioMixer mixer;

    void Start () {
		hasWon = false;
	}

	void OnEnable () {
		//Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	void OnDisable () {
		//Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}

	void OnLevelFinishedLoading (Scene scene, LoadSceneMode mode) {
		GameObject play = GameObject.FindGameObjectWithTag ("Player");
		if (play) {
			player = play.GetComponent<PlayerController> ();
		} else {
			Debug.Log ("No player found!");
		}
	}

	void Update () {

        switch (UIState) {
            case GameUIState.MAINMENU:
                if (Input.GetButtonDown("Cancel")) {
                    ExitGame();
                }

                break;

            case GameUIState.INGAME:
                if (Input.GetButtonDown("Cancel")) {
                    panels.ShowPausePanel(true);

                    //Call the DoPause function to pause the game
                    DoPause ();
                }

				if (player) {
					if (player.IsDead()) {
						Timer timing = GameObject.FindObjectOfType<Timer> ();
						if (timing) {
							playerTime = string.Format ("{0:00} : {1:00} AM", timing.minutes, timing.seconds);
						}

						UIState = GameUIState.GAMEOVER;
					}
				}

                break;
                
            case GameUIState.PAUSED:
				mixer.SetFloat ("MasterVolume", -80);

				if (Input.GetButtonDown("Cancel")) {
                    panels.ShowPausePanel(false);

                    //Call the UnPause function to unpause the game
                    UnPause ();
                }

				break;

            case GameUIState.GAMEOVER:
				if (hasWon) {
					mixer.SetFloat ("MelodyVolume", 0);
					mixer.SetFloat ("HorrorVolume", -80);
				} else {
					mixer.SetFloat ("MelodyVolume", -80);
					mixer.SetFloat ("HorrorVolume", 0);
				}
				mixer.SetFloat ("NoiseVolume", -80);

				GamingOver ();
				panels.ShowGameOverPanel (true);

				if (Input.GetButtonDown ("Cancel")) {
					panels.ShowPausePanel (true);

					//Call the DoPause function to pause the game
					DoPause ();
				}

				break;
        }
    }

    public void StartGame() {
		// Reset status
		hasWon = false;

		SceneManager.LoadScene(gameSceneName);
        UIState = GameUIState.INGAME;
    }
    
    public void ExitGame () {
        Application.Quit ();
    }

    public void OptionBack() {
        if (UIState == GameUIState.MAINMENU) {
            panels.ShowMenuPanel(true);
        } else if (UIState == GameUIState.PAUSED) {
            panels.ShowPausePanel(true);
        }
    }

    public void DoPause () {
        Time.timeScale = 0;

        UIState = GameUIState.PAUSED;
    }

    public void UnPause () {
        Time.timeScale = 1;
		
		AdjustAudioVolume (volumeSlider.value);
		UIState = GameUIState.INGAME;
    }

    public void GoToMenu() {
		UnPause();
		panels.ShowGameOverPanel (false);
		SceneManager.LoadScene(0);
		
        UIState = GameUIState.MAINMENU;
    }

	public void GamingOver() {
		if (hasWon) {
			gameOverText.text = "You survived the night!";
		} else {
			gameOverText.text = "You maintained your sanity until " + playerTime;
		}
	}

	public void AdjustAudioVolume(float value) {
		float linearValue = Mathf.Lerp (-20, 0, value);
		linearValue = (value > 0.05) ? linearValue : -80;
		mixer.SetFloat ("MasterVolume", linearValue);
	}
}
