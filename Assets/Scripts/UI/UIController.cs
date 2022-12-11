using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class UIController : MonoBehaviour {
    public Button toggleShortcutsOverlay;
    public XFormController xFormController;

    public GameObject root;
    public GameObject shortcutsOverlay;

    public TMPro.TMP_Text timerText;
    public TMPro.TMP_Text scoreText;

    private int previousScore = 0;

    void Start() {
        // spawnIntervalSlider.slider.onValueChanged.AddListener(value => updateLinesFromSliders());
        // speedSlider.slider.onValueChanged.AddListener(value => updateLinesFromSliders());
        // lifespanSlider.slider.onValueChanged.AddListener(value => updateLinesFromSliders());

        toggleShortcutsOverlay.onClick.AddListener(ToggleShortcutsOverlay);
    }

    void ToggleShortcutsOverlay() {
        shortcutsOverlay.SetActive(!shortcutsOverlay.activeSelf);
    }

    public void SetTimeLeft(float timeLeft) {
        timerText.text = "0:" + timeLeft.ToString("00");
    }

    public void SetScore(int score) {
        int scoreDiff = score - previousScore;

        SpawnScoreChangePopup(scoreDiff);

        scoreText.text = score.ToString("n0");
        previousScore = score;
    }

    private void SpawnScoreChangePopup(int value) {
        // todo...
    }
}
