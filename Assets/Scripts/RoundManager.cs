using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour {
    private static RoundManager _instance;

    public static RoundManager Instance {
        get {
            if (_instance is null) {
                Debug.LogError("RoundManager is NULL");
            }
            return _instance;
        }
    }

    public int Score { get; set; }
    public float TimeLeft { get; private set; }
    public bool RoundOver { get; private set; }
    public int CurrentRound { get; private set; }

    [Header("Objects")]
    public UIController HUD;
    public OverlayController overlayController;
    public AudioSource backgroundMusic;

    [Header("Variables")]
    public int roundTime = 30;

    void Awake() {
        _instance = this;
    }

    void Start() {
        RoundOver = true;
        CurrentRound = 1;

        HUD.SetTimeLeft(roundTime);
        overlayController.SetGameStart();
    }

    void Update() {
        // Wait for key press to start round
        if (RoundOver && Input.anyKeyDown) {
            StartRound();
        }

        if (!RoundOver) {
            // Update round timer
            if (TimeLeft <= float.Epsilon) {
                TimeLeft = 0;
                EndRound();
                CurrentRound++;
            } else {
                TimeLeft -= Time.deltaTime;
            }

            // Update HUD
            HUD.SetTimeLeft(TimeLeft);
            HUD.SetScore(Score);
        }
    }

    public void StartRound() {
        // Start background music when first round starts
        if (CurrentRound == 1) {
            backgroundMusic.Play();
        }

        Score = 0;

        SpawnRings();

        TimeLeft = roundTime;
        RoundOver = false;

        overlayController.DisableAll();
    }

    public void EndRound() {
        RoundOver = true;
        overlayController.SetRoundOver(CurrentRound);
    }

    public void SpawnRings() {

    }
}
