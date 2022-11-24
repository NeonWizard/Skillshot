using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimLineCreatable : MonoBehaviour {
    public GameObject root;
    public GameObject aimLinePrefab;

    void OnMouseDown() {
        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, 1 << gameObject.layer);
        if (hit) {
            Vector3 creationSpot = hitInfo.point;

            GameObject aimline = Instantiate(aimLinePrefab, root.transform);
            AimLine cmp = aimline.GetComponent<AimLine>();

            // Set left endpoint where the mouse clicked
            cmp.SetEndpointPosition(0, creationSpot);

            // Try to raycast to an opposing wall to place the right endpoint,
            // otherwise fall back to a point X units away along the wall normal
            Ray ray = new Ray(hitInfo.point + transform.forward * -1f, transform.forward * -1f);
            hit = Physics.Raycast(ray, out hitInfo, Mathf.Infinity, 1 << gameObject.layer);
            if (hit) {
                cmp.SetEndpointPosition(1, hitInfo.point);
            } else {
                cmp.SetEndpointPosition(1, creationSpot + transform.forward * -50f);
            }
        }
    }
}
