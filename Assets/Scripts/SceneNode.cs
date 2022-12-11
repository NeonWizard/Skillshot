using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneNode : MonoBehaviour {
    protected Matrix4x4 mCombinedParentXform;

    public Vector3 NodeOrigin = Vector3.zero;
    public List<NodePrimitive> PrimitiveList;

    private Vector3 initialOrigin;
    private Quaternion initialRotation;

    // Use this for initialization
    protected void Start() {
        InitializeSceneNode();
        // Debug.Log("PrimitiveList:" + PrimitiveList.Count);
        initialOrigin = NodeOrigin;
        initialRotation = transform.localRotation;
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
        Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);

        mCombinedParentXform = parentXform * orgT * trs;

        // propagate to all children
        foreach (Transform child in transform) {
            SceneNode cn = child.GetComponent<SceneNode>();
            if (cn != null) {
                cn.CompositeXform(ref mCombinedParentXform);
            }
        }

        // disenminate to primitives
        foreach (NodePrimitive p in PrimitiveList) {
            p.LoadShaderMatrix(ref mCombinedParentXform);
        }
    }
}
