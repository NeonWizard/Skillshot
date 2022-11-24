using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AimLine : MonoBehaviour {
    [Header("GameObjects")]
    public GameObject[] lineEndPts;
    public GameObject lineSegment;

    public GameObject travelingBallPrefab;

    [Header("Configurable values")]
    [Range(0.5f, 4f)]
    [Tooltip("The duration in seconds between each traveling ball spawn.")]
    public float spawnInterval = 1f;

    [Range(0.5f, 15f)]
    [Tooltip("The speed each ball is set to when it's spawned.")]
    public float ballSpeed = 5f;

    [Range(1, 15)]
    [Tooltip("The lifespan each ball is given when it's spawned.")]
    public int ballLifespan = 10;

    [Header("Materials")]
    public Material defaultEndpointMaterial;
    public Material glowingEndpointMaterial;
    public Material defaultSegmentMaterial;
    public Material glowingSegmentMaterial;
    public Material reflectedSegmentMaterial;

    [Header("Layers")]
    public LayerMask wallLayer;

    private GameObject _selectedEndPt;
    private GameObject _reflectedLine;

    void Start() {
        // Start the infinite ball spawning loop
        StartCoroutine(SpawnLoop());

        // Subscribe to the lighting controller's changes to make line endpoints glow in the dark
        Lighting.onLightingModeChange += SetGlowingEndpoints;

        _reflectedLine = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        _reflectedLine.GetComponent<Renderer>().material = reflectedSegmentMaterial;
        _reflectedLine.transform.localScale = new Vector3(0.2f, 1f, 0.2f);
        _reflectedLine.SetActive(false);
    }

    void Update() {
        // Update the transform of the line segment to connect the two endpoints
        updateTransform();

        // Clicking to select endpoint for click+drag
        if (Input.GetMouseButtonDown(0)) {
            onMouseDown();
        }

        //  Dragging endpoint
        else if (Input.GetMouseButton(0)) {
            if (_selectedEndPt != null) {
                moveEndpointToMouse();
            }
        }

        // Unselect selected endpoint on mouse up
        else if (Input.GetMouseButtonUp(0)) {
            if (_selectedEndPt != null) {
                _selectedEndPt.GetComponent<Renderer>().material = defaultEndpointMaterial;
                _selectedEndPt = null;
            }
        }
    }

    private void onMouseDown() {
        // If click was on an EventSystem object (UI element), then skip
        if (EventSystem.current.IsPointerOverGameObject()) return;

        // TODO: Combine these raycasts into one

        // Check if clicking on endpoint to select it
        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, 1 << lineEndPts[0].layer);
        if (hit) {
            _selectedEndPt = hitInfo.collider?.gameObject;
            _selectedEndPt.GetComponent<Renderer>().material.color = Color.black;
            return;
        }

        // Otherwise, check if clicking on line segment for deletion
        hitInfo = new RaycastHit();
        hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, 1 << lineSegment.layer);
        if (hit) {
            Destroy(gameObject);
        }
    }

    private void moveEndpointToMouse() {
        if (_selectedEndPt == null) return;

        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, wallLayer);
        if (hit) {
            _selectedEndPt.transform.position = hitInfo.point;
        }
    }

    private void updateTransform() {
        Vector3 v = lineEndPts[1].transform.localPosition - lineEndPts[0].transform.localPosition;
        float length = v.magnitude;

        lineSegment.transform.localPosition = lineEndPts[0].transform.localPosition + v / 2;

        Vector3 scale = lineSegment.transform.localScale;
        scale.y = v.magnitude / 2;
        lineSegment.transform.localScale = scale;

        lineSegment.transform.localRotation = Quaternion.FromToRotation(Vector3.up, v);

        _reflectedLine.SetActive(false);

        // Check for intersection with a barrier
        Barrier[] barriers = GameObject.FindObjectsOfType<Barrier>();
        foreach (Barrier barrier in barriers) {
            Vector3 v1 = lineEndPts[1].transform.position - lineEndPts[0].transform.position;
            Vector3 Vn = -barrier.transform.forward;

            float denom = Vector3.Dot(v1.normalized, Vn);
            float D = barrier.transform.position.magnitude;

            // If moving away from (or parallel to) plane, no intersection for bounce
            if (denom >= 0) {
                continue;
            }

            // Calculate intersection
            float a = Vector3.Dot(barrier.transform.position - lineEndPts[0].transform.position, Vn);
            float b = Vector3.Dot(v1.normalized, Vn);
            float x = a / b;

            Vector3 pon = lineEndPts[0].transform.position + x * v1.normalized;

            // Only bounce if intersection is within target circle
            if ((pon - barrier.transform.position).magnitude > barrier.targetCircle.transform.lossyScale.x / 2) {
                continue;
            }

            // Do bounce
            Vector3 vr = 2 * Vector3.Dot(v1.normalized, Vn) * Vn - v1.normalized;
            Debug.DrawLine(pon, pon - (vr * 10f), Color.green, 1f);

            Vector3 reflectedEndpoint = pon - (vr * (lineEndPts[0].transform.position - pon).magnitude);
            Vector3 reflectedV = (reflectedEndpoint - pon);
            _reflectedLine.transform.position = pon + reflectedV / 2;
            Vector3 reflectedScale = _reflectedLine.transform.localScale;
            reflectedScale.y = reflectedV.magnitude / 2;
            _reflectedLine.transform.localScale = reflectedScale;
            _reflectedLine.transform.localRotation = Quaternion.FromToRotation(Vector3.up, reflectedV);
            _reflectedLine.SetActive(true);
        }
    }

    // ----------------------

    public void SpawnBall() {
        // Instantiate new traveling ball, parented to the AimLine
        GameObject ball = Instantiate(travelingBallPrefab, transform);

        // Set the starting position to the first line end point (leftmost)
        ball.transform.localPosition = lineEndPts[0].transform.localPosition;

        // Set the direction towards the second line end point (rightmost)
        Vector3 v = lineEndPts[1].transform.localPosition - lineEndPts[0].transform.localPosition;
        ball.transform.localRotation = Quaternion.FromToRotation(Vector3.up, v);

        // Set configurable values
        TravelingBall ballComponent = ball.GetComponent<TravelingBall>();
        ballComponent.speed = ballSpeed;
        ballComponent.lifespan = ballLifespan;
    }

    IEnumerator SpawnLoop() {
        while (true) {
            SpawnBall();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void SetGlowingEndpoints(bool glowing) {
        // temporarily disabled for now
        return;

        // if (glowing) {
        //     lineSegment.GetComponent<Renderer>().material = glowingSegmentMaterial;
        //     foreach (GameObject lineEndPt in lineEndPts) {
        //         lineEndPt.GetComponent<Renderer>().material = glowingEndpointMaterial;
        //         lineEndPt.GetComponentInChildren<Light>().enabled = true;
        //     }
        // } else {
        //     lineSegment.GetComponent<Renderer>().material = defaultSegmentMaterial;
        //     foreach (GameObject lineEndPt in lineEndPts) {
        //         lineEndPt.GetComponent<Renderer>().material = defaultEndpointMaterial;
        //         lineEndPt.GetComponentInChildren<Light>().enabled = false;
        //     }
        // }
    }

    public void SetEndpointPosition(int endpointNum, Vector3 position) {
        lineEndPts[endpointNum].transform.position = position;
    }
}
