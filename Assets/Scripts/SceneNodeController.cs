using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneNodeController : MonoBehaviour {
    public Dropdown TheMenu = null;
    public SceneNode TheRoot = null;
    public XFormController XformControl = null;
    public Camera MainCamera = null;
    private SceneNode previousSceneNode = null;

    const string kChildSpace = "  ";
    List<Dropdown.OptionData> mSelectMenuOptions = new List<Dropdown.OptionData>();

    List<Transform> mSelectedTransform = new List<Transform>();

    public int selectedIndex;
    public float rotationSpeed;

    void Start() {
        TheMenu.ClearOptions();

        // mSelectMenuOptions.Add(new Dropdown.OptionData(TheRoot.transform.name));
        // mSelectedTransform.Add(TheRoot.transform);
        GetChildrenNames("", TheRoot.transform);
        TheMenu.AddOptions(mSelectMenuOptions);
        TheMenu.onValueChanged.AddListener(SelectionChange);

        SelectionChange(0);
    }

    void Update() {
        // -- Switching joints
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            SelectionChange(mod(selectedIndex - 1, mSelectedTransform.Count));
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            SelectionChange(mod(selectedIndex + 1, mSelectedTransform.Count));
        }

        // -- Controlling joints
        Quaternion rotation = Quaternion.identity;
        Transform selected = mSelectedTransform[selectedIndex].gameObject.transform;
        // Up/down (x-axis) (W/S)
        if (Input.GetKey(KeyCode.W)) {
            rotation = Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, selected.right);
        }
        if (Input.GetKey(KeyCode.S)) {
            rotation = Quaternion.AngleAxis(-rotationSpeed * Time.deltaTime, selected.right);
        }

        // Left/right (y-axis) (A/D)
        if (Input.GetKey(KeyCode.A)) {
            rotation = Quaternion.AngleAxis(-rotationSpeed * Time.deltaTime, selected.up);
        }
        if (Input.GetKey(KeyCode.D)) {
            rotation = Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, selected.up);
        }

        // Roll (z-axis) (Q/E)
        if (Input.GetKey(KeyCode.Q)) {
            rotation = Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, selected.forward);
        }
        if (Input.GetKey(KeyCode.E)) {
            rotation = Quaternion.AngleAxis(-rotationSpeed * Time.deltaTime, selected.forward);
        }

        Quaternion myRotation = selected.rotation;
        selected.rotation *= (Quaternion.Inverse(myRotation) * rotation * myRotation);
    }

    void GetChildrenNames(string blanks, Transform node) {
        string space = blanks + kChildSpace;
        for (int i = node.childCount - 1; i >= 0; i--) {
            Transform child = node.GetChild(i);
            SceneNode cn = child.GetComponent<SceneNode>();
            if (cn != null) {
                if (cn.Controllable) {
                    mSelectMenuOptions.Add(new Dropdown.OptionData(space + child.name));
                    mSelectedTransform.Add(child);
                    GetChildrenNames(blanks + kChildSpace, child);
                } else {
                    GetChildrenNames(blanks, child);
                }
            }
        }
    }

    void SelectionChange(int index) {
        XformControl.SetSelectedObject(mSelectedTransform[index].gameObject);
        SceneNode cn = mSelectedTransform[index].GetComponent<SceneNode>();
        previousSceneNode = cn;
        selectedIndex = index;
        TheMenu.SetValueWithoutNotify(selectedIndex);
    }

    // why must i suffer
    // https://stackoverflow.com/questions/1082917/mod-of-negative-number-is-melting-my-brain
    int mod(int x, int m) {
        return (x % m + m) % m;
    }
}
