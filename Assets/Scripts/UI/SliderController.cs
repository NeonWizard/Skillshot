using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour {
    public TMPro.TMP_Text textValue;
    public Slider slider;

    void Start() {
        SetValue(slider.value);

        // Add listener for updating UI
        slider.onValueChanged.AddListener(SetValue);
    }

    // Helper function for setting text label value
    private void SetTextValue(float value) {
        textValue.text = string.Format("{0:0.0000}", value);
    }

    // ----------------------

    public void Reset() {
        SetValue(0);
    }

    public void SetValue(float value) {
        slider.SetValueWithoutNotify(value);
        SetTextValue(value);
    }
}
