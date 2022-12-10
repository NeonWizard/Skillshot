using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotArm : MonoBehaviour {
    [Header("Objects")]
    public GameObject cannon;

    [Header("Prefabs")]
    public GameObject ballPrefab;

    [Header("Variables")]
    public Vector2 ballFireVelocity;

    void Start() {

    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Fire(50f);
        }
    }

    // Fire the cannon on the arm
    void Fire(float velocity) {
        GameObject ball = GameObject.Instantiate(ballPrefab);

        // set transform
        ball.transform.position = cannon.transform.position + cannon.transform.forward * 1f;
        ball.transform.rotation = cannon.transform.rotation;

        // set physics velocity (towards local forward vector)
        ball.GetComponent<Rigidbody>().velocity = cannon.transform.forward * velocity;
    }
}
