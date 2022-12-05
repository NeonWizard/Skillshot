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

        xSlider.slider.onValueChanged.AddListener((value) => MoveCamera(value, 0));
        ySlider.slider.onValueChanged.AddListener((value) => MoveCamera(value, 1));
        zSlider.slider.onValueChanged.AddListener((value) => MoveCamera(value, 2));
    }

    void MoveCamera(float value, int axis) {
        switch (axis) {
            case (0):
                camera.transform.position = startPos + new Vector3(value, 0, 0);
                break;
            case (1):
                camera.transform.position = startPos + new Vector3(0, value, 0);
                break;
            case (2):
                camera.transform.position = startPos + new Vector3(0, 0, value);
                break;
        }
    }
}
