using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneNodeController : MonoBehaviour {
    public Dropdown TheMenu = null;
    public SceneNode TheRoot = null;
    public XFormController XformControl = null;
    //public Transform SelectedObjAxisFrame = null; //not used in this project
    public Camera MainCamera = null;
    private SceneNode previousSceneNode = null;

    const string kChildSpace = "  ";
    List<Dropdown.OptionData> mSelectMenuOptions = new List<Dropdown.OptionData>();

    List<Transform> mSelectedTransform = new List<Transform>();
    float rSpeed = 10f / 6;
    float Direction = 1f;
    private float nextActionTime = 0.0f;
    public float period = 2f;
    private bool first = true;

    void Start() {
        TheMenu.ClearOptions();

        // mSelectMenuOptions.Add(new Dropdown.OptionData(TheRoot.transform.name));
        // mSelectedTransform.Add(TheRoot.transform);
        GetChildrenNames("", TheRoot.transform);
        TheMenu.AddOptions(mSelectMenuOptions);
        TheMenu.onValueChanged.AddListener(SelectionChange);

        SelectionChange(0);
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
    }
}
