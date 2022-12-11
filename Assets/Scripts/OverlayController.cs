using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayController : MonoBehaviour {
    [Header("Overlays")]
    public GameObject gameStart;
    public GameObject roundOver;
    public GameObject gameOver;

    [Header("Text Meshes")]
    public TMPro.TMP_Text roundText;
    public TMPro.TMP_Text gameOverText;

    void Start() {
        SetGameStart();
    }

    public void DisableAll() {
        gameStart.SetActive(false);
        roundOver.SetActive(false);
        gameOver.SetActive(false);
    }

    public void SetGameStart() {
        DisableAll();
        gameStart.SetActive(true);
    }

    public void SetRoundOver(int round) {
        DisableAll();

        roundText.text = string.Format("Round {0} over!", round.ToString());

        roundOver.SetActive(true);
    }

    public void SetGameOver(int score) {
        DisableAll();

        gameOverText.text = string.Format("You scored <b>{0}</b> points!", score.ToString("n0"));

        gameOver.SetActive(true);
    }
}
