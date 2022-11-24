using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSwitcher : MonoBehaviour {
    public GameObject mainMenu;
    public GameObject HUD;

    public GameObject rootObject;

    void Start() {
        rootObject.SetActive(false);
        mainMenu.SetActive(true);
        HUD.SetActive(false);
    }

    void Update() {
        // Press any key or mouse button to start game
        if (mainMenu.activeSelf) {
            if (Input.anyKey) {
                rootObject.SetActive(true);
                mainMenu.SetActive(false);
                HUD.SetActive(true);
            }
        }
    }
}
