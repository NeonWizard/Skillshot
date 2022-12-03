using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class UIController : MonoBehaviour {
    public Button toggleShortcutsOverlay;
    public XFormController xFormController;

    public GameObject root;
    public GameObject shortcutsOverlay;

    private GameObject _selectedObject;
    private Material _selectedObjectPreviousMaterial;

    void Start() {
        // spawnIntervalSlider.slider.onValueChanged.AddListener(value => updateLinesFromSliders());
        // speedSlider.slider.onValueChanged.AddListener(value => updateLinesFromSliders());
        // lifespanSlider.slider.onValueChanged.AddListener(value => updateLinesFromSliders());

        toggleShortcutsOverlay.onClick.AddListener(ToggleShortcutsOverlay);
    }

    void ToggleShortcutsOverlay() {
        shortcutsOverlay.SetActive(!shortcutsOverlay.activeSelf);
    }
}
