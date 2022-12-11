using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSliders : MonoBehaviour {
    public SliderController xSlider;
    public SliderController ySlider;
    public SliderController zSlider;

    public new Camera camera;
    private Vector3 startPos;

    void Start() {
        xSlider.slider.minValue = -5f;
        xSlider.slider.maxValue = 5f;

        ySlider.slider.minValue = -5f;
        ySlider.slider.maxValue = 5f;

        zSlider.slider.minValue = -5f;
        zSlider.slider.maxValue = 5f;

        xSlider.Reset();
        ySlider.Reset();
        zSlider.Reset();

        startPos = camera.transform.position;

        xSlider.slider.onValueChanged.AddListener((val) => UpdateCamera());
        ySlider.slider.onValueChanged.AddListener((val) => UpdateCamera());
        zSlider.slider.onValueChanged.AddListener((val) => UpdateCamera());
    }

    void UpdateCamera() {
        camera.transform.position = new Vector3(xSlider.slider.value, ySlider.slider.value, zSlider.slider.value) + startPos;
    }
}
