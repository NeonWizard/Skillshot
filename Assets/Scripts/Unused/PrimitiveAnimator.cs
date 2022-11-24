using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class PrimitiveAnimator : MonoBehaviour {
    public enum Direction { X, Y, Z };
    public Direction moveDirection;
    public float moveRange = 5f;

    public float speed = 1f;
    public Vector3 rotationSpeed;

    public Color positiveColor;
    public Color negativeColor;

    private Vector3 moveVector;
    private bool movingForward;

    private new MeshRenderer renderer;

    void Start() {
        switch (moveDirection) {
            case Direction.X:
                moveVector = Vector3.right;
                break;
            case Direction.Y:
                moveVector = Vector3.up;
                break;
            case Direction.Z:
                moveVector = Vector3.forward;
                break;
            default:
                moveVector = Vector3.zero;
                break;
        }

        renderer = GetComponent<MeshRenderer>();
    }

    void Update() {
        // Move
        transform.localPosition += moveVector * speed * Time.smoothDeltaTime * (movingForward ? 1 : -1);

        // Rotate
        transform.eulerAngles += rotationSpeed * Time.smoothDeltaTime;

        // Scaling position by normalized moveVector essentially masks only the position on the axis we care about
        Vector3 offsetOnAxis = Vector3.Scale(transform.localPosition, moveVector);
        float sizeOffset = Vector3.Scale(transform.localScale, moveVector).magnitude / 2;
        // Change color and bounce back
        if (movingForward) {
            if (Vector3.Dot(offsetOnAxis, Vector3.one) > (moveRange + sizeOffset)) {
                movingForward = false;
            }
            renderer.material.color = positiveColor;
        } else {
            if (Vector3.Dot(offsetOnAxis, Vector3.one) < sizeOffset) {
                movingForward = true;
            }
            renderer.material.color = negativeColor;
        }
    }
}
