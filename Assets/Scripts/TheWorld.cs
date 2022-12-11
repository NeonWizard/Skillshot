using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class TheWorld : MonoBehaviour {
    public SceneNode robotArm;

    private void Update() {
        Matrix4x4 i = Matrix4x4.identity;
        robotArm.CompositeXform(ref i);
    }

    private void Reset() {
        robotArm.Reset();
    }
}
