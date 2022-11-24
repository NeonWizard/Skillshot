using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class UIController : MonoBehaviour {
    public SliderController spawnIntervalSlider;
    public SliderController speedSlider;
    public SliderController lifespanSlider;

    public XFormController xFormController;

    public LayerMask selectableLayer;

    public Material materialForSelected;

    public GameObject root;

    private GameObject _selectedObject;
    private Material _selectedObjectPreviousMaterial;

    void Start() {
        spawnIntervalSlider.slider.onValueChanged.AddListener(value => updateLinesFromSliders());
        speedSlider.slider.onValueChanged.AddListener(value => updateLinesFromSliders());
        lifespanSlider.slider.onValueChanged.AddListener(value => updateLinesFromSliders());
    }

    private void updateLinesFromSliders() {
        AimLine[] aimlines = GameObject.FindObjectsOfType<AimLine>();
        foreach (AimLine aimline in aimlines) {
            aimline.spawnInterval = spawnIntervalSlider.slider.value;
            aimline.ballSpeed = speedSlider.slider.value;
            aimline.ballLifespan = Mathf.FloorToInt(lifespanSlider.slider.value);
        }
    }
}
