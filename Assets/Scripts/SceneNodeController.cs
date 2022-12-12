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
    List<Transform> mDecorationTransforms = new List<Transform>();
    List<Vector3> mOrigionalDecorationRotations = new List<Vector3>();  
    float rSpeed = 10f/6;
    float Direction = 1f;  
    private float nextActionTime = 0.0f;
    public float period = 2f;
    private bool first = true;

    void Start () {
        TheMenu.ClearOptions();  

        mSelectMenuOptions.Add(new Dropdown.OptionData(TheRoot.transform.name));
        mSelectedTransform.Add(TheRoot.transform);
        GetChildrenNames("", TheRoot.transform);
        TheMenu.AddOptions(mSelectMenuOptions);
        TheMenu.onValueChanged.AddListener(SelectionChange);

        XformControl.SetSelectedObject(TheRoot.gameObject);
        SceneNode cn = TheRoot.GetComponent<SceneNode>();
        previousSceneNode = cn;
    }

    void Update() {
        for (int j = mDecorationTransforms.Count - 1; j >= 0; j--)
        {
            if (Time.time > nextActionTime ) { 
                nextActionTime += period;
                Direction *= -1f;
            }

            // rotate object
            Vector3 newRot = mDecorationTransforms[j].eulerAngles;
            newRot.z += rSpeed*Direction;
            mDecorationTransforms[j].eulerAngles = newRot;
        }
    }

    void GetChildrenNames(string blanks, Transform node)
    {
        string space = blanks + kChildSpace;
        for (int i = node.childCount - 1; i >= 0; i--)
        {
            Transform child = node.GetChild(i);
            SceneNode cn = child.GetComponent<SceneNode>();
            if (cn != null)
            {
                if (child.name != "LeftDecor-Node" && child.name != "RightDecor-Node" && child.name != "TopDecor-Node")
                {
                    mSelectMenuOptions.Add(new Dropdown.OptionData(space + child.name));
                    mSelectedTransform.Add(child);
                    GetChildrenNames(blanks + kChildSpace, child);
                }
                if (child.name == "LeftDecor-Node" || child.name == "RightDecor-Node" || child.name == "TopDecor-Node") // CHANGE TO ELSE ???
                {
                    NodePrimitive np = cn.PrimitiveList[0].GetComponent<NodePrimitive>();
                    Transform decor_transform = np.gameObject.transform;

                    mDecorationTransforms.Add(decor_transform);
                    mOrigionalDecorationRotations.Add(decor_transform.eulerAngles);

                }
            }
        }
    }

    void SelectionChange(int index)
    {
        XformControl.SetSelectedObject(mSelectedTransform[index].gameObject);
        SceneNode cn = mSelectedTransform[index].GetComponent<SceneNode>();
        previousSceneNode = cn;

    }
}
