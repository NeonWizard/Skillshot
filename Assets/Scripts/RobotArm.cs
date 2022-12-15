using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotArm : MonoBehaviour {
    [Header("Objects")]
    public GameObject cannon;

    [Header("Prefabs")]
    public GameObject ballPrefab;
    public GameObject explosion;

    [Header("Variables")]
    public Vector2 ballFireVelocity;

    void Start() {

    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Fire(ballFireVelocity.y);
            StartCoroutine(Shake(.15f, .4f));
        }
    }

    // Fire the cannon on the arm
    void Fire(float velocity) {
        GameObject ball = GameObject.Instantiate(ballPrefab);
        GameObject exp = GameObject.Instantiate(explosion);

        // set transform
        ball.transform.position = cannon.transform.position + cannon.transform.forward * 0.5f;
        ball.transform.rotation = cannon.transform.rotation;

        // set explosion transform
        exp.transform.position = ball.transform.position;
        exp.transform.rotation = ball.transform.rotation;

        // set physics velocity (towards local forward vector)
        ball.GetComponent<Rigidbody>().velocity = cannon.transform.forward * velocity;
    }

    // I know this should be in the camera script but my head hurts and i'm lazy
    // Courtesy of brackeys rest in peace
    public IEnumerator Shake(float duraction, float magnitude) {
        Vector3 originalPos = Camera.main.transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duraction) {
            float x = Random.Range(-.5f, .5f) * magnitude;
            float y = Random.Range(-.5f, .5f) * magnitude;

            Camera.main.transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.localPosition = originalPos;
    }
}
