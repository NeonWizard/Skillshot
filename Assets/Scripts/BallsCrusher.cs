using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsCrusher : MonoBehaviour {
    public string targetTag;

    void Start() {

    }

    void Update() {

    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == targetTag) {
            Destroy(other.gameObject);
        }
    }
}
