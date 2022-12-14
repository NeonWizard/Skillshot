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
    public int RingsHit { get; private set; }

    [Header("Objects")]
    public UIController HUD;
    public OverlayController overlayController;
    public AudioSource backgroundMusic;

    [Header("Prefabs")]
    public GameObject ringPrefab;

    [Header("Variables")]
    public int roundTime = 30;
    public BoxCollider ringSpawnArea;
    public int basePointsPerRing = 10;

    [Header("Sounds")]
    public AudioClip scorePoints;
    public AudioClip gameOver; // todo
    public AudioClip roundOver; // todo

    private List<GameObject> rings = new List<GameObject>();

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
        DeleteAllBalls();

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
        // Delete old rings
        foreach (GameObject ring in rings) {
            Destroy(ring);
        }
        rings.Clear();

        // Spawn new rings
        int numToSpawn = (int)Mathf.Min(Mathf.Floor(Random.Range(2, 4) + Random.Range(0, CurrentRound)), 8);
        for (int i = 0; i < numToSpawn; i++) {
            GameObject ring = Instantiate(ringPrefab);

            Vector3 colliderPoint = GetRandomPointInsideCollider(ringSpawnArea);
            ring.transform.position = ringSpawnArea.transform.TransformPoint(colliderPoint);
            if (colliderPoint.x > 0) {
                Vector3 oldScale = ring.transform.localScale;
                oldScale.x = -1;
                ring.transform.localScale = oldScale;
            }

            rings.Add(ring);
        }
    }

    // Called whenever a ring is hit
    public void RingHit() {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.pitch = 1f + 0.1f * RingsHit;
        audioSource.PlayOneShot(scorePoints);

        Score += basePointsPerRing + (int)Mathf.Pow(2, RingsHit);
        RingsHit++;
    }

    // todo: move to a utils static singleton or something
    public static Vector3 GetRandomPointInsideCollider(BoxCollider boxCollider) {
        Vector3 extents = boxCollider.size / 2f;
        Vector3 point = new Vector3(
            Random.Range(-extents.x, extents.x),
            Random.Range(-extents.y, extents.y),
            Random.Range(-extents.z, extents.z)
        );
        return point;
    }

    public void DeleteAllBalls() {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("CrushableBalls");
        foreach (GameObject ball in balls) {
            Destroy(ball);
        }
    }
}
