using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelingBall : MonoBehaviour {
    [Range(0.5f, 15f)]
    public float speed = 5f;

    [Range(1, 15)]
    public int lifespan = 10;

    public GameObject shadowPrefab;

    // map of barrier IDs to shadows
    private Dictionary<int, Shadow> shadows = new Dictionary<int, Shadow>();

    void Start() {
        // Destroy the ball after (lifespan) seconds
        Destroy(gameObject, lifespan);
    }

    void Update() {
        Barrier[] barriers = GameObject.FindObjectsOfType<Barrier>();

        // Move forwards at (speed) units per second
        Vector3 destination = transform.position + transform.up * speed * Time.smoothDeltaTime;

        foreach (Barrier barrier in barriers) {
            Vector3 v1 = destination - transform.position;
            Vector3 Vn = -barrier.transform.forward;

            float denom = Vector3.Dot(v1.normalized, Vn);
            float D = barrier.transform.position.magnitude;

            // If moving away from (or parallel to) plane, no intersection for bounce
            if (denom >= 0) {
                continue;
            }

            // Calculate intersection
            float a = Vector3.Dot(barrier.transform.position - transform.position, Vn);
            float b = Vector3.Dot(v1.normalized, Vn);
            float x = a / b;

            Vector3 pon = transform.position + x * v1.normalized;

            // Only bounce if we're close enough
            if ((transform.position - pon).magnitude > 0.1f) {
                continue;
            }

            // Only bounce if intersection is within target circle
            if ((pon - barrier.transform.position).magnitude > barrier.targetCircle.transform.lossyScale.x / 2) {
                continue;
            }

            // Do bounce
            Vector3 vr = 2 * Vector3.Dot(transform.up, Vn) * Vn - transform.up;
            transform.up = vr.normalized * -1f;
        }

        transform.position = destination;

        // Project shadow on all Barriers
        drawShadows();
    }

    private void drawShadows() {
        Barrier[] barriers = GameObject.FindObjectsOfType<Barrier>();

        // Pre-emptively set all shadows to inactive
        foreach (KeyValuePair<int, Shadow> entry in shadows) {
            entry.Value.gameObject.SetActive(false);
        }

        // Create any new shadows, and place all shadows
        foreach (Barrier barrier in barriers) {
            Vector3 Vn = -barrier.transform.forward;

            // Check if shadow is in front of barrier
            if (Vector3.Dot(transform.position - barrier.transform.position, Vn) <= 0f) {
                continue;
            }

            // Intersect shadow with barrier plane
            float d = Vector3.Dot(transform.position - barrier.transform.position, Vn);
            Vector3 pl = d * Vn;
            Vector3 pon = transform.position - pl;

            // Check if intersection is within target circle
            if ((pon - barrier.transform.position).magnitude > barrier.targetCircle.transform.lossyScale.x / 2) {
                continue;
            }

            // Create or place shadow
            Shadow shadow;
            if (shadows.ContainsKey(barrier.GetInstanceID())) {
                shadow = shadows[barrier.GetInstanceID()];
            } else {
                shadow = GameObject.Instantiate(shadowPrefab, transform).GetComponent<Shadow>();
                shadows[barrier.GetInstanceID()] = shadow;
            }

            shadow.Place(
                pon + Vector3.forward * -0.1f,
                Quaternion.Inverse(transform.localRotation) * barrier.transform.localRotation * Quaternion.Euler(90, 0, 0)
            );

            // Set active shadows to active
            shadow.gameObject.SetActive(true);
        }
    }

    // NOTES:
    // - In my implementation Sphere/Plane intersection is defined as when the center
    //   of sphere (ball) is within 0.1 distant from the plane.
    private void checkReflections() {

    }
}
