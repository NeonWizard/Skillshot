using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingHit : MonoBehaviour {
    void Start() {

    }

    void OnTriggerEnter(Collider other) {
        RoundManager.Instance.RingHit();

        // todo: make ring glow or pulse or something idk
    }
}
