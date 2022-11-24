using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighting : MonoBehaviour {
    public Light directionalLight;

    public delegate void OnLightingModeChange(bool darkmode);
    public static OnLightingModeChange onLightingModeChange;

    void Start() {
        SetLightingMode(false);
    }

    public void SetLightingMode(bool awesome) {
        if (awesome) {
            RenderSettings.ambientLight = new Color(0.1f, 0.1f, 0.1f);
            RenderSettings.ambientEquatorColor = Color.black;
            RenderSettings.ambientGroundColor = Color.black;
        } else {
            RenderSettings.ambientLight = Color.white;
            RenderSettings.ambientEquatorColor = new Color32(29, 32, 34, 255);
            RenderSettings.ambientGroundColor = new Color32(12, 11, 9, 255);
        }

        onLightingModeChange?.Invoke(awesome);
    }
}
