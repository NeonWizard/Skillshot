using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XFormController : MonoBehaviour {
    public TMPro.TMP_Text header;

    public Toggle translationToggle;
    public Toggle scaleToggle;
    public Toggle rotationToggle;

    public SliderController xSlider;
    public SliderController ySlider;
    public SliderController zSlider;

    private GameObject _selectedObject;

    public enum TransformType { Translation, Scale, Rotation };
    private TransformType _currentTransformType;

    public Vector2 translationMinMax = new Vector2(-10, 10);
    public Vector2 scaleMinMax = new Vector2(1, 5);
    public Vector2 rotationMinMax = new Vector2(-180, 180);

    void Start() {
        // Default transform type to Translation
        SetTransformType(TransformType.Translation);

        // Reset sliders
        SetSliders(Vector3.zero);

        // Select Barrier #1 by default
        SetSelectedObject(GameObject.FindObjectOfType<Barrier>()?.gameObject);

        // Add listeners for toggle buttons
        translationToggle.onValueChanged.AddListener((enabled) => {
            if (!enabled) return;
            SetTransformType(TransformType.Translation);
        });
        scaleToggle.onValueChanged.AddListener((enabled) => {
            if (!enabled) return;
            SetTransformType(TransformType.Scale);
        });
        rotationToggle.onValueChanged.AddListener((enabled) => {
            if (!enabled) return;
            SetTransformType(TransformType.Rotation);
        });

        // Add listeners for sliders
        xSlider.slider.onValueChanged.AddListener((value) => {
            UpdateSelectedObjectTransform(_currentTransformType);
        });
        ySlider.slider.onValueChanged.AddListener((value) => {
            UpdateSelectedObjectTransform(_currentTransformType);
        });
        zSlider.slider.onValueChanged.AddListener((value) => {
            UpdateSelectedObjectTransform(_currentTransformType);
        });
    }

    // Poll selected gameobject state and update UI as necessary
    void Update() {
        // Keep UI sliders up to date with current selected gameobject state
        UpdateSliders();
    }

    // Helper function to set the selected name in the header
    private void SetHeaderSelectedName(string name) {
        header.text = string.Format("Selected: <b>{0}</b>", name);
    }

    // Helper function to set min/max of all sliders
    private void SetSliderMinMaxes(Vector2 minmax) {
        xSlider.slider.minValue = minmax.x;
        xSlider.slider.maxValue = minmax.y;
        ySlider.slider.minValue = minmax.x;
        ySlider.slider.maxValue = minmax.y;
        zSlider.slider.minValue = minmax.x;
        zSlider.slider.maxValue = minmax.y;
    }

    // Helper function to set sliders to specific value
    private void SetSliders(Vector3 value) {
        xSlider.SetValue(value.x);
        ySlider.SetValue(value.y);
        zSlider.SetValue(value.z);
    }

    // Helper function to update slider values from current gameobject state
    private void UpdateSliders() {
        if (_selectedObject == null) {
            SetSliders(Vector3.zero);
            return;
        }

        switch (_currentTransformType) {
            case (TransformType.Translation):
                SetSliders(_selectedObject.transform.localPosition);
                break;
            case (TransformType.Scale):
                SetSliders(_selectedObject.transform.localScale);
                break;
            case (TransformType.Rotation):
                // Special case for rotation
                // Reference: https://answers.unity.com/questions/554743/how-to-calculate-transformlocaleuleranglesx-as-neg.html
                Vector3 sliderVector = fixRotationVector(_selectedObject.transform.localEulerAngles);

                SetSliders(sliderVector);
                break;
        }
    }

    // Helper function to set the current transform type we're editing
    private void SetTransformType(TransformType type) {
        _currentTransformType = type;

        // Make sure sliders are updated to the correct values first before setting slider min/max,
        // which otherwise causes an onValueChange to be emitted with the wrong previous slider values
        UpdateSliders();

        switch (type) {
            case (TransformType.Translation):
                SetSliderMinMaxes(translationMinMax);
                translationToggle.SetIsOnWithoutNotify(true);
                break;
            case (TransformType.Scale):
                SetSliderMinMaxes(scaleMinMax);
                scaleToggle.SetIsOnWithoutNotify(true);
                break;
            case (TransformType.Rotation):
                SetSliderMinMaxes(rotationMinMax);
                rotationToggle.SetIsOnWithoutNotify(true);
                break;
        }
    }

    private void UpdateSelectedObjectTransform(TransformType transformType) {
        if (_selectedObject == null) return;

        Vector3 sliderVector = new Vector3(xSlider.slider.value, ySlider.slider.value, zSlider.slider.value);
        switch (transformType) {
            case (TransformType.Translation):
                _selectedObject.transform.localPosition = sliderVector;
                break;
            case (TransformType.Scale):
                _selectedObject.transform.localScale = sliderVector;
                break;
            case (TransformType.Rotation):
                // Special case for rotation
                // Reference: https://answers.unity.com/questions/554743/how-to-calculate-transformlocaleuleranglesx-as-neg.html
                sliderVector = fixRotationVector(sliderVector);

                _selectedObject.transform.localEulerAngles = sliderVector;
                break;
        }
    }

    private Vector3 fixRotationVector(Vector3 rotationVector) {
        return new Vector3(
            (rotationVector.x > 180) ? rotationVector.x - 360 : rotationVector.x,
            (rotationVector.y > 180) ? rotationVector.y - 360 : rotationVector.y,
            (rotationVector.z > 180) ? rotationVector.z - 360 : rotationVector.z
        );
    }

    // ----------------------

    // Selects a new object and updates the widget
    public void SetSelectedObject(GameObject obj) {
        // Early exit if setting to same object
        if (obj == _selectedObject) return;

        // Set widget header name
        SetHeaderSelectedName(obj?.name ?? "None");

        // Reset checkboxes
        SetTransformType(TransformType.Translation);

        // Store newly selected object (or null)
        _selectedObject = obj;
    }
}
