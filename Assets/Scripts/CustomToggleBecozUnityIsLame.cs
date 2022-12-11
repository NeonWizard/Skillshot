using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomToggleBecozUnityIsLame : MonoBehaviour {
    public AudioSource backgroundMusic;

    void Start() {
        GetComponent<Toggle>().onValueChanged.AddListener((val) => backgroundMusic.mute = !val);
    }
}
