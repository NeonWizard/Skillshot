using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class SceneNode : MonoBehaviour {
    protected Matrix4x4 mCombinedParentXform;

    [Header("Positions")]
    public Vector3 NodeOrigin = Vector3.zero;
    public Vector3 Pivot;
    private Vector3 _pivot;

    [Header("Primitives")]
    public List<NodePrimitive> PrimitiveList;

    [Header("Settings")]
    public bool Controllable;

    private Vector3 initialOrigin;
    private Quaternion initialRotation;

    // Use this for initialization
    protected void Start() {
        InitializeSceneNode();
        initialOrigin = NodeOrigin;
        initialRotation = transform.localRotation;
    }

    void OnDrawGizmosSelected() {
        // For helping with pivot placement
        // Gizmos.DrawCube(_pivot, new Vector3(0.22f, 0.22f, 0.22f));
    }

    private void InitializeSceneNode() {
        mCombinedParentXform = Matrix4x4.identity;
    }

    public void Reset() {
        NodeOrigin = initialOrigin;
        transform.position = Vector3.zero;
        transform.localRotation = initialRotation;
        transform.localScale = Vector3.one;

        // propagate to all children
        foreach (Transform child in transform) {
            SceneNode cn = child.GetComponent<SceneNode>();
            if (cn != null) {
                cn.Reset();
            }
        }
    }

    // This must be called _BEFORE_ each draw!!
    public void CompositeXform(ref Matrix4x4 parentXform) {
        Matrix4x4 orgT = Matrix4x4.Translate(NodeOrigin);
        Matrix4x4 p = Matrix4x4.TRS(Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 invp = Matrix4x4.TRS(-Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);

        mCombinedParentXform = parentXform * orgT * p * trs * invp;
        _pivot = mCombinedParentXform.MultiplyPoint(Pivot);

        // propagate to all children
        foreach (Transform child in transform) {
            SceneNode cn = child.GetComponent<SceneNode>();
            if (cn != null) {
                cn.CompositeXform(ref mCombinedParentXform);
            }
        }

        // disenminate to primitives
        foreach (NodePrimitive prim in PrimitiveList) {
            prim.LoadShaderMatrix(ref mCombinedParentXform);
        }
    }
}
