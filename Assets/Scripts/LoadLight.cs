using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLight : MonoBehaviour {
    public PointLight Light;

    void Update() {
        Light.LoadLightToShader();
    }
}
