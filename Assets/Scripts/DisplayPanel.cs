using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayPanel : MonoBehaviour {

    public GameObject menuPanel;
    public GameObject optionPanel;
    public GameObject pausePanel;
    public GameObject gameOverPanel;

	public void ShowMenuPanel(bool show) {
        menuPanel.SetActive(show);
    }

    public void ShowOptionPanel(bool show) {
        optionPanel.SetActive(show);
    }

    public void ShowPausePanel(bool show) {
        pausePanel.SetActive(show);
	}

	public void ShowGameOverPanel (bool show) {
		gameOverPanel.SetActive (show);
	}
}
