using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxController : MonoBehaviour {
    public float rotationAmount = 1f;

    void Start() {

    }

    void Update() {
        RenderSettings.skybox.SetFloat("_Rotation", Time.timeSinceLevelLoad * rotationAmount);
    }
}
