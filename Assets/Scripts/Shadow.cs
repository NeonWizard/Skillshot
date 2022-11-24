using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This component will only ever attach to the 'Shadow' prefab. This prefab will be a child of the
// gameobject that should cast the shadow. The 'Shadow' prefab should NEVER have a local position other than zero.
public class Shadow : MonoBehaviour {
    public GameObject line;
    public GameObject shadowCircle;

    void Start() {

    }

    void Update() {

    }

    // Place the shadow somewhere. The line will still always originate from the parent gameobject
    public void Place(Vector3 position, Quaternion rotation) {
        Vector3 v = position - transform.position;

        // Place the shadow circle
        shadowCircle.transform.position = position;
        shadowCircle.transform.localRotation = rotation;

        // Scale the shadow circle based on the distance
        float size = Mathf.Lerp(0f, 1f, Mathf.SmoothStep(0f, 1f, 1 - (v.magnitude / 10f)));
        Vector3 shadowScale = shadowCircle.transform.localScale;
        shadowScale.x = shadowScale.z = size;
        shadowCircle.transform.localScale = shadowScale;

        // Place the line
        line.transform.position = transform.position + v / 2;

        Vector3 scale = line.transform.localScale;
        scale.y = v.magnitude / 2;
        line.transform.localScale = scale;

        line.transform.rotation = Quaternion.FromToRotation(Vector3.up, v);
    }
}
